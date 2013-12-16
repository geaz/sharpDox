using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Plugins.Html.Templates.Nav;
using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Html.Templates.Strings;

namespace SharpDox.Plugins.Html.Steps
{
    public class CreateNavigationStep : Step
    {
        public override void ProcessStep(HtmlExporter htmlExporter)
        {
            htmlExporter.ExecuteOnStepProgress(5);
            htmlExporter.ExecuteOnStepMessage(htmlExporter.HtmlStrings.CreatingNavigation);
            CreateIndexNav(htmlExporter.Repository, htmlExporter.CurrentStrings, htmlExporter.CurrentLanguage, htmlExporter.OutputPath);

            htmlExporter.ExecuteOnStepProgress(10);
            CreateNamespaceNavs(htmlExporter.Repository, htmlExporter.CurrentStrings, htmlExporter.OutputPath);

            htmlExporter.ExecuteOnStepProgress(15);
            CreateArticleNavs(htmlExporter.Repository, htmlExporter.CurrentStrings, htmlExporter.CurrentLanguage, htmlExporter.OutputPath);

            htmlExporter.CurrentStep = new CreateArticleStep();
        }

        private void CreateIndexNav(SDRepository repository, IStrings strings, string currentLanguage, string outputPath)
        {
            var indexNavTemplate = new IndexNavTemplate { Repository = repository, Strings = strings, CurrentLanguage = currentLanguage };
            File.WriteAllText(Path.Combine(outputPath, "nav", "index.html"), indexNavTemplate.TransformText());
        }

        private void CreateNamespaceNavs(SDRepository repository, IStrings strings, string outputPath)
        {
            foreach (var sdNamespace in repository.GetAllNamespaces())
            {
                var namespaceTemplate = new NamespaceNavTemplate { Namespace = sdNamespace, Strings = strings, ApiUrl = repository.Articles.Count == 0 ? "index" : "api" };
                File.WriteAllText(Path.Combine(outputPath, "nav", sdNamespace.Fullname + ".html"), namespaceTemplate.TransformText());
            }
        }

        private void CreateArticleNavs(SDRepository repository, IStrings strings, string currentLanguage, string outputPath)
        {
            if (repository.Articles.Count > 0)
            {
                var articles = repository.Articles.ContainsKey(currentLanguage)
                    ? repository.Articles[currentLanguage]
                    : repository.Articles["default"];
                CreateArticleNavs(articles, repository, strings, currentLanguage, outputPath);
            }
        }

        private void CreateArticleNavs(IEnumerable<SDArticle> articles, SDRepository repository, IStrings strings, string currentLanguage, string outputPath)
        {
            foreach (var article in articles)
            {
                if (article.Children.Count > 0)
                {
                    var artivleNavTemplate = new ArticleNavTemplate { Article = article, Repository = repository, Strings = strings, CurrentLanguage = currentLanguage };
                    File.WriteAllText(Path.Combine(outputPath, "nav", article.Title.Replace(" ", "_") + ".html"), artivleNavTemplate.TransformText());

                    CreateArticleNavs(article.Children, repository, strings, currentLanguage, outputPath);
                }
                else if (article.Content == "SDDoc")
                {
                    var apiNavWrapperTemplate = new ApiNavWrapperTemplate { Article = article, Repository = repository, Strings = strings, CurrentLanguage = currentLanguage };
                    File.WriteAllText(Path.Combine(outputPath, "nav", "api.html"), apiNavWrapperTemplate.TransformText());
                }
            }
        }
    }
}
