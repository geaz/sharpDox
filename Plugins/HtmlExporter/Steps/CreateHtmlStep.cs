using System;
using System.IO;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Html.Templates.Strings;

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

                    foreach (var constructor in type.Constructors)
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = constructor.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = type,
                            SDMember = constructor,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "constructor", constructor.Guid + ".html"), memberTemplate.TransformText());
                    }
                    foreach (var method in type.Methods)
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = method.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = type,
                            SDMember = method,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "method", method.Guid + ".html"), memberTemplate.TransformText());
                    }
                    foreach (var field in type.Fields)
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = field.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = type,
                            SDMember = field,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "field", field.Guid + ".html"), memberTemplate.TransformText());
                    }
                    foreach (var property in type.Properties)
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = property.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = type,
                            SDMember = property,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "property", property.Guid + ".html"), memberTemplate.TransformText());
                    }
                    foreach (var eve in type.Events)
                    {
                        var memberTemplate = new MemberTemplate
                        {
                            Strings = strings,
                            CurrentLanguage = eve.Documentation.ContainsKey(docLanguage) ? docLanguage : "default",
                            SDType = type,
                            SDMember = eve,
                            Repository = repository
                        };
                        File.WriteAllText(Path.Combine(outputPath, "event", eve.Guid + ".html"), memberTemplate.TransformText());
                    }
                }
            }
        }
    }
}
