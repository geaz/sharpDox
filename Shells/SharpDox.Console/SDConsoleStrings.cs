using SharpDox.Sdk.Local;

namespace SharpDox.Console
{
    public class SDConsoleStrings : ILocalStrings
    {
        private string _configMissing = "Please provide a config file.";
        private string _path = "PATH";
        private string _pressToEnd = "Press enter to exit sharpDox ...";

        public string DisplayName { get { return "SharpDoxConsole"; } }

        public string ConfigMissing
        {
            get { return _configMissing; }
            set { _configMissing = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string PressToEnd
        {
            get { return _pressToEnd; }
            set { _pressToEnd = value; }
        }
    }
}