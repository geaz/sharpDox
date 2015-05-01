using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression6
    {
        [TestMethod]
        public void ClassWithoutNamespaceGetsParsedSuccessfully()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier(".Regression6");

            // Assert            
            Assert.IsNotNull(type);
        }
    }
}
