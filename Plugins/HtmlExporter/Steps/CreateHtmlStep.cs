using System;
using System.IO;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Html.Templates.Strings;
using System.Text;
using SharpDox.Plugins.Html.Templates.Nav;
using System.Collections.Generic;
using SharpDox.Model.Documentation;

namespace SharpDox.Plugins.Html.Steps
{
    public class CreateHtmlStep : Step
    {
        public override void ProcessStep(HtmlExporter htmlExporter)
        {
            htmlExporter.ExecuteOnStepProgress(50);

            CreateHtml(htmlExporter, htmlExporter.Repository, htmlExporter.CurrentStrings, htmlExporter.CurrentLanguage, htmlExporter.OutputPath);

            htmlExporter.ExecuteOnStepProgress(100);
            htmlExporter.CurrentStep = null;
        }

        private void CreateHtml(HtmlExporter htmlExporter, SDRepository repository, IStrings strings, string docLanguage, string outputPath)
        {
            var navJson = GetNavJson(repository, strings, docLanguage);
            var indexTemplate = new IndexTemplate { Repository = repository, Strings = strings, CurrentLanguage = docLanguage, NavJson = navJson };
            File.WriteAllText(Path.Combine(outputPath, "index.html"), indexTemplate.TransformText());

            var homeTemplate = new HomeTemplate { Repository = repository, Strings = strings, CurrentLanguage = docLanguage };
            File.WriteAllText(Path.Combine(outputPath, "article", "home.html"), homeTemplate.TransformText());

            var namespaceCount = 0d;
            var namespaceTotal = repository.GetAllNamespaces().Count;
            foreach (var nameSpace in repository.GetAllNamespaces())
            {
                htmlExporter.ExecuteOnStepProgress(Convert.ToInt16((namespaceCount / namespaceTotal) * 50) + 50);
                htmlExporter.ExecuteOnStepMessage(htmlExporter.HtmlStrings.CreateFilesForNamespace + ": " + nameSpace.Fullname);
                namespaceCount++;

                var namespaceTemplate = new NamespaceTemplate { Strings = strings, CurrentLanguage = docLanguage, Namespace = nameSpace, Repository = repository };
                File.WriteAllText(Path.Combine(outputPath, "namespace", nameSpace.Guid + ".html"), namespaceTemplate.TransformText());

                foreach (var type in nameSpace.Types)
                {
                    type.SortMembers();
                    var typeTemplate = new TypeTemplate
                    {
                        Strings = strings,
                        CurrentLanguage = type.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                        SDType = type,
                        Repository = repository
                    };
                    File.WriteAllText(Path.Combine(outputPath, "type", type.Guid + ".html"), typeTemplate.TransformText());
                }
            }
        }

        private string GetNavJson(SDRepository repository, IStrings strings, string currentLanguage)
        {
            var indexNavTemplate = new IndexNavTemplate { Repository = repository, Strings = strings, CurrentLanguage = currentLanguage };
            var indexNav = indexNavTemplate.TransformText().Trim();

            return indexNav;
        }
    }
}
