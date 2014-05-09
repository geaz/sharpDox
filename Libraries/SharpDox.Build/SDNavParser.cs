using SharpDox.Model;
using SharpDox.Model.Documentation.Article;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpDox.Build
{
    internal class SDNavParser
    {
        private readonly string _referencePath;
        private readonly IEnumerable<string> _articles;

        public SDNavParser(string inputFile)
        {
            _referencePath = Path.GetDirectoryName(inputFile);
            _articles = Directory.EnumerateFiles(_referencePath, "*.sda", SearchOption.AllDirectories);            
        }

        public SDProject ParseNavFile(string navFile, SDProject sdProject)
        {
            var navFileLanguage = Path.GetFileNameWithoutExtension(navFile);
            sdProject.AddDocumentationLanguage(navFileLanguage);

            var articles = new List<SDArticle>();
            var levelNodes = new List<SDArticle>();

            if (navFile != null)
            {
                foreach (var line in File.ReadAllLines(navFile))
                {
                    var article = GetArticle(line);
                    if (article is SDDocPlaceholder)
                    {
                        var solutionFile = ((SDDocPlaceholder)article).SolutionFile;
                        sdProject.AddRepository(solutionFile);
                    }

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
                        articles.Add(article);
                    }
                    else
                    {
                        article.Parent = levelNodes[navLevel - 2];
                        levelNodes[navLevel - 2].Children.Add(article);
                    }
                }
            }
            
            sdProject.Articles.Add(navFileLanguage, articles);
            return sdProject;
        }

        private SDArticle GetArticle(string line)
        {
            var splitted = line.Split('#');
            SDArticle article = null;

            if (splitted.Length > 1)
            {
                var articleFile = GetArticleFile(splitted[1]);
                if (articleFile != string.Empty && Path.GetExtension(articleFile) == ".csproj" || Path.GetExtension(articleFile) == ".sln")
                {
                    article = new SDDocPlaceholder
                    {
                        Title = GetNavTitle(splitted[0]),
                        SolutionFile = articleFile
                    };
                }
                else if(articleFile != string.Empty)
                {
                    article = new SDArticle
                    {
                        Title = GetNavTitle(splitted[0]),
                        Filename = Path.GetFileNameWithoutExtension(articleFile),
                        Content = File.ReadAllText(articleFile)
                    };
                }
            }
            else
            {
                article = new SDArticlePlaceholder
                {
                    Title = GetNavTitle(splitted[0])
                };
            }

            return article;
        }

        private string GetNavTitle(string navEntry)
        {
            return navEntry.Substring(0, 1) == "-" ? GetNavTitle(navEntry.Substring(1)) : navEntry;
        }

        private string GetArticleFile(string articleFileName)
        {
            var articleFile = Path.GetFullPath(Path.Combine(_referencePath, articleFileName));
            if (File.Exists(articleFile))
            {
                return articleFile;
            }
            else
            {
                articleFile = _articles.SingleOrDefault(a => Path.GetFileNameWithoutExtension(a) == articleFileName);
                if (File.Exists(articleFile))
                {
                    return articleFile;
                }
            }

            return string.Empty;
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
