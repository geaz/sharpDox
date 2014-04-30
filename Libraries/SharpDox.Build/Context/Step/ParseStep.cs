using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SharpDox.Build.Loader;
using SharpDox.Build.Parser;
using SharpDox.Model.Repository;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class ParseStep
    {
        private CSharpSolution _solution;
        private SDRepository _repository;
        private List<string> _excludedIdentifiers;

        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;
        private readonly ICoreConfigSection _coreConfigSection;

        public ParseStep(SDBuildStrings sdBuildStrings, ICoreConfigSection coreConfigSection, BuildMessenger buildMessenger)
        {
            _coreConfigSection = coreConfigSection;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;
        }

        public SDRepository ParseSolution(CSharpSolution solution, List<string> excludedIdentifiers)
        {
            _solution = solution;
            _excludedIdentifiers = excludedIdentifiers ?? new List<string>();
            _repository = new SDRepository();

            _buildMessenger.ExecuteOnStepProgress(0);
            _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.ParsingSolution);

            GetProjectInfos();
            GetImages();
            ParseNamespaces();
            ParseTypes();
            ParseArticles();
            ParseMethodCalls();
            ResolveUses();

            _buildMessenger.ExecuteOnStepProgress(100);

            return _repository;
        }

        private void GetProjectInfos()
        {
            var potentialReadMes = Directory.EnumerateFiles(Path.GetDirectoryName(_coreConfigSection.InputPath), "*readme*");
            if (potentialReadMes.Any())
            {
                foreach (var readme in potentialReadMes)
                {
                    var splitted = Path.GetFileName(readme).Split('.');
                    if (splitted.Length > 0 && CultureInfo.GetCultures(CultureTypes.NeutralCultures).Any(c => c.TwoLetterISOLanguageName == splitted[0].ToLower()))
                    {
                        if (!_repository.ProjectInfo.Description.ContainsKey(splitted[0].ToLower()))
                        {
                            _repository.ProjectInfo.Description.Add(splitted[0].ToLower(), File.ReadAllText(readme));
                            _repository.AddDocumentationLanguage(splitted[0].ToLower());
                        }
                    }
                    else if (splitted.Length > 0 && splitted[0].ToLower() == "readme" && !_repository.ProjectInfo.Description.ContainsKey("default"))
                    {
                        _repository.ProjectInfo.Description.Add("default", File.ReadAllText(readme));
                    }
                }
            }

            _repository.ProjectInfo.DocLanguage = _coreConfigSection.DocLanguage;
            _repository.ProjectInfo.LogoPath = _coreConfigSection.LogoPath;
            _repository.ProjectInfo.Author = _coreConfigSection.Author;
            _repository.ProjectInfo.ProjectName = _coreConfigSection.ProjectName;
            _repository.ProjectInfo.VersionNumber = _coreConfigSection.VersionNumber;
        }

        private void GetImages()
        {
            var images = Directory.EnumerateFiles(Path.GetDirectoryName(_coreConfigSection.InputPath), "sdi.*", SearchOption.AllDirectories);
            foreach (var image in images)
            {
                _repository.Images.Add(image);
            }
        }

        private void ParseArticles()
        {
            var articleParser = new ArticleParser(_coreConfigSection.InputPath, _repository);
            _repository.Articles = articleParser.ParseArticles();
        }

        private void ParseNamespaces()
        {
            var pi = 0;
            var namespaceParser = new NamespaceParser(_repository, _excludedIdentifiers, _coreConfigSection);
            namespaceParser.OnItemParseStart += (n, i, t) => { PostProgress(_sdBuildStrings.ParsingNamespace + ": " + n, i, t, pi, _solution.Projects.Count); };

            for (int i = 0; i < _solution.Projects.Count; i++)
            {
                pi = i;
                namespaceParser.ParseProjectNamespaces(_solution.Projects[i]);
            }
        }

        private void ParseTypes()
        {
            var pi = 0;
            var typeParser = new TypeParser(_repository, _excludedIdentifiers);
            typeParser.OnItemParseStart += (n, i, t) => { PostProgress(_sdBuildStrings.ParsingClass + ": " + n, i, t, pi, _solution.Projects.Count); };

            for (int i = 0; i < _solution.Projects.Count; i++)
            {
                pi = i;
                typeParser.ParseProjectTypes(_solution.Projects[i]);
            }
        }

        private void ParseMethodCalls()
        {
            var pi = 0;
            var methodCallParser = new MethodCallParser(_repository, _solution);
            methodCallParser.OnItemParseStart += (n, i, t) => { PostProgress(_sdBuildStrings.ParsingMethod + ": " + n, i, t, pi, _repository.GetAllNamespaces().Count); };

            var namespaces = _repository.GetAllNamespaces();
            for (int i = 0; i < namespaces.Count; i++)
            {
                pi = i;
                methodCallParser.ParseMethodCalls(namespaces[i]);
            }
        }

        private void ResolveUses()
        {
            var useParser = new UseParser(_repository);
            useParser.OnItemParseStart += (n, i, t) => { PostProgress(_sdBuildStrings.ParsingUseings + ": " + n, i, t, 0, 1); };

            useParser.ResolveAllUses();
        }

        private void PostProgress(string message, double itemIndex, double itemTotal, double parentIndex, double parentTotal)
        {
            var percentage = ((itemIndex / itemTotal) * (100d / parentTotal)) + (parentIndex * (100d / parentTotal));

            _buildMessenger.ExecuteOnStepMessage(message);
            _buildMessenger.ExecuteOnStepProgress((int)percentage);
        }
    }
}
