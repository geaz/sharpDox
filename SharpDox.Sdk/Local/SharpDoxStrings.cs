namespace SharpDox.Sdk.Local
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
    public class SharpDoxStrings : ILocalStrings
    {
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
    }
}