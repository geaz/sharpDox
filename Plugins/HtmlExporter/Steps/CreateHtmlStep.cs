using System;
using System.IO;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Html.Templates.Strings;
using System.Threading.Tasks;

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
            var indexTemplate = new IndexTemplate { Repository = repository, Strings = strings, CurrentLanguage = docLanguage };
            File.WriteAllText(Path.Combine(outputPath, "index.html"), indexTemplate.TransformText());

            var namespaceCount = 0d;
            var namespaceTotal = repository.GetAllNamespaces().Count;

            Parallel.ForEach(repository.GetAllNamespaces(), sdNamespace =>
            {
                htmlExporter.ExecuteOnStepProgress(Convert.ToInt16((namespaceCount / namespaceTotal) * 50) + 50);
                htmlExporter.ExecuteOnStepMessage(htmlExporter.HtmlStrings.CreateFilesForNamespace + ": " + sdNamespace.Fullname);
                namespaceCount++;

                var namespaceTemplate = new NamespaceTemplate { Strings = strings, CurrentLanguage = docLanguage, Namespace = sdNamespace, Repository = repository };
                File.WriteAllText(Path.Combine(outputPath, "namespace", sdNamespace.Guid + ".html"), namespaceTemplate.TransformText());

                Parallel.ForEach(sdNamespace.Types, sdType =>
                {
                    sdType.SortMembers();
                    var typeTemplate = new TypeTemplate
                    {
                        Strings = strings,
                        CurrentLanguage = sdType.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                        SDType = sdType,
                        Repository = repository
                    };
                    File.WriteAllText(Path.Combine(outputPath, "type", sdType.Guid + ".html"), typeTemplate.TransformText());

                    Parallel.ForEach(sdType.Constructors, sdConstructor =>
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = sdConstructor.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = sdType,
                            SDMember = sdConstructor,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "constructor", sdConstructor.Guid + ".html"), memberTemplate.TransformText());
                    });

                    Parallel.ForEach(sdType.Methods, sdMethod =>
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = sdMethod.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = sdType,
                            SDMember = sdMethod,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "method", sdMethod.Guid + ".html"), memberTemplate.TransformText());
                    });

                    Parallel.ForEach(sdType.Fields, sdFields =>
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = sdFields.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = sdType,
                            SDMember = sdFields,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "field", sdFields.Guid + ".html"), memberTemplate.TransformText());
                    });

                    Parallel.ForEach(sdType.Properties, sdProperty =>
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = sdProperty.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = sdType,
                            SDMember = sdProperty,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "property", sdProperty.Guid + ".html"), memberTemplate.TransformText());
                    });

                    Parallel.ForEach(sdType.Events, sdEvent =>
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = sdEvent.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = sdType,
                            SDMember = sdEvent,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "event", sdEvent.Guid + ".html"), memberTemplate.TransformText());
                    });
                });
            });
        }
    }
}
