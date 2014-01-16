using System.IO;
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
        private ChmExporter _chmExporter;

        public override void ProcessStep(ChmExporter chmExporter)
        {
            _chmExporter = chmExporter;

            _chmExporter.ExecuteOnStepProgress(10);
            _chmExporter.ExecuteOnStepMessage(_chmExporter.ChmStrings.CreateStylesheet);
            CreateStylesheet();

            _chmExporter.ExecuteOnStepProgress(15);
            _chmExporter.ExecuteOnStepMessage(_chmExporter.ChmStrings.CreateToc);
            CreateTocFile();

            _chmExporter.ExecuteOnStepProgress(20);
            _chmExporter.ExecuteOnStepMessage(_chmExporter.ChmStrings.CreateProject);
            CreateProjectFile();

            _chmExporter.ExecuteOnStepProgress(25);
            _chmExporter.ExecuteOnStepMessage(_chmExporter.ChmStrings.CreateArticles);
            CreateArticleFiles();

            _chmExporter.ExecuteOnStepProgress(30);
            _chmExporter.ExecuteOnStepMessage(_chmExporter.ChmStrings.CreateIndex);
            CreateIndexFile();

            chmExporter.ExecuteOnStepProgress(35);
            CreateApiIndexFiles();

            chmExporter.ExecuteOnStepProgress(50);
            CreateTypeFiles();

            chmExporter.CurrentStep = new CompileStep();
        }

        private void CreateStylesheet()
        {
            var styleSheetFile = Path.Combine(_chmExporter.TmpPath, "css", "style.css");
            var template = new StylesheetTemplate { ChmConfig = _chmExporter.ChmConfig };
            File.WriteAllText(styleSheetFile, template.TransformText());
        }

        private void CreateTocFile()
        {
            var tocHtmlFile = Path.Combine(_chmExporter.TmpPath, _chmExporter.Repository.ProjectInfo.ProjectName.Replace(" ", "") + ".hhc");
            var template = new HhcTemplate { SDRepository = _chmExporter.Repository, CurrentLanguage = _chmExporter.CurrentLanguage, Strings = _chmExporter.CurrentStrings };
            File.WriteAllText(tocHtmlFile, template.TransformText());
        }

        private void CreateProjectFile()
        {
            var projectFile = Path.Combine(_chmExporter.TmpPath, _chmExporter.Repository.ProjectInfo.ProjectName.Replace(" ", "") + ".hhp");
            var template = new HhpTemplate { ProjectInfo = _chmExporter.Repository.ProjectInfo };
            File.WriteAllText(projectFile, template.TransformText());
        }

        private void CreateArticleFiles()
        {
            if (_chmExporter.Repository.Articles.Count > 0)
            {
                var articles = _chmExporter.Repository.Articles.ContainsKey(_chmExporter.CurrentLanguage) ? 
                                _chmExporter.Repository.Articles[_chmExporter.CurrentLanguage] : 
                                _chmExporter.Repository.Articles["default"];
                CreateArticles(articles);
            }
        }

        private void CreateArticles(IEnumerable<SDArticle> articles)
        {
            foreach (var article in articles)
            {
                var articleHtmlFile = Path.Combine(_chmExporter.TmpPath, Helper.RemoveIllegalCharacters(article.Title.Replace(" ", "_")) + ".html");

                if (string.IsNullOrEmpty(article.Content) || article.Content == "SDDoc")
                {
                    var template = new EmptyArticleTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDRepository = _chmExporter.Repository,
                        SDArticle = article,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(articleHtmlFile, template.TransformText());
                }
                else
                {
                    var template = new ArticleTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        Repository = _chmExporter.Repository,
                        SDArticle = article,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(articleHtmlFile, template.TransformText());
                }

                CreateArticles(article.Children);
            }
        }

        private void CreateIndexFile()
        {
            var indexHtmlFile = Path.Combine(_chmExporter.TmpPath, _chmExporter.Repository.ProjectInfo.ProjectName.Replace(" ", "") + "-Index.html");
            var template = new IndexTemplate { ProjectInfo = _chmExporter.Repository.ProjectInfo, CurrentLanguage = _chmExporter.CurrentLanguage };
            File.WriteAllText(indexHtmlFile, template.TransformText());
        }

        private void CreateApiIndexFiles()
        {
            foreach (var sdNamespace in _chmExporter.Repository.GetAllNamespaces())
            {
                _chmExporter.ExecuteOnStepMessage(string.Format("{0}: {1}", _chmExporter.ChmStrings.CreateIndexFilesFor, sdNamespace.Fullname));

                var namespaceHtmlFile = Path.Combine(_chmExporter.TmpPath, sdNamespace.Guid + ".html");
                var template = new NamespaceTemplate
                {
                    ProjectInfo = _chmExporter.Repository.ProjectInfo,
                    SDNamespace = sdNamespace,
                    SDRepository = _chmExporter.Repository,
                    CurrentLanguage = _chmExporter.CurrentLanguage,
                    Strings = _chmExporter.CurrentStrings
                };
                File.WriteAllText(namespaceHtmlFile, template.TransformText());

                foreach (var sdType in sdNamespace.Types)
                {
                    var fieldsIndexHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + "-Fields.html");
                    var fieldsTemplate = new FieldsTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDNamespace = sdNamespace,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(fieldsIndexHtmlFile, fieldsTemplate.TransformText());

                    var eveIndexHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + "-Events.html");
                    var eventsTemplate = new EventsTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDNamespace = sdNamespace,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(eveIndexHtmlFile, eventsTemplate.TransformText());

                    var propertiesIndexHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + "-Properties.html");
                    var propertiesTemplate = new PropertiesTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDNamespace = sdNamespace,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(propertiesIndexHtmlFile, propertiesTemplate.TransformText());

                    var constructorsIndexHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + "-Constructors.html");
                    var constructorsTemplate = new ConstructorsTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDNamespace = sdNamespace,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(constructorsIndexHtmlFile, constructorsTemplate.TransformText());

                    var methodsIndexHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + "-Methods.html");
                    var methodsTemplate = new MethodsTemplate
                    {
                        ProjectInfo = _chmExporter.Repository.ProjectInfo,
                        SDNamespace = sdNamespace,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings
                    };
                    File.WriteAllText(methodsIndexHtmlFile, methodsTemplate.TransformText());
                }
            }
        }

        private void CreateTypeFiles()
        {
            foreach (var sdNamespace in _chmExporter.Repository.GetAllNamespaces())
            {
                foreach (var sdType in sdNamespace.Types)
                {
                    _chmExporter.ExecuteOnStepMessage(string.Format("{0}: {1}", _chmExporter.ChmStrings.CreateType, sdType.Name));

                    sdType.SortMembers();
                    var typeHtmlFile = Path.Combine(_chmExporter.TmpPath, sdType.Guid + ".html");

                    var template = new TypeTemplate
                    {
                        SDRepository = _chmExporter.Repository,
                        SDType = sdType,
                        CurrentLanguage = _chmExporter.CurrentLanguage,
                        Strings = _chmExporter.CurrentStrings,
                        TmpFilepath = _chmExporter.TmpPath
                    };

                    if (!sdType.IsClassDiagramEmpty())
                    {
                        sdType.GetClassDiagram().ToPng(Path.Combine(_chmExporter.TmpPath, "diagrams", sdType.Guid + ".png"));
                    }

                    File.WriteAllText(typeHtmlFile, template.TransformText());
                }
            }
        }
    }
}