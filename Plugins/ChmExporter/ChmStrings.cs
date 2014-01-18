using SharpDox.Sdk.Local;

namespace SharpDox.Plugins.Chm
{
    public class ChmStrings : ILocalStrings
    {
        private string _description = "This exporter creates a Windows Helpfile (chm). It supports multilanguage documentations, articles and namespace descriptions.";
        private string _start = "Starting chm exporter";
        private string _createStylesheet = "Creating stylesheet";
        private string _createIndex = "Creating index file";
        private string _createToc = "Creating toc file";
        private string _createProject = "Creating project file";
        private string _createArticles = "Creating article files";
        private string _createIndexFilesFor = "Creating index files for";
        private string _createType = "Creating type";
        private string _compiling = "Compiling chm";
        private string _saving = "Saving chm to output";
        private string _cleaning = "Cleaning temporary folder";
        private string _defaultColorsHeaderText = "Default Colors";
        private string _customColorHeaderText = "Custom Color";
        private string _reset = "Reset";
        private string _backgroundColor = "Background Color";
        private string _textColor = "Text Color";
        private string _linkColor = "Link Color";
        private string _linkHoverColor = "Link Hover Color";
        private string _tableHeaderBackgroundColor = "Tableheader Color";
        private string _tableHeaderBorderColor = "Tableheader Border Color";
        private string _breadCrumbBackgroundColor = "Breadcrumb Background Color";
        private string _breadCrumbBorderColor = "Breadcrumb Border Color";
        private string _breadCrumbLinkColor = "Breadcrumb Link Color";
        private string _breadCrumbLinkHoverColor = "Breadcrumb Link Hover Color";
        private string _syntaxBoxBackgroundColor = "Syntaxbox Background Color";
        private string _syntaxBoxBorderColor = "Syntaxbox Border Color";
        private string _syntaxBoxTextColor = "Syntaxbox Text Color";

        public string DisplayName { get { return "ChmExporter"; } }

        public string Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public string CreateStylesheet
        {
            get { return _createStylesheet; }
            set { _createStylesheet = value; }
        }

        public string CreateIndex
        {
            get { return _createIndex; }
            set { _createIndex = value; }
        }

        public string CreateToc
        {
            get { return _createToc; }
            set { _createToc = value; }
        }

        public string CreateProject
        {
            get { return _createProject; }
            set { _createProject = value; }
        }

        public string CreateArticles
        {
            get { return _createArticles; }
            set { _createArticles = value; }
        }

        public string Compiling
        {
            get { return _compiling; }
            set { _compiling = value; }
        }

        public string Saving
        {
            get { return _saving; }
            set { _saving = value; }
        }

        public string Cleaning
        {
            get { return _cleaning; }
            set { _cleaning = value; }
        }

        public string DefaultColorsHeaderText
        {
            get { return _defaultColorsHeaderText; }
            set { _defaultColorsHeaderText = value; }
        }

        public string CustomColorHeaderText
        {
            get { return _customColorHeaderText; }
            set { _customColorHeaderText = value; }
        }

        public string Reset
        {
            get { return _reset; }
            set { _reset = value; }
        }

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public string TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public string LinkColor
        {
            get { return _linkColor; }
            set { _linkColor = value; }
        }

        public string LinkHoverColor
        {
            get { return _linkHoverColor; }
            set { _linkHoverColor = value; }
        }

        public string TableHeaderBackgroundColor
        {
            get { return _tableHeaderBackgroundColor; }
            set { _tableHeaderBackgroundColor = value; }
        }

        public string TableHeaderBorderColor
        {
            get { return _tableHeaderBorderColor; }
            set { _tableHeaderBorderColor = value; }
        }

        public string BreadCrumbBackgroundColor
        {
            get { return _breadCrumbBackgroundColor; }
            set { _breadCrumbBackgroundColor = value; }
        }

        public string BreadCrumbBorderColor
        {
            get { return _breadCrumbBorderColor; }
            set { _breadCrumbBorderColor = value; }
        }

        public string BreadCrumbLinkColor
        {
            get { return _breadCrumbLinkColor; }
            set { _breadCrumbLinkColor = value; }
        }

        public string BreadCrumbLinkHoverColor
        {
            get { return _breadCrumbLinkHoverColor; }
            set { _breadCrumbLinkHoverColor = value; }
        }

        public string SyntaxBoxBackgroundColor
        {
            get { return _syntaxBoxBackgroundColor; }
            set { _syntaxBoxBackgroundColor = value; }
        }

        public string SyntaxBoxBorderColor
        {
            get { return _syntaxBoxBorderColor; }
            set { _syntaxBoxBorderColor = value; }
        }

        public string SyntaxBoxTextColor
        {
            get { return _syntaxBoxTextColor; }
            set { _syntaxBoxTextColor = value; }
        }

        public string CreateIndexFilesFor
        {
            get { return _createIndexFilesFor; }
            set { _createIndexFilesFor = value; }
        }

        public string CreateType
        {
            get { return _createType; }
            set { _createType = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
