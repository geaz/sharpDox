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
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression3");

            // Assert            
            Assert.IsTrue(type.Methods[0].Syntax.Contains("static"));
        }
    }
}
