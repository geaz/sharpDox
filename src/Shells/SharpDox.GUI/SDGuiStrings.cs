using SharpDox.Sdk.Local;

namespace SharpDox.GUI
{
    public class SDGuiStrings : ILocalStrings
    {
        public string Text { get; set; } = "Text";
        public string LinkText { get; set; } = "Link Text";
        public string Link { get; set; } = "Link Url";
        public string LinkTitle { get; set; } = "Link Title";
        public string Mandatory { get; set; } = "required";
        public string Optional { get; set; } = "optional";
        public string Build { get; set; } = "Build";
        public string File { get; set; } = "File";
        public string New { get; set; } = "New";
        public string Load { get; set; } = "Load";
        public string Save { get; set; } = "Save";
        public string SaveAs { get; set; } = "Save as";
        public string RecentProjects { get; set; } = "Recent Projects";
        public string Help { get; set; } = "Help";
        public string Homepage { get; set; } = "Homepage";
        public string Github { get; set; } = "Github";
        public string Documentation { get; set; } = "Documentation";
        public string PleaseWait { get; set; } = "Please wait...";
        public string SeeBuild { get; set; } = "See build page for details!";
        public string LastBuild { get; set; } = "LAST BUILD";
        public string RefreshTree { get; set; } = "Refresh tree";
        public string IncludeAll { get; set; } = "Include all";
        public string HidePrivate { get; set; } = "Hide all private members";
        public string HideProtected { get; set; } = "Hide all protected members";
        public string HideInternal { get; set; } = "Hide all internal members";
        public string No { get; set; } = "no";
        public string Excluded { get; set; } = "excluded";
        public string Elements { get; set; } = "element(s)";
        public string Abort { get; set; } = "Abort";
        public string NoneSelected { get; set; } = "none selected";
        public string VisibilitySettings { get; set; } = "Visibility Settings";

        public string DisplayName => "SharpDoxGui";
    }
}