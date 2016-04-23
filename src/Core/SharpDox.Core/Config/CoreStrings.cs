using SharpDox.Sdk.Local;

namespace SharpDox.Core.Config
{
    /// <default>
    ///     <summary>
    ///     All strings used by the core application.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Alle Zeichenketten die von der Haupt-Anwendung genutzt werden.
    ///     </summary>
    /// </de>
    public class CoreStrings : ILocalStrings
    {
        /// <default>
        ///     <summary>
        ///     Localized Text: "New Configuration"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "New Configuration"
        ///     </summary>
        /// </de>
        public string NewConfig { get; set; } = "New Configuration";

        /// <default>
        ///     <summary>
        ///     Localized Text: "NEVER"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "NEVER"
        ///     </summary>
        /// </de>
        public string Never { get; set; } = "NEVER";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Starting sharpDox ..."
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Starting sharpDox ..."
        ///     </summary>
        /// </de>
        public string StartSD { get; set; } = "Starting sharpDox ...";

        /// <default>
        ///     <summary>
        ///     Localized Text: "No shell registered. At least one shell is necessary to run sharpDox."
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "No shell registered. At least one shell is necessary to run sharpDox."
        ///     </summary>
        /// </de>
        public string NoShells { get; set; } = "No shell registered. At least one shell is necessary to run sharpDox.";

        /// <default>
        ///     <summary>
        ///     Localized Text: "More than one shell registered. Which one should be started?"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "More than one shell registered. Which one should be started?"
        ///     </summary>
        /// </de>
        public string MoreShells { get; set; } = "More than one shell registered. Which one should be started?";

        /// <default>
        ///     <summary>
        ///     Localized Text: "General Settings"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "General Settings"
        ///     </summary>
        /// </de>
        public string ConfigTitle { get; set; } = "General Settings";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Project Name"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Project Name"
        ///     </summary>
        /// </de>
        public string ProjectName { get; set; } = "Project Name";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Version Number"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Version Number"
        ///     </summary>
        /// </de>
        public string VersionNumber { get; set; } = "Version Number";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Author"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Author"
        ///     </summary>
        /// </de>
        public string Author { get; set; } = "Author";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Author Homepage"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Author Homepage"
        ///     </summary>
        /// </de>
        public string AuthorUrl { get; set; } = "Author Homepage";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Project Homepage"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Project Homepage"
        ///     </summary>
        /// </de>
        public string ProjectUrl { get; set; } = "Project Homepage";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Logo Path"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Logo Path"
        ///     </summary>
        /// </de>
        public string LogoPath { get; set; } = "Logo Path";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Input File"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Input File"
        ///     </summary>
        /// </de>
        public string InputFile { get; set; } = "Input File";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Excluded Identifiers"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Excluded Identifiers"
        ///     </summary>
        /// </de>
        public string ExcludedIdentifiers { get; set; } = "Excluded Identifiers";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Exporters"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Exporters"
        ///     </summary>
        /// </de>
        public string Exporters { get; set; } = "Exporters";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Output Path"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Output Path"
        ///     </summary>
        /// </de>
        public string OutputPath { get; set; } = "Output Path";

        /// <default>
        ///     <summary>
        ///     Localized Text: "Doc Language"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Doc Language"
        ///     </summary>
        /// </de>
        public string DocLanguage { get; set; } = "Doc Language";

        /// <default>
        ///     <summary>
        ///     Gets the name of the language file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namen der für die Sprachdatei genutzt wird.
        ///     </summary>
        /// </de>
        public string DisplayName => "SharpDox";
    }
}