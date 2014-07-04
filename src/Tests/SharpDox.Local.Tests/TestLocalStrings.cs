using SharpDox.Sdk.Local;

namespace SharpDox.Local.Tests
{
    internal class TestLocalStrings : ILocalStrings
    {
        private string _testString1 = "TestString1";
        private string _testString2 = "TestString2";

        public string DisplayName
        {
            get { return "TestStrings"; }
        }

        public string TestString2
        {
            get { return _testString2; }
            set { _testString2 = value; }
        }

        public string TestString1
        {
            get { return _testString1; }
            set { _testString1 = value; }
        }
    }
}
