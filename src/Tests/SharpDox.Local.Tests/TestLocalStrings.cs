using SharpDox.Sdk.Local;

namespace SharpDox.Local.Tests
{
    internal class TestLocalStrings : ILocalStrings
    {
        public string TestString1 { get; set; } = "TestString1";
        public string TestString2 { get; set; } = "TestString2";

        public string DisplayName => "TestStrings";
    }
}
