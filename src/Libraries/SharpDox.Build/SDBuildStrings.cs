using SharpDox.Sdk.Local;

namespace SharpDox.Build
{
    public class SDBuildStrings : ILocalStrings
    {
        public string StartingBuild { get; set; } = "Starting build";
        public string BuildSuccess { get; set; } = "Build ended successfully";
        public string StartingStep { get; set; } = "Starting step: \"{0}\"";
        public string StepCheckConfig { get; set; } = "Check config";
        public string StepParseProject { get; set; } = "Parsing project file";
        public string StepParseCode { get; set; } = "Parsing code";
        public string StepExport { get; set; } = "Export documentation";
        public string StepEnd { get; set; } = "Finalizing build";
        public string ParsingProject { get; set; } = "Parsing project";
        public string ParseTokens { get; set; } = "Parsing tokens";
        public string ParsingDescriptions { get; set; } = "Parsing descriptions";
        public string ParsingNav { get; set; } = "Parsing navigation file";
        public string NoDocLanguage { get; set; } = "Please select a default language!";
        public string NoProjectNameGiven { get; set; } = "No project name given. Please enter a project name and try again.";
        public string NoProjectGiven { get; set; } = "No Project given. Please select a project and try again.";
        public string ProjectNotFound { get; set; } = "No project found at the given path.";
        public string NoOutputPathGiven { get; set; } = "No output path given. Please select a path and try again.";
        public string OutputPathNotFound { get; set; } = "Output path not found.";
        public string BuildStopped { get; set; } = "Build stopped!";
        public string CouldNotEndBuild { get; set; } = "Could not build documentation.";
        public string RequirementError { get; set; } = "There was an error with the requirements of one or more exporters. Please check the output window.";
        public string RunningExporter { get; set; } = "Starting exporter: \"{0}\"";

        public string DisplayName => "SharpDoxBuild";
    }
}