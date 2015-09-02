using SharpDox.Model;
using SharpDox.Model.Documentation;
using SharpDox.Model.Documentation.Article;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpDox.Build
{
    internal class SDNavParser
    {
        private List<string> _articleIds = new List<string>(); 

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
                    var article = GetArticle(line, sdProject.Tokens);
                    if (article is SDDocPlaceholder)
                    {
                        var solutionFile = ((SDDocPlaceholder)article).SolutionFile;
                        sdProject.AddSolution(solutionFile);
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

        private SDArticle GetArticle(string line, Dictionary<string, string> tokens)
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
                else if(articleFile != string.Empty && articleFile.StartsWith("http://") || articleFile.StartsWith("https://"))
                {
                    article = new SDArticleLink
                    {
                        Title = GetNavTitle(splitted[0]),
                        Link = articleFile
                    };
                }
                else if(articleFile != string.Empty)
                {
                    var filename = Path.GetFileNameWithoutExtension(articleFile);
                    article = new SDArticle()
                    {
                        Id = CreateArticleId(filename),
                        Title = GetNavTitle(splitted[0]),
                        Filename = filename,
                        Content = new SDTemplate(File.ReadAllText(articleFile), tokens)
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

        private string CreateArticleId(string title, int i = 0)
        {
            var id = title;
            if (_articleIds.Contains(id))
            {
                id = CreateArticleId(string.Format("{0}{1}", title, ++i));
            }
            _articleIds.Add(id);

            return id;
        }

        private string GetArticleFile(string articleFileName)
        {
            if(articleFileName.StartsWith("http://") || articleFileName.StartsWith("https://"))
            {
                return articleFileName;
            }

            var articleFile = Path.GetFullPath(Path.Combine(_referencePath, articleFileName));
            if (File.Exists(articleFile))
            {
                return articleFile;
            }
            else
            {
                articleFile = _articles.SingleOrDefault(a => Path.GetFileNameWithoutExtension(a.ToLower()) == articleFileName.ToLower());
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
