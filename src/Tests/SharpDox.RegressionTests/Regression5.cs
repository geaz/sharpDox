using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression5
    {
        [TestMethod]
        public void RepositoryShouldContainNestedTypes()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression5");

            // Assert            
            Assert.AreEqual(1, type.NestedTypes.Count);
        }
    }
}
