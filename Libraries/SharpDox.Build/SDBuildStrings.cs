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
        private string _loadingSolution = "Loading solution. Duration depends on solution size ...";
        private string _projectsLoaded = "{0} lines of code ({1:f1} MB) in {2} files in {3} projects loaded ...";
        private string _parsingSDProject = "Parsing sharpDox project ...";
        private string _parsingSolution = "Parsing solution. Duration depends on solution size ...";
        private string _parsingNamespace = "Parsing namespace";
        private string _parsingClass = "Parsing class";
        private string _parsingMethod = "Parsing method";
        private string _parsingUseings = "Parsing useings";
        private string _createClassDiagrams = "Creating Classdiagrams";
        private string _createSequenceDiagrams = "Creating Sequencediagrams";
        private string _startExporter = "Starting exporter";
        private string _startingParse = "Starting parse";
        private string _parseSuccess = "Parse ended successfully.";
        private string _parseStopped = "Parse stopped!";
        private string _couldNotEndParse = "Could not parse solution.";
        private string _readingProject = "Reading project";
        
        
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

        public string LoadingSolution
        {
            get { return _loadingSolution; }
            set { _loadingSolution = value; }
        }

        public string ProjectsLoaded
        {
            get { return _projectsLoaded; }
            set { _projectsLoaded = value; }
        }

        public string ParsingSDProject
        {
            get { return _parsingSDProject; }
            set { _parsingSDProject = value; }
        }

        public string ParsingSolution
        {
            get { return _parsingSolution; }
            set { _parsingSolution = value; }
        }

        public string ParsingNamespace
        {
            get { return _parsingNamespace; }
            set { _parsingNamespace = value; }
        }

        public string ParsingClass
        {
            get { return _parsingClass; }
            set { _parsingClass = value; }
        }

        public string ParsingMethod
        {
            get { return _parsingMethod; }
            set { _parsingMethod = value; }
        }

        public string ParsingUseings
        {
            get { return _parsingUseings; }
            set { _parsingUseings = value; }
        }

        public string CreateClassDiagrams
        {
            get { return _createClassDiagrams; }
            set { _createClassDiagrams = value; }
        }

        public string CreateSequenceDiagrams
        {
            get { return _createSequenceDiagrams; }
            set { _createSequenceDiagrams = value; }
        }

        public string StartExporter
        {
            get { return _startExporter; }
            set { _startExporter = value; }
        }

        public string StartingParse
        {
            get { return _startingParse; }
            set { _startingParse = value; }
        }

        public string ParseSuccess
        {
            get { return _parseSuccess; }
            set { _parseSuccess = value; }
        }

        public string ParseStopped
        {
            get { return _parseStopped; }
            set { _parseStopped = value; }
        }

        public string CouldNotEndParse
        {
            get { return _couldNotEndParse; }
            set { _couldNotEndParse = value; }
        }

        public string ReadingProject
        {
            get { return _readingProject; }
            set { _readingProject = value; }
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