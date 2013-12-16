using SharpDox.Sdk.Local;

namespace SharpDox.Plugins.Html
{
    public class HtmlStrings : ILocalStrings
    {
        private string _creatingFolders = "Creating Folders";
        private string _creatingNavigation = "Creating navigation files";
        private string _createFilesForNamespace = "Creating files for namespace";
        private string _creatingArticle = "Creating article: {0}";
        private string _copyingFile = "Copying File: {0}";

        public string DisplayName { get { return "HtmlExporter"; } }

        public string CreatingFolders
        {
            get { return _creatingFolders; }
            set { _creatingFolders = value; }
        }

        public string CreateFilesForNamespace
        {
            get { return _createFilesForNamespace; }
            set { _createFilesForNamespace = value; }
        }

        public string CreatingNavigation
        {
            get { return _creatingNavigation; }
            set { _creatingNavigation = value; }
        }

        public string CreatingArticle
        {
            get { return _creatingArticle; }
            set { _creatingArticle = value; }
        }

        public string CopyingFile
        {
            get { return _copyingFile; }
            set { _copyingFile = value; }
        }
    }
}
