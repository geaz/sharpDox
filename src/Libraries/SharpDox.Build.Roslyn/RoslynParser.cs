using System;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Build.Roslyn.Parser;
using SharpDox.Build.Roslyn.Parser.ProjectParser;
using SharpDox.Model;
using SharpDox.Model.Repository;
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
            ResolveUses(parserOptions);

            return sdSolution;
        }

        private void ParseProjects(ParserOptions parserOptions)
        {
            var projects = parserOptions.CodeSolution.Projects.ToList();
            for (int i = 0; i < projects.Count; i++)
            {
                var project = projects[i];
                ExecuteOnStepMessage(string.Format(_parserStrings.CompilingAndParsing, project.Name));
                ExecuteOnStepProgress((int)((double)i / projects.Count * 50));

                var projectCompilation = project.GetCompilationAsync().Result;
                var targetFx = _targetFxParser.GetTargetFx(project.FilePath);
                var sdRepository = parserOptions.SDSolution.GetExistingOrNew(targetFx);
                
                parserOptions.SDRepository = sdRepository;

                var nparser = new NamespaceParser(parserOptions);
                nparser.ParseProjectNamespacesRecursively(projectCompilation.Assembly.GlobalNamespace);
            }
            ExecuteOnStepProgress(40);
        }

        private void CleanUpNamespaces(SDSolution sdSolution)
        {
            for(int i = 0; i < sdSolution.Repositories.Count; i++)
            {
                var sdRepository = sdSolution.Repositories[i];
                ExecuteOnStepMessage(string.Format(_parserStrings.CleanUp, sdRepository.TargetFx.Name));
                ExecuteOnStepProgress((int)((double)i / sdSolution.Repositories.Count * 10) + 50);

                foreach (var sdNamespace in sdRepository.GetAllNamespaces())
                {
                    if (sdNamespace.Types.Count == 0) sdRepository.RemoveNamespace(sdNamespace);
                }
            }
            ExecuteOnStepProgress(50);
        }

        private void ParseMethodCalls(ParserOptions parserOptions)
        {
            for (int i = 0; i < parserOptions.SDSolution.Repositories.Count; i++)
            {
                var sdRepository = parserOptions.SDSolution.Repositories[i];
                ExecuteOnStepMessage(string.Format(_parserStrings.ParsingMethod, sdRepository.TargetFx.Name));
                ExecuteOnStepProgress((int)((double)i / parserOptions.SDSolution.Repositories.Count * 40) + 60);

                var methodParser = new MethodCallParser(parserOptions);
                methodParser.ParseMethodCalls();
            }
        }

        private void ResolveUses(ParserOptions parserOptions)
        { 
            var useParser = new UseParser(parserOptions); 
            useParser.OnItemParseStart += (n) => { ExecuteOnStepMessage(_parserStrings.ParsingUseings + ": " + n); }; 
            useParser.ResolveAllUses(); 
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
