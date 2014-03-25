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
        private string _configTitle = "General Settings";
        private string _projectName = "Project Name";
        private string _versionNumber = "Version Number";
        private string _author = "Author";
        private string _logoPath = "Logo Path";
        private string _inputPath = "Input Path";
        private string _excludedIdentifiers = "Excluded Identifiers";
        private string _exporters = "Exporters";
        private string _outputPath = "Output Path";
        private string _docLanguage = "Doc Language";

        private string _startSd = "Starting sharpDox ...";
        private string _noShells = "No shell registered. At least one shell is necessary to run sharpDox.";
        private string _moreShells = "More than one shell registered. Which one should be started?";
        private string _newConfig = "New Configuration";
        private string _never = "NEVER";

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
        public string DisplayName
        {
            get { return "SharpDox"; }
        }

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
        public string NewConfig
        {
            get { return _newConfig; }
            set { _newConfig = value; }
        }

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
        public string Never
        {
            get { return _never; }
            set { _never = value; }
        }

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
        public string StartSD
        {
            get { return _startSd; }
            set { _startSd = value; }
        }

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
        public string NoShells
        {
            get { return _noShells; }
            set { _noShells = value; }
        }

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
        public string MoreShells
        {
            get { return _moreShells; }
            set { _moreShells = value; }
        }

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
        public string ConfigTitle
        {
            get { return _configTitle; }
            set { _configTitle = value; }
        }

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
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

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
        public string VersionNumber
        {
            get { return _versionNumber; }
            set { _versionNumber = value; }
        }

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
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

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
        public string LogoPath
        {
            get { return _logoPath; }
            set { _logoPath = value; }
        }

        /// <default>
        ///     <summary>
        ///     Localized Text: "Input Path"
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lokalisierter Text: "Input Path"
        ///     </summary>
        /// </de>
        public string InputPath
        {
            get { return _inputPath; }
            set { _inputPath = value; }
        }

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
        public string ExcludedIdentifiers
        {
            get { return _excludedIdentifiers; }
            set { _excludedIdentifiers = value; }
        }

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
        public string Exporters
        {
            get { return _exporters; }
            set { _exporters = value; }
        }

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
        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

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
        public string DocLanguage
        {
            get { return _docLanguage; }
            set { _docLanguage = value; }
        }
    }
}