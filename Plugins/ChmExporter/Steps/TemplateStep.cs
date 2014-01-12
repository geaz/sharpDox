using System.IO;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Chm.Templates.Strings;
using SharpDox.Plugins.Chm.Templates;
using SharpDox.Plugins.Chm.Templates.Sites;
using SharpDox.Plugins.Chm.Templates.Nav;
using System.Collections.Generic;
using SharpDox.Model.Documentation;
using SharpDox.UML;

namespace SharpDox.Plugins.Chm.Steps
{
    internal class TemplateStep : Step
    {
        private string _tmpFilepath;
        private string _currentLanguage;
        private SDRepository _repository;
        private ChmConfig _chmConfig;
        private IStrings _strings;

        public override void ProcessStep(ChmExporter chmExporter)
        {
            _tmpFilepath = chmExporter.TmpPath;
            _currentLanguage = chmExporter.CurrentLanguage;
            _repository = chmExporter.Repository;
            _chmConfig = chmExporter.ChmConfig;
            _strings = chmExporter.CurrentStrings;

            chmExporter.ExecuteOnStepProgress(10);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateStylesheet);
            CreateStylesheet();

            chmExporter.ExecuteOnStepProgress(15);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateIndex);
            CreateIndexFile();

            chmExporter.ExecuteOnStepProgress(20);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateToc);
            CreateTocFile();

            chmExporter.ExecuteOnStepProgress(25);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateProject);
            CreateProjectFile();

            chmExporter.ExecuteOnStepProgress(30);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateArticles);
            CreateArticleFiles();

            chmExporter.ExecuteOnStepProgress(35);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateNamespaces);
            CreateNamespaceFiles();

            chmExporter.ExecuteOnStepProgress(40);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateTypes);
            CreateTypeFiles();

            chmExporter.ExecuteOnStepProgress(45);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateFields);
            CreateFieldFiles();

            chmExporter.ExecuteOnStepProgress(50);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateEvents);
            CreateEventFiles();

            chmExporter.ExecuteOnStepProgress(55);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateProperties);
            CreatePropertyFiles();

            chmExporter.ExecuteOnStepProgress(60);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.CreateMethods);
            CreateMethodFiles();

            chmExporter.CurrentStep = new CompileStep();
        }

        private void CreateStylesheet()
        {
            var styleSheetFile = Path.Combine(_tmpFilepath, "css", "style.css");
            var template = new StylesheetTemplate { ChmConfig = _chmConfig };
            File.WriteAllText(styleSheetFile, template.TransformText());
        }

        private void CreateIndexFile()
        {
            var indexHtmlFile = Path.Combine(_tmpFilepath, _repository.ProjectInfo.ProjectName.Replace(" ", "") + "-Index.html");
            var template = new IndexTemplate { ProjectInfo = _repository.ProjectInfo, CurrentLanguage = _currentLanguage };
            File.WriteAllText(indexHtmlFile, template.TransformText());
        }

        private void CreateTocFile()
        {
            var tocHtmlFile = Path.Combine(_tmpFilepath, _repository.ProjectInfo.ProjectName.Replace(" ", "") + ".hhc");
            var template = new HhcTemplate { SDRepository = _repository, CurrentLanguage = _currentLanguage, Strings = _strings };
            File.WriteAllText(tocHtmlFile, template.TransformText());
        }

        private void CreateProjectFile()
        {
            var projectFile = Path.Combine(_tmpFilepath, _repository.ProjectInfo.ProjectName.Replace(" ", "") + ".hhp");
            var template = new HhpTemplate { ProjectInfo = _repository.ProjectInfo };
            File.WriteAllText(projectFile, template.TransformText());
        }

        private void CreateArticleFiles()
        {
            if (_repository.Articles.Count > 0)
            {
                var articles = _repository.Articles.ContainsKey(_currentLanguage) ? _repository.Articles[_currentLanguage] : _repository.Articles["default"];
                CreateArticles(articles);
            }
        }

        private void CreateArticles(IEnumerable<SDArticle> articles)
        {
            foreach (var article in articles)
            {
                var articleHtmlFile = Path.Combine(_tmpFilepath, Helper.RemoveIllegalCharacters(article.Title.Replace(" ", "_")) + ".html");

                if (string.IsNullOrEmpty(article.Content) || article.Content == "SDDoc")
                {
                    var template = new EmptyArticleTemplate { ProjectInfo = _repository.ProjectInfo, SDRepository = _repository, SDArticle = article, Strings = _strings };
                    File.WriteAllText(articleHtmlFile, template.TransformText());
                }
                else
                {
                    var template = new ArticleTemplate { ProjectInfo = _repository.ProjectInfo, Repository = _repository, SDArticle = article, Strings = _strings };
                    File.WriteAllText(articleHtmlFile, template.TransformText());
                }

                CreateArticles(article.Children);
            }
        }

        private void CreateNamespaceFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                var namespaceHtmlFile = Path.Combine(_tmpFilepath, nameSpace.Guid + ".html");
                var template = new NamespaceTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDRepository = _repository, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(namespaceHtmlFile, template.TransformText());
            }
        }

        private void CreateTypeFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                foreach (var type in nameSpace.Types)
                {
                    type.SortMembers();
                    var typeHtmlFile = Path.Combine(_tmpFilepath, type.Guid + ".html");

                    var template = new TypeTemplate { SDRepository = _repository, SDType = type, CurrentLanguage = _currentLanguage, Strings = _strings };

                    if (!type.IsClassDiagramEmpty())
                    {
                        type.GetClassDiagram().ToPng(Path.Combine(_tmpFilepath, "diagrams", type.Guid + ".png"));
                    }

                    File.WriteAllText(typeHtmlFile, template.TransformText());
                }
            }
        }

        private void CreateFieldFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                foreach (var type in nameSpace.Types)
                {
                    var sdType = _repository.GetTypeByIdentifier(type.Identifier);

                    var fieldsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Fields.html");
                    var fieldsTemplate = new FieldsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(fieldsIndexHtmlFile, fieldsTemplate.TransformText());

                    foreach (var field in sdType.Fields)
                    {
                        var fieldHtmlFile = Path.Combine(_tmpFilepath, field.Guid + ".html");
                        var fieldTemplate = new FieldTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDField = field, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                        File.WriteAllText(fieldHtmlFile, fieldTemplate.TransformText());
                    }
                }
            }
        }

        private void CreateEventFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                foreach (var type in nameSpace.Types)
                {
                    var sdType = _repository.GetTypeByIdentifier(type.Identifier);

                    var eveIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Events.html");
                    var eventsTemplate = new EventsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(eveIndexHtmlFile, eventsTemplate.TransformText());

                    foreach (var eve in sdType.Events)
                    {
                        var eveHtmlFile = Path.Combine(_tmpFilepath, eve.Guid + ".html");
                        var eventTemplate = new EventTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDEvent = eve, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                        File.WriteAllText(eveHtmlFile, eventTemplate.TransformText());
                    }
                }
            }
        }

        private void CreatePropertyFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                foreach (var type in nameSpace.Types)
                {
                    var sdType = _repository.GetTypeByIdentifier(type.Identifier);

                    var propertiesIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Properties.html");
                    var propertiesTemplate = new PropertiesTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(propertiesIndexHtmlFile, propertiesTemplate.TransformText());

                    foreach (var property in sdType.Properties)
                    {
                        var propertyHtmlFile = Path.Combine(_tmpFilepath, property.Guid + ".html");
                        var template = new PropertyTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDProperty = property, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                        File.WriteAllText(propertyHtmlFile, template.TransformText());
                    }
                }
            }
        }

        private void CreateMethodFiles()
        {
            foreach (var nameSpace in _repository.GetAllNamespaces())
            {
                foreach (var type in nameSpace.Types)
                {
                    var sdType = _repository.GetTypeByIdentifier(type.Identifier);

                    var constructorsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Constructors.html");
                    var constructorsTemplate = new ConstructorsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(constructorsIndexHtmlFile, constructorsTemplate.TransformText());

                    var methodsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Methods.html");
                    var methodsTemplate = new MethodsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(methodsIndexHtmlFile, methodsTemplate.TransformText());

                    foreach (var method in sdType.Methods)
                    {
                        var methodHtmlFile = Path.Combine(_tmpFilepath, method.Guid + ".html");
                        var template = new MethodTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDMethod = method, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };

                        if (!method.IsSequenceDiagramEmpty())
                        {
                            method.GetSequenceDiagram(_repository.GetAllTypes()).ToPng(Path.Combine(_tmpFilepath, "diagrams", method.Guid + ".png"));
                        }

                        File.WriteAllText(methodHtmlFile, template.TransformText());
                    }

                    foreach (var constructor in sdType.Constructors)
                    {
                        var constructorHtmlFile = Path.Combine(_tmpFilepath, constructor.Guid + ".html");
                        var template = new MethodTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = nameSpace, SDMethod = constructor, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };

                        if (!constructor.IsSequenceDiagramEmpty())
                        {
                            constructor.GetSequenceDiagram(_repository.GetAllTypes()).ToPng(Path.Combine(_tmpFilepath, "diagrams", constructor.Guid + ".png"));
                        }

                        File.WriteAllText(constructorHtmlFile, template.TransformText());
                    }
                }
            }
        }
    }
}