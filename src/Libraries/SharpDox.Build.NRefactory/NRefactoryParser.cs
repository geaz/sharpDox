using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Build.NRefactory.Parser;
using SharpDox.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public SDRepository GetStructureParsedSolution(string solutionFile)
        {
            var sdRepository = new SDRepository();
            var solution = LoadSolution(solutionFile, 3);

            StructureParseNamespaces(solution, sdRepository);
            StructureParseTypes(solution, sdRepository);

            return sdRepository;
        }

        public SDRepository GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens)
        {
            var sdRepository = new SDRepository();
            var solution = LoadSolution(solutionFile, 5);

            ParseNamespaces(solution, sdRepository, sharpDoxConfig, tokens);
            ParseTypes(solution, sdRepository, sharpDoxConfig);
            ParseMethodCalls(solution, sdRepository);
            ResolveUses(sdRepository);

            return sdRepository;
        }

        private CSharpSolution LoadSolution(string solutionFile, int steps)
        {
            var solution = new CSharpSolution();
            solution.OnLoadingProject += (m) => ExecuteOnStepMessage(string.Format(_parserStrings.ReadingProject, m));
            solution.OnLoadedProject += (t, i) => ExecuteOnStepProgress((int)(((double)i/(double)t) * 100 / steps));
            solution.LoadSolution(solutionFile);

            return solution;
        }

        private void StructureParseNamespaces(CSharpSolution solution, SDRepository sdRepository)
        {
            var pi = 0;
            for (int i = 0; i < solution.Projects.Count; i++)
            {
                pi = i;
                var types = solution.Projects[i].Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
                for (int j = 0; j < types.Count; j++)
                {
                    PostParseProgress(_parserStrings.ParsingNamespace + ": " + types[j].Namespace, j, types.Count, pi, solution.Projects.Count, 1, 3);

                    var sdNamespace = new SDNamespace(types[j].Namespace);
                    sdRepository.AddNamespace(sdNamespace);
                }
            }
        }

        private void StructureParseTypes(CSharpSolution solution, SDRepository sdRepository)
        {
            var pi = 0;
            for (int i = 0; i < solution.Projects.Count; i++)
            {
                pi = i;
                var types = solution.Projects[i].Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
                for (int j = 0; j < types.Count; j++)
                {
                    var type = types[j];
                    if (types[j].Kind != TypeKind.Delegate)
                    {
                        PostParseProgress(_parserStrings.ParsingClass + ": " + string.Format("{0}.{1}", types[j].Namespace, types[j].Name), j, types.Count, pi, solution.Projects.Count, 2, 3);

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
        }

        private void ParseNamespaces(CSharpSolution solution, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens)
        {
            var pi = 0;
            var namespaceParser = new NamespaceParser(sdRepository, sharpDoxConfig, sharpDoxConfig.InputFile, tokens);
            namespaceParser.OnDocLanguageFound += ExecuteOnDocLanguageFound;
            namespaceParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingNamespace + ": " + n, i, t, pi, solution.Projects.Count, 1, 5); };

            for (int i = 0; i < solution.Projects.Count; i++)
            {
                pi = i;
                namespaceParser.ParseProjectNamespaces(solution.Projects[i]);
            }
        }

        private void ParseTypes(CSharpSolution solution, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig)
        {
            var pi = 0;
            var typeParser = new TypeParser(sdRepository, sharpDoxConfig);
            typeParser.OnItemParseStart += (n, i, t) => { PostParseProgress(_parserStrings.ParsingClass + ": " + n, i, t, pi, solution.Projects.Count, 2, 5); };

            for (int i = 0; i < solution.Projects.Count; i++)
            {
                pi = i;
                typeParser.ParseProjectTypes(solution.Projects[i]);
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
