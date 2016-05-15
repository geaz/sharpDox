using System;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Build.Roslyn.Parser;
using SharpDox.Build.Roslyn.Parser.ProjectParser;
using SharpDox.Model;
using SharpDox.Model.Documentation.Token;
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
        private readonly Dictionary<SDTargetFx, List<SDToken>> _seeTokens;

        public RoslynParser(ParserStrings parserStrings)
        {
            _parserStrings = parserStrings;
            _roslynLoader = new RoslynLoader();
            _targetFxParser = new SDTargetFxParser();
            _seeTokens = new Dictionary<SDTargetFx, List<SDToken>>();
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
            ResolveSeeTokens(parserOptions);
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

                if (!_seeTokens.ContainsKey(targetFx))
                {
                    _seeTokens.Add(targetFx, new List<SDToken>());
                }
                _seeTokens[targetFx].AddRange(parserOptions.SeeTokens);
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
            for (int i = 0; i < parserOptions.SDSolution.Repositories.Count; i++)
            {
                var sdRepository = parserOptions.SDSolution.Repositories[i];
                ExecuteOnStepMessage(string.Format(_parserStrings.ParsingUseings, sdRepository.TargetFx.Name));
                ExecuteOnStepProgress((int)((double)i / parserOptions.SDSolution.Repositories.Count * 40) + 60);

                var useParser = new UseParser(sdRepository);
                useParser.ResolveAllUses();
            }
        }

        private void ResolveSeeTokens(ParserOptions parserOptions)
        {
            for (int i = 0; i < parserOptions.SDSolution.Repositories.Count; i++)
            {
                var sdRepository = parserOptions.SDSolution.Repositories[i];
                ExecuteOnStepMessage(string.Format(_parserStrings.ParsingSeeTokens, sdRepository.TargetFx.Name));
                ExecuteOnStepProgress((int) ((double) i/parserOptions.SDSolution.Repositories.Count*40) + 60);
                
                var seeParser = new SeeParser(sdRepository, _seeTokens[sdRepository.TargetFx]);
                seeParser.ResolveAllSeeTokens();
            }
        }

        private void ExecuteOnDocLanguageFound(string lang)
        {
            var handlers = OnDocLanguageFound;
            handlers?.Invoke(lang);
        }

        private void ExecuteOnStepMessage(string message)
        {
            var handlers = OnStepMessage;
            handlers?.Invoke(message);
        }

        private void ExecuteOnStepProgress(int progress)
        {
            var handlers = OnStepProgress;
            handlers?.Invoke(progress);
        }
    }
}
