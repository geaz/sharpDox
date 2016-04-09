using System;
using System.Collections.Generic;
using SharpDox.Build.Roslyn.Parser;
using SharpDox.Model;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Roslyn
{
    public class RoslynParser : ICodeParser
    {
        public event Action<string> OnDocLanguageFound;
        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;

        private readonly ParserStrings _parserStrings;
        private readonly RoslynLoader _roslynLoader;
        private readonly SDTargetFxParser _targetFxParser;

        public RoslynParser(ParserStrings parserStrings)
        {
            _parserStrings = parserStrings;
            _roslynLoader = new RoslynLoader();
            _targetFxParser = new SDTargetFxParser();
        }

        public SDSolution GetParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens, bool parseMethodCalls)
        {
            var sdSolution = new SDSolution(solutionFile);
            var solution = _roslynLoader.LoadSolutionFile(solutionFile);

            var parserOptions = new ParserOptions();
            parserOptions.CodeSolution = solution;
            parserOptions.SDSolution = sdSolution;
            parserOptions.SharpDoxConfig = sharpDoxConfig;
            parserOptions.Tokens = tokens;

            ParseProjects(parserOptions);
            CleanUpNamespaces(sdSolution);
            if (parseMethodCalls) ParseMethodCalls(parserOptions);

            return sdSolution;
        }

        private void ParseProjects(ParserOptions parserOptions)
        {
            foreach (var project in parserOptions.CodeSolution.Projects)
            {
                ExecuteOnStepMessage(string.Format(_parserStrings.Compiling, project.Name));
                var projectCompilation = project.GetCompilationAsync().Result;

                var targetFx = _targetFxParser.GetTargetFx(project.FilePath);
                var sdRepository = parserOptions.SDSolution.GetExistingOrNew(targetFx);
                
                parserOptions.SDRepository = sdRepository;

                var nparser = new NamespaceParser(parserOptions);
                nparser.ParseProjectNamespacesRecursively(projectCompilation.Assembly.GlobalNamespace);
            }
        }

        private void CleanUpNamespaces(SDSolution sdSolution)
        {
            foreach (var sdRepository in sdSolution.Repositories)
            {
                ExecuteOnStepMessage(string.Format(_parserStrings.Compiling, sdRepository.TargetFx.Name));

                foreach (var sdNamespace in sdRepository.GetAllNamespaces())
                {
                    if (sdNamespace.Types.Count == 0) sdRepository.RemoveNamespace(sdNamespace);
                }
            }
        }

        private void ParseMethodCalls(ParserOptions parserOptions)
        {
            foreach (var sdRepository in parserOptions.SDSolution.Repositories)
            {
                ExecuteOnStepMessage(string.Format(_parserStrings.Compiling, sdRepository.TargetFx.Name));

                var methodParser = new MethodCallParser(parserOptions);
                methodParser.ParseMethodCalls();
            }
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
