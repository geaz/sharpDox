using SharpDox.Sdk.Local;

namespace SharpDox.GUI
{
    public class SDGuiStrings : ILocalStrings
    {
        private string _build = "Build";
        private string _generalSettings = "General Settings";
        private string _exportSettings = "Exportsettings";
        private string _selectExportSetting = "select an export option";
        private string _visibilitySettings = "Visibility Settings";
        private string _new = "New";
        private string _load = "Load";
        private string _save = "Save";
        private string _saveAs = "Save as";
        private string _recentProjects = "Recent Projects";
        private string _start = "Start";
        private string _stop = "Stop";
        private string _projectName = "Projectname";
        private string _versionnumber = "Versionnumber";
        private string _author = "Author / Company";
        private string _logo = "Logo";
        private string _projectPath = "Projectpath";
        private string _docLanguage = "Doc Language";
        private string _outputPath = "Outputpath";
        private string _pleaseWait = "Please wait...";
        private string _seeBuild = "See build page for details!";
        private string _lastBuild = "LAST BUILD";

        public string DisplayName { get { return "SharpDoxGui"; } }

        public string GeneralSettings
        {
            get { return _generalSettings; }
            set { _generalSettings = value; }
        }

        public string ExportSettings
        {
            get { return _exportSettings; }
            set { _exportSettings = value; }
        }

        public string Build
        {
            get { return _build; }
            set { _build = value; }
        }

        public string SelectExportSetting
        {
            get { return _selectExportSetting; }
            set { _selectExportSetting = value; }
        }

        public string VisibilitySettings
        {
            get { return _visibilitySettings; }
            set { _visibilitySettings = value; }
        }

        public string New
        {
            get { return _new; }
            set { _new = value; }
        }

        public string Load
        {
            get { return _load; }
            set { _load = value; }
        }

        public string Save
        {
            get { return _save; }
            set { _save = value; }
        }

        public string SaveAs
        {
            get { return _saveAs; }
            set { _saveAs = value; }
        }

        public string RecentProjects
        {
            get { return _recentProjects; }
            set { _recentProjects = value; }
        }

        public string Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public string Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public string Versionnumber
        {
            get { return _versionnumber; }
            set { _versionnumber = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public string ProjectPath
        {
            get { return _projectPath; }
            set { _projectPath = value; }
        }

        public string DocLanguage
        {
            get { return _docLanguage; }
            set { _docLanguage = value; }
        }

        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        public string PleaseWait
        {
            get { return _pleaseWait; }
            set { _pleaseWait = value; }
        }

        public string SeeBuild
        {
            get { return _seeBuild; }
            set { _seeBuild = value; }
        }

        public string LastBuild
        {
            get { return _lastBuild; }
            set { _lastBuild = value; }
        }
    }
}