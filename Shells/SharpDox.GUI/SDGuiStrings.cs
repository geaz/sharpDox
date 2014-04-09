using SharpDox.Sdk.Local;

namespace SharpDox.GUI
{
    public class SDGuiStrings : ILocalStrings
    {
        private string _mandatory = "required";
        private string _optional = "optional";
        private string _no = "no";
        private string _noneSelected = "none selected";
        private string _excluded = "excluded";
        private string _elements = "element(s)";
        private string _abort = "Abort";
        private string _build = "Build";
        private string _new = "New";
        private string _load = "Load";
        private string _save = "Save";
        private string _saveAs = "Save as";
        private string _recentProjects = "Recent Projects";
        private string _pleaseWait = "Please wait...";
        private string _seeBuild = "See build page for details!";
        private string _lastBuild = "LAST BUILD";
        private string _refreshTree = "Refresh tree";
        private string _includeAll = "Include all";
        private string _hidePrivate = "Hide all private members";
        private string _hideProtected = "Hide all protected members";
        private string _hideInternal = "Hide all internal members";
        private string _visibilitySettings = "Visibility Settings";

        public string DisplayName { get { return "SharpDoxGui"; } }

        public string Mandatory
        {
            get { return _mandatory; }
            set { _mandatory = value; }
        }

        public string Optional
        {
            get { return _optional; }
            set { _optional = value; }
        }

        public string Build
        {
            get { return _build; }
            set { _build = value; }
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

        public string RefreshTree
        {
            get { return _refreshTree; }
            set { _refreshTree = value; }
        }

        public string IncludeAll
        {
            get { return _includeAll; }
            set { _includeAll = value; }
        }

        public string HidePrivate
        {
            get { return _hidePrivate; }
            set { _hidePrivate = value; }
        }

        public string HideProtected
        {
            get { return _hideProtected; }
            set { _hideProtected = value; }
        }

        public string HideInternal
        {
            get { return _hideInternal; }
            set { _hideInternal = value; }
        }

        public string No
        {
            get { return _no; }
            set { _no = value; }
        }

        public string Excluded
        {
            get { return _excluded; }
            set { _excluded = value; }
        }

        public string Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        public string Abort
        {
            get { return _abort; }
            set { _abort = value; }
        }

        public string NoneSelected
        {
            get { return _noneSelected; }
            set { _noneSelected = value; }
        }

        public string VisibilitySettings
        {
            get { return _visibilitySettings; }
            set { _visibilitySettings = value; }
        }
    }
}