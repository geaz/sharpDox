using SharpDox.Sdk.Local;

namespace SharpDox.Build
{
    public class ParserStrings : ILocalStrings
    {        
        private string _parsingNamespace = "Parsing namespace";
        private string _parsingClass = "Parsing class";
        private string _parsingMethod = "Parsing method";
        private string _parsingUseings = "Parsing useings";
        private string _readingProject = "Reading project: {0}";  

        public string DisplayName { get { return "SharpDoxParse"; } }
                
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

        public string ReadingProject
        {
            get { return _readingProject; }
            set { _readingProject = value; }
        }
    }
}