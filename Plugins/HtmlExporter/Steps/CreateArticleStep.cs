using System.IO;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Html.Templates.Strings;

namespace SharpDox.Plugins.Html.Steps
{
    public class CreateArticleStep : Step
    {
        public override void ProcessStep(HtmlExporter htmlExporter)
        {
            htmlExporter.ExecuteOnStepProgress(20);

            if (htmlExporter.Repository.Articles.Count > 0)
            {
                var articles = htmlExporter.Repository.Articles.ContainsKey(htmlExporter.CurrentLanguage)
                    ? htmlExporter.Repository.Articles[htmlExporter.CurrentLanguage]
                    : htmlExporter.Repository.Articles["default"];

                foreach (var article in articles)
                {
                    htmlExporter.ExecuteOnStepMessage(string.Format(htmlExporter.HtmlStrings.CreatingArticle, article.Title));
                    CreateArticle(article, htmlExporter.Repository, htmlExporter.CurrentStrings, htmlExporter.OutputPath);
                }
            }

            htmlExporter.CurrentStep = new CopyStep();
        }

        private void CreateArticle(SDArticle article, SDRepository repository, IStrings strings, string outputPath)
        {
            if (!string.IsNullOrEmpty(article.Content) && article.Content != "SDDoc")
            {
                var articleTemplate = new ArticleTemplate { Repository = repository, Article = article, Strings = strings };
                File.WriteAllText(Path.Combine(outputPath, "article", string.Format("{0}.html", article.Title.Replace(" ", "_"))), articleTemplate.TransformText());
            }

            if (article.Content != "SDDoc")
            {
                foreach (var child in article.Children)
                {
                    CreateArticle(child, repository, strings, outputPath);
                }
            }
        }
    }
}
