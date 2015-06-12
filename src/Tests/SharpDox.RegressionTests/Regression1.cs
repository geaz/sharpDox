using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression1
    {
        [TestMethod]
        public void VoidShouldNotBeUpperCase()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.Solutions.First().Value.Repositories.First().Value.GetTypeByIdentifier("SharpDox.TestProject.Regression1");

            // Assert            
            Assert.AreEqual("void", type.Methods[0].ReturnType.Name);
        }
    }
}
