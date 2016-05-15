using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Model.Documentation.Token;
using SharpDox.RegressionTests;

namespace SharpDox.Build.Tests
{
    [TestClass]
    public class CrefTests
    {
        [TestMethod]
        public void ShouldCrefTypeCorrectly()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdMethod = sdProject.Solutions.First().Value.Repositories.First().GetMethodByIdentifier("SharpDox.TestProject.SeeAlsoDocType<TK>.TestMethod2");
            var seeTokens = sdMethod.Documentations.First().Value.Summary.Where(t => t.Role == SDTokenRole.See);

            // Assert       
            foreach (var seeToken in seeTokens)
            {
                Assert.AreNotEqual(string.Empty, ((SDSeeToken)seeToken).Identifier);
            }
        }
    }
}
