using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Build.NRefactory.Parser;
using SharpDox.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Model;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory
{
    public class NRefactoryParser : ICodeParser
    {
        public event Action<string> OnDocLanguageFound;
        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;

        private int _currentProjectIndex;
        private int _totalProjects;

        private readonly ParserStrings _parserStrings;

        public NRefactoryParser(ParserStrings parserStrings)
        {
            _parserStrings = parserStrings;
        }

        public SDSolution GetStructureParsedSolution(string solutionFile)
        {
            var solution = LoadSolution(solutionFile);
            return ParseSolution(solution, null, null, true);
        }

        public SDSolution GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens)
        {
            var solution = LoadSolution(solutionFile);
            return ParseSolution(solution, sharpDoxConfig, tokens, false);
        }

        private SDSolution ParseSolution(CSharpSolution solution, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens, bool structured)
        {
            var sdSolution = new SDSolution(solution.SolutionFile);
            var targetFxParser = new SDTargetFxParser();

            _currentProjectIndex = 1;
            _totalProjects = solution.Projects.Count;
            for (var i = 0; i < solution.Projects.Count; i++)
            {
                ExecuteOnStepProgress((int)(((double)i / (double)solution.Projects.Count) * 50) + 20);

                var project = solution.Projects[i];
                var projectFileName = project.FileName;
                var targetFx = targetFxParser.GetTargetFx(projectFileName);

                var sdRepository = sdSolution.GetExistingOrNew(targetFx);
                if (structured)
                {
                    StructureParseNamespaces(project, sdRepository);
                    StructureParseTypes(project, sdRepository);
                }
                else
                {
                    ParseNamespaces(project, sdRepository, sharpDoxConfig, tokens);
                    ParseTypes(project, sdRepository, sharpDoxConfig);

                    // Because of excluding privates, internals and protected members
                    // it is possible, that a namespace has no visible namespaces at all.
                    // It is necessary to remove empty namespaces.
                    RemoveEmptyNamespaces(sdRepository);
                }
                _currentProjectIndex++;
            }

            ExecuteOnStepProgress(80);
            if (!structured)
            {
                var i = 0;
                foreach (var sdRepository in sdSolution.Repositories)
                {
                    ParseMethodCalls(solution, sdRepository);
                    ResolveUses(sdRepository);
                }
            }

            return sdSolution;
        }

        private CSharpSolution LoadSolution(string solutionFile)
        {
            var solution = new CSharpSolution();

            solution.OnLoadingProject += (m) => ExecuteOnStepMessage(string.Format(_parserStrings.ReadingProject, m));
            solution.OnLoadedProject += (t, i) => ExecuteOnStepProgress((int)(((double)i / (double)t) * 20));

            solution.LoadSolution(solutionFile);

            return solution;
        }

        private void StructureParseNamespaces(CSharpProject project, SDRepository sdRepository)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int j = 0; j < types.Count; j++)
            {
                PostParseMessage(_parserStrings.ParsingNamespace + ": " + types[j].Namespace);

                var sdNamespace = new SDNamespace(types[j].Namespace);
                sdRepository.AddNamespace(sdNamespace);
            }
        }

        private void StructureParseTypes(CSharpProject project, SDRepository sdRepository)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int j = 0; j < types.Count; j++)
            {
                var type = types[j];
                if (types[j].Kind != TypeKind.Delegate)
                {
                    PostParseMessage(_parserStrings.ParsingClass + ": " + string.Format("{0}.{1}", types[j].Namespace, types[j].Name));

                    var nameSpace = sdRepository.GetNamespaceByIdentifier(type.Namespace);
                    var namespaceRef = nameSpace ?? new SDNamespace(type.Namespace) { IsProjectStranger = true };

                    var sdType = new SDType(type.GetIdentifier(), type.Name, namespaceRef)
                    {
                        Accessibility = type.GetDefinition().Accessibility.ToString().ToLower()
                    };

                    sdRepository.AddType(sdType);

                    EventParser.ParseMinimalFields(sdType, types[j]);
                    PropertyParser.ParseMinimalProperties(sdType, types[j]);
                    FieldParser.ParseMinimalFields(sdType, types[j]);
                    MethodParser.ParseMinimalConstructors(sdType, types[j]);
                    MethodParser.ParseMinimalMethods(sdType, types[j]);

                    sdRepository.AddNamespaceTypeRelation(types[j].Namespace, sdType.Identifier);
                }
            }
        }

        private void ParseNamespaces(CSharpProject project, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens)
        {
            var namespaceParser = new NamespaceParser(sdRepository, sharpDoxConfig, sharpDoxConfig.InputFile, tokens);
            namespaceParser.OnDocLanguageFound += ExecuteOnDocLanguageFound;
            namespaceParser.OnItemParseStart += (n) => { PostParseMessage(_parserStrings.ParsingNamespace + ": " + n); };

            namespaceParser.ParseProjectNamespaces(project);
        }

        private void ParseTypes(CSharpProject project, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig)
        {
            var typeParser = new TypeParser(sdRepository, sharpDoxConfig);
            typeParser.OnItemParseStart += (n) => { PostParseMessage(_parserStrings.ParsingClass + ": " + n); };

            typeParser.ParseProjectTypes(project);
        }

        private void RemoveEmptyNamespaces(SDRepository repository)
        {
            foreach (var sdNamespace in repository.GetAllNamespaces())
            {
                if (sdNamespace.Types.Count == 0)
                {
                    repository.RemoveNamespace(sdNamespace);
                }
            }
        }

        private void ParseMethodCalls(CSharpSolution solution, SDRepository sdRepository)
        {
            var methodCallParser = new MethodCallParser(sdRepository, solution);
            methodCallParser.OnItemParseStart += (n) => { ExecuteOnStepMessage(_parserStrings.ParsingMethod + ": " + n); };
            methodCallParser.ParseMethodCalls();            
        }

        private void ResolveUses(SDRepository sdRepository)
        {
            var useParser = new UseParser(sdRepository);
            useParser.OnItemParseStart += (n) => { ExecuteOnStepMessage(_parserStrings.ParsingUseings + ": " + n); };
            useParser.ResolveAllUses();
        }

        private void PostParseMessage(string message)
        {
            ExecuteOnStepMessage(string.Format("(Project {0}/{1}) - {2}", _currentProjectIndex, _totalProjects, message));
        }

        private void ExecuteOnDocLanguageFound(string lang)
        {
            var handlers = OnDocLanguageFound;
            if (handlers != null)
            {
                handlers(lang);
            }
        }

        private void ExecuteOnStepMessage(string message)
        {
            var handlers = OnStepMessage;
            if (handlers != null)
            {
                handlers(message);
            }
        }

        private void ExecuteOnStepProgress(int progress)
        {
            var handlers = OnStepProgress;
            if (handlers != null)
            {
                handlers(progress);
            }
        }
    }
}
