using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression8
    {
        [TestMethod]
        public void NoneAccessibilityShouldGetCorrectly()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var type = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("SharpDox.TestProject.Regression8");

            // Assert            
            Assert.IsTrue(type.Accessibility.Equals("internal"));
            Assert.IsTrue(type.Methods[0].Accessibility.Equals("private"));
            Assert.IsTrue(type.Fields[0].Accessibility.Equals("private"));
            Assert.IsTrue(type.Properties[0].Accessibility.Equals("private"));
        }
    }
}
