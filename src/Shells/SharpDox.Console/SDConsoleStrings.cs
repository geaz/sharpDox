using SharpDox.Sdk.Local;

namespace SharpDox.Console
{
    public class SDConsoleStrings : ILocalStrings
    {
        public string ConfigMissing { get; set; } = "Please provide a config file.";
        public string Path { get; set; } = "PATH";
        public string PressToEnd { get; set; } = "Press enter to exit sharpDox ...";

        public string DisplayName => "SharpDoxConsole";
    }
}