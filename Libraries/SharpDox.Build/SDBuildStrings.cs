using SharpDox.Sdk.Local;

namespace SharpDox.Build
{
    public class SDBuildStrings : ILocalStrings
    {
        private string _startingBuild = "Starting build";
        private string _buildSuccess = "Build ended successfully";
        private string _startingStep = "Starting step: \"{0}\"";
        private string _stepCheckConfig = "Check config";
        private string _stepParseProject = "Parsing project file";
        private string _stepParseCode = "Parsing code";
        private string _stepExport = "Export documentation";
        private string _stepEnd = "Finalizing build";

        private string _parsingProject = "Parsing project";
        private string _parsingDescriptions = "Parsing descriptions";
        private string _parsingNav = "Parsing navigation file";

        private string _noDocLanguage = "Please select a default language!";
        private string _noProjectNameGiven = "No project name given. Please enter a project name and try again.";
        private string _noProjectGiven = "No Project given. Please select a project and try again.";
        private string _projectNotFound = "No project found at the given path.";
        private string _noOutputPathGiven = "No output path given. Please select a path and try again.";
        private string _outputPathNotFound = "Output path not found.";
        private string _buildStopped = "Build stopped!";
        private string _couldNotEndBuild = "Could not build documentation.";
        private string _requirementError = "There was an error with the requirements of one or more exporters. Please check the output window.";

        public string DisplayName { get { return "SharpDoxBuild"; } }

        public string StartingBuild
        {
            get { return _startingBuild; }
            set { _startingBuild = value; }
        }

        public string BuildSuccess
        {
            get { return _buildSuccess; }
            set { _buildSuccess = value; }
        }

        public string StartingStep
        {
            get { return _startingStep; }
            set { _startingStep = value; }
        }

        public string StepCheckConfig
        {
            get { return _stepCheckConfig; }
            set { _stepCheckConfig = value; }
        }

        public string StepParseProject
        {
            get { return _stepParseProject; }
            set { _stepParseProject = value; }
        }

        public string StepParseCode
        {
            get { return _stepParseCode; }
            set { _stepParseCode = value; }
        }

        public string StepExport
        {
            get { return _stepExport; }
            set { _stepExport = value; }
        }

        public string StepEnd
        {
            get { return _stepEnd; }
            set { _stepEnd = value; }
        }

        public string ParsingProject
        {
            get { return _parsingProject; }
            set { _parsingProject = value; }
        }

        public string ParsingDescriptions
        {
            get { return _parsingDescriptions; }
            set { _parsingDescriptions = value; }
        }

        public string ParsingNav
        {
            get { return _parsingNav; }
            set { _parsingNav = value; }
        }

        public string NoDocLanguage
        {
            get { return _noDocLanguage; }
            set { _noDocLanguage = value; }
        }

        public string NoProjectNameGiven
        {
            get { return _noProjectNameGiven; }
            set { _noProjectNameGiven = value; }
        }

        public string NoProjectGiven
        {
            get { return _noProjectGiven; }
            set { _noProjectGiven = value; }
        }

        public string ProjectNotFound
        {
            get { return _projectNotFound; }
            set { _projectNotFound = value; }
        }

        public string NoOutputPathGiven
        {
            get { return _noOutputPathGiven; }
            set { _noOutputPathGiven = value; }
        }

        public string OutputPathNotFound
        {
            get { return _outputPathNotFound; }
            set { _outputPathNotFound = value; }
        }        

        public string BuildStopped
        {
            get { return _buildStopped; }
            set { _buildStopped = value; }
        }

        public string CouldNotEndBuild
        {
            get { return _couldNotEndBuild; }
            set { _couldNotEndBuild = value; }
        }

        public string RequirementError
        {
            get { return _requirementError; }
            set { _requirementError = value; }
        }
    }
}