using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class InheritDoc
    {
        [TestMethod]
        public void DocShouldInherit()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.Solutions.First().Value.Repositories.First().Value.GetTypeByIdentifier("SharpDox.TestProject.InheritDoc");
            var method = type.Methods[0];

            // Assert            
            //Assert.IsNotNull(type.Documentations.GetElementOrDefault("en").Summary);
            Assert.IsNotNull(method.Documentations.GetElementOrDefault("en").Summary);
        }
    }
}
