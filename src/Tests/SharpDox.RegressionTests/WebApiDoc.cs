using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class WebApiDoc
    {
        [TestMethod]
        public void DocShouldContainWebApiDoc()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.WebApiDoc");
            var method = type.Methods[0];

            // Assert            
            Assert.IsTrue(method.Documentations.GetElementOrDefault("en").Returns.ContainsKey("200"));
        }
    }
}
