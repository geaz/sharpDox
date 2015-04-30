using SharpDox.Sdk.Local;

namespace SharpDox.Console
{
    public class SDConsoleStrings : ILocalStrings
    {
        private string _configMissing = "Please provide a config file.";
        private string _path = "PATH";

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

    }
}
