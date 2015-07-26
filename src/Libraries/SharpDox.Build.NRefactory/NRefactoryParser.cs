using ICSharpCode.NRefactory.MonoCSharp;
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

        private readonly ParserStrings _parserStrings;

        public NRefactoryParser(ParserStrings parserStrings)
        {
            _parserStrings = parserStrings;
        }

        public SDSolution GetStructureParsedSolution(string solutionFile)
        {
            var solution = LoadSolution(solutionFile, 3);
            return ParseSolution(solution, null, null, true);
        }

        public SDSolution GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens)
        {
            var solution = LoadSolution(solutionFile, 5);
            return ParseSolution(solution, sharpDoxConfig, tokens, false);
        }

        private SDSolution ParseSolution(CSharpSolution solution, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens, bool structured)
        {
            var sdSolution = new SDSolution(solution.SolutionFile);
            var targetFxParser = new SDTargetFxParser();

            for (var i = 0; i < solution.Projects.Count; i++)
            {
                var project = solution.Projects[i];
                var projectFileName = project.FileName;
                var targetFx = targetFxParser.GetTargetFx(projectFileName);

                var sdRepository = sdSolution.GetExistingOrNew(targetFx);
                if (structured)
                {
                    StructureParseNamespaces(project, sdRepository, i, solution.Projects.Count);
                    StructureParseTypes(project, sdRepository, i, solution.Projects.Count);
                }
                else
                {
                    ParseNamespaces(project, sdRepository, sharpDoxConfig, tokens, i, solution.Projects.Count);
                    ParseTypes(project, sdRepository, sharpDoxConfig, i, solution.Projects.Count);

                    // Because of excluding privates, internals and protected members
                    // it is possible, that a namespace has no visible namespaces at all.
                    // It is necessary to remove empty namespaces.
                    RemoveEmptyNamespaces(sdRepository);
                }
            }

            if (!structured)
            {
                foreach (var sdRepository in sdSolution.Repositories)
                {
                    ParseMethodCalls(solution, sdRepository);
                    ResolveUses(sdRepository);
                }
            }

            return sdSolution;
        }

        private CSharpSolution LoadSolution(string solutionFile, int steps)
        {
            var solution = new CSharpSolution();

            solution.OnLoadingProject += (m) => ExecuteOnStepMessage(string.Format(_parserStrings.ReadingProject, m));
            solution.OnLoadedProject += (t, i) => ExecuteOnStepProgress((int)(((double)i / (double)t) * 100 / steps));

            solution.LoadSolution(solutionFile);

            return solution;
        }

        private void StructureParseNamespaces(CSharpProject project, SDRepository sdRepository, int currentProject, int totalProjects)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int j = 0; j < types.Count; j++)
            {
                PostParseProgress(_parserStrings.ParsingNamespace + ": " + types[j].Namespace, j, types.Count, currentProject, totalProjects, 1, 3);

                var sdNamespace = new SDNamespace(types[j].Namespace);
                sdRepository.AddNamespace(sdNamespace);
            }
        }

        private void StructureParseTypes(CSharpProject project, SDRepository sdRepository, int currentProject, int totalProjects)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int j = 0; j < types.Count; j++)
            {
                var type = types[j];
                if (types[j].Kind != TypeKind.Delegate)
                {
                    PostParseProgress(_parserStrings.ParsingClass + ": " + string.Format("{0}.{1}", types[j].Namespace, types[j].Name), j, types.Count, currentProject, totalProjects, 2, 3);

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

        private void ParseNamespaces(CSharpProject project, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens, int currentProject, int totalProjects)
        {
            var namespaceParser = new NamespaceParser(sdRepository, sharpDoxConfig, sharpDoxConfig.InputFile, tokens);
            namespaceParser.OnDocLanguageFound += ExecuteOnDocLanguageFound;
            namespaceParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingNamespace + ": " + n, i, t, currentProject, totalProjects, 1, 5); };

            namespaceParser.ParseProjectNamespaces(project);
        }

        private void ParseTypes(CSharpProject project, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig, int currentProject, int totalProjects)
        {
            var typeParser = new TypeParser(sdRepository, sharpDoxConfig);
            typeParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingClass + ": " + n, i, t, currentProject, totalProjects, 2, 5); };

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
            var pi = 0;
            var methodCallParser = new MethodCallParser(sdRepository, solution);
            methodCallParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingMethod + ": " + n, i, t, pi, sdRepository.GetAllNamespaces().Count, 3, 5); };

            var namespaces = sdRepository.GetAllNamespaces();
            for (int i = 0; i < namespaces.Count; i++)
            {
                pi = i;
                methodCallParser.ParseMethodCalls(namespaces[i]);
            }
        }

        private void ResolveUses(SDRepository sdRepository)
        {
            var useParser = new UseParser(sdRepository);
            useParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingUseings + ": " + n, i, t, 0, 1, 4, 5); };

            useParser.ResolveAllUses();
        }

        private void PostParseProgress(string message, double itemIndex, double itemTotal, double parentIndex, double parentTotal, int step, int stepsTotal)
        {
            var percentage = ((((itemIndex / itemTotal) * (100d / parentTotal)) + (parentIndex * (100d / parentTotal))) / stepsTotal) + ((double)step / (double)stepsTotal * 100d);

            ExecuteOnStepMessage(message);
            ExecuteOnStepProgress((int)percentage);
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
