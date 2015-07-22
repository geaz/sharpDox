using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression7
    {
        [TestMethod]
        public void SyntaxShouldContainAllTypeParameters()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("SharpDox.TestProject.Regression7");

            // Assert            
            Assert.IsTrue(type.Methods[0].Syntax.Contains("List<String>"));
            Assert.IsTrue(type.Methods[0].Syntax.Contains("List<Int32>"));
        }
    }
}
