using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression4
    {
        [TestMethod]
        public void DocumentationShouldContainExceptions()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression4");

            // Assert            
            Assert.AreEqual(2, type.Methods[0].Documentations.GetElementOrDefault("default").Exceptions.Count);
        }
    }
}
