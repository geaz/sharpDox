using SharpDox.Sdk.Local;

namespace SharpDox.Build.Roslyn
{
    public class ParserStrings : ILocalStrings
    {
        public string CompilingAndParsing { get; set; } = "Compiling and Parsing: {0}";
        public string CleanUp { get; set; } = "Cleaning repository: {0}";
        public string ParsingMethod { get; set; } = "Parsing Methods of Target: {0}";

        public string DisplayName => "RoslynBuilder";
    }
}