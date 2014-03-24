using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpDox.Build.Parser
{
    internal class ArticleParser
    {
        private readonly SDRepository _sdRepository;
        private readonly IEnumerable<string> _navFiles;
        private readonly IEnumerable<string> _articles;

        public ArticleParser(string inputPath, SDRepository sdRepository)
        {
            _sdRepository = sdRepository;
            _navFiles = Directory.EnumerateFiles(Path.GetDirectoryName(inputPath), "*.sdnav", SearchOption.AllDirectories);
            _articles = Directory.EnumerateFiles(Path.GetDirectoryName(inputPath), "*.sda", SearchOption.AllDirectories);
        }

        public Dictionary<string, List<SDArticle>> ParseArticles()
        {
            var articles = new Dictionary<string, List<SDArticle>>();

            foreach (var language in _sdRepository.DocumentationLanguages)
            {
                var languageArticles = new List<SDArticle>();
                var levelNodes = new List<SDArticle>();

                var navFile = GetNavFileByLanguage(language);

                if (navFile != null)
                {
                    foreach (var line in File.ReadAllLines(navFile))
                    {
                        var article = GetArticle(line, language);
                        var navLevel = GetNavLevel(line);

                        if (levelNodes.Count < navLevel)
                        {
                            levelNodes.Add(article);
                        }
                        else
                        {
                            levelNodes[navLevel - 1] = article;
                        }

                        if (navLevel == 1)
                        {
                            languageArticles.Add(article);
                        }
                        else
                        {
                            article.Parent = levelNodes[navLevel - 2];
                            levelNodes[navLevel - 2].Children.Add(article);
                        }
                    }
                }

                if (languageArticles.Count > 0) articles.Add(language, languageArticles);
            }

            return articles;
        }

        private string GetNavFileByLanguage(string language)
        {
            return _navFiles.SingleOrDefault(n => Path.GetFileNameWithoutExtension(n) == language) ?? _navFiles.SingleOrDefault(n => Path.GetFileNameWithoutExtension(n).ToLower() == "default");
        }

        private SDArticle GetArticle(string line, string language)
        {
            var splitted = line.Split('#');
            var articleFilename = string.Empty;
            var articleContent = string.Empty;

            if (splitted.Length > 1)
            {
                if (splitted[1] == "SDDoc")
                {
                    articleContent = "SDDoc";
                }
                else
                {
                    articleFilename = language.ToLower() != "default" ? string.Format("{0}.{1}", language, splitted[1]) : splitted[1];
                    var articleFile = _articles.SingleOrDefault(a => Path.GetFileNameWithoutExtension(a) == articleFilename);
                    articleContent = !string.IsNullOrEmpty(articleFile) ? File.ReadAllText(articleFile) : string.Empty;
                }
            }

            var article = new SDArticle();
            article.Title = GetNavTitle(splitted[0]);
            article.Filename = articleFilename;
            article.Content = articleContent;

            return article;
        }

        private string GetNavTitle(string navEntry)
        {
            return navEntry.Substring(0, 1) == "-" ? GetNavTitle(navEntry.Substring(1)) : navEntry;
        }

        private int GetNavLevel(string navEntry)
        {
            var level = 0;
            if (navEntry.Substring(0, 1) == "-") level++;
            if (navEntry.Substring(1, 1) == "-") level += GetNavLevel(navEntry.Substring(1));

            return level;
        }
    }
}
