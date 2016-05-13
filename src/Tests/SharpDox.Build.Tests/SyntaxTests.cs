using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.RegressionTests;

namespace SharpDox.Build.Tests
{
    [TestClass]
    public class SyntaxTests
    {
        [TestMethod]
        public void ShouldContainTypeParametersAndReturns()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdMethod = sdProject.Solutions.First().Value.Repositories.First().GetMethodByIdentifier("SharpDox.TestProject.TypeWithTypeArguments<T>.TypeParameters(System.Collections.Generic.List<System.String>)");
            
            // Assert            
            Assert.AreEqual(sdMethod.Syntax, "public List<string> TypeParameters(List<string> test)");
        }
    }
}
