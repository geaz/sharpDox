using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Model;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Tests
{
    [TestClass]
    public class SDTargetFxParserTests
    {
        [TestMethod]
        public void RecognizesNet40()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.NET40.csproj", KnownTargetFxs.Net40);
        }
        
        // Workaround since MsTest doesn't have "TestCase" where we can put in parameters
        private void TestProjectFx(string relativeProjectFile, SDTargetFx expectedFx)
        {
            var assemblyDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            var fullPath = Path.Combine(assemblyDirectory, relativeProjectFile);

            var targetFxParser = new SDTargetFxParser();
            var actualFx = targetFxParser.GetTargetFx(fullPath);

            Assert.AreEqual(expectedFx.Identifier, actualFx.Identifier, string.Format("Failed when determining the target fx for '{0}'", relativeProjectFile));
        }
    }
}