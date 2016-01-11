using System.Linq;
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
            var type = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("<global namespace>.Regression6");

            // Assert            
            Assert.IsNotNull(type);
        }
    }
}
