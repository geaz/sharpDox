using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression3
    {
        [TestMethod]
        public void SyntaxShouldContainStaticKeyword()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("SharpDox.TestProject.Regression3");

            // Assert            
            Assert.IsTrue(type.Methods[0].Syntax.Contains("static"));
        }
    }
}
