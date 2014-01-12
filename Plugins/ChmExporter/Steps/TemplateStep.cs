using System.IO;
using SharpDox.Model.Repository;
using SharpDox.Plugins.Chm.Templates.Strings;
using SharpDox.Plugins.Chm.Templates;
using SharpDox.Plugins.Chm.Templates.Sites;
using SharpDox.Plugins.Chm.Templates.Nav;
using System.Collections.Generic;
using SharpDox.Model.Documentation;
using SharpDox.UML;
using System.Threading.Tasks;

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
            CreateDocumentationFiles(chmExporter);

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
            Parallel.ForEach(articles, article =>
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
            });
        }

        private void CreateDocumentationFiles(ChmExporter chmExporter)
        {
            var currentNamespaceNumber = 0;
            var namespaceCount = _repository.GetAllNamespaces().Count;

            Parallel.ForEach(_repository.GetAllNamespaces(), sdNamespace =>
            {
                chmExporter.ExecuteOnStepMessage(sdNamespace.Fullname);
                chmExporter.ExecuteOnStepProgress(((++currentNamespaceNumber / namespaceCount) * 75) + 35);

                var namespaceHtmlFile = Path.Combine(_tmpFilepath, sdNamespace.Guid + ".html");
                var namespaceTemplate = new NamespaceTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDRepository = _repository, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(namespaceHtmlFile, namespaceTemplate.TransformText());

                CreateTypeFiles(sdNamespace);
            });
        }

        private void CreateTypeFiles(SDNamespace sdNamespace)
        {
            Parallel.ForEach(sdNamespace.Types, sdType =>
            {
                sdType.SortMembers();
                var typeHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + ".html");

                var typeTemplate = new TypeTemplate { SDRepository = _repository, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };

                if (!sdType.IsClassDiagramEmpty())
                {
                    sdType.GetClassDiagram().ToPng(Path.Combine(_tmpFilepath, "diagrams", sdType.Guid + ".png"));
                }
                File.WriteAllText(typeHtmlFile, typeTemplate.TransformText());

                var fieldsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Fields.html");
                var fieldsTemplate = new FieldsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(fieldsIndexHtmlFile, fieldsTemplate.TransformText());

                Parallel.ForEach(sdType.Fields, sdField =>
                {
                    var fieldHtmlFile = Path.Combine(_tmpFilepath, sdField.Guid + ".html");
                    var fieldTemplate = new FieldTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDField = sdField, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(fieldHtmlFile, fieldTemplate.TransformText());
                });

                var eveIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Events.html");
                var eventsTemplate = new EventsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(eveIndexHtmlFile, eventsTemplate.TransformText());

                Parallel.ForEach(sdType.Events, sdEvent =>
                {
                    var eveHtmlFile = Path.Combine(_tmpFilepath, sdEvent.Guid + ".html");
                    var eventTemplate = new EventTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDEvent = sdEvent, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(eveHtmlFile, eventTemplate.TransformText());
                });

                var propertiesIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Properties.html");
                var propertiesTemplate = new PropertiesTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(propertiesIndexHtmlFile, propertiesTemplate.TransformText());

                Parallel.ForEach(sdType.Properties, sdProperty =>
                {
                    var propertyHtmlFile = Path.Combine(_tmpFilepath, sdProperty.Guid + ".html");
                    var propertyTemplate = new PropertyTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDProperty = sdProperty, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                    File.WriteAllText(propertyHtmlFile, propertyTemplate.TransformText());
                });

                var constructorsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Constructors.html");
                var constructorsTemplate = new ConstructorsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(constructorsIndexHtmlFile, constructorsTemplate.TransformText());

                var methodsIndexHtmlFile = Path.Combine(_tmpFilepath, sdType.Guid + "-Methods.html");
                var methodsTemplate = new MethodsTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };
                File.WriteAllText(methodsIndexHtmlFile, methodsTemplate.TransformText());

                Parallel.ForEach(sdType.Methods, sdMethod =>
                {
                    var methodHtmlFile = Path.Combine(_tmpFilepath, sdMethod.Guid + ".html");
                    var template = new MethodTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDMethod = sdMethod, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };

                    if (!sdMethod.IsSequenceDiagramEmpty())
                    {
                        sdMethod.GetSequenceDiagram(_repository.GetAllTypes()).ToPng(Path.Combine(_tmpFilepath, "diagrams", sdMethod.Guid + ".png"));
                    }

                    File.WriteAllText(methodHtmlFile, template.TransformText());
                });

                Parallel.ForEach(sdType.Constructors, sdConstructor =>
                {
                    var constructorHtmlFile = Path.Combine(_tmpFilepath, sdConstructor.Guid + ".html");
                    var template = new MethodTemplate { ProjectInfo = _repository.ProjectInfo, SDNamespace = sdNamespace, SDMethod = sdConstructor, SDType = sdType, CurrentLanguage = _currentLanguage, Strings = _strings };

                    if (!sdConstructor.IsSequenceDiagramEmpty())
                    {
                        sdConstructor.GetSequenceDiagram(_repository.GetAllTypes()).ToPng(Path.Combine(_tmpFilepath, "diagrams", sdConstructor.Guid + ".png"));
                    }

                    File.WriteAllText(constructorHtmlFile, template.TransformText());
                });
            });
        }
    }
}
