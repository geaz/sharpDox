using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.RegressionTests;

namespace SharpDox.Build.Tests
{
    [TestClass]
    public class IdentifierTests
    {
        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForNamespace()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdNamespace = sdProject.Solutions.First().Value.Repositories.First().GetNamespaceByIdentifier("SharpDox.TestProject");

            // Assert            
            Assert.IsNotNull(sdNamespace);
        }

        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForType()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdType = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("SharpDox.TestProject.Regression1");

            // Assert            
            Assert.IsNotNull(sdType);
        }

        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForTypeWithTypeArguments()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdType = sdProject.Solutions.First().Value.Repositories.First().GetTypeByIdentifier("SharpDox.TestProject.TypeWithTypeArguments<T>");

            // Assert            
            Assert.IsNotNull(sdType);
        }

        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForMethod()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdMethod = sdProject.Solutions.First().Value.Repositories.First().GetMethodByIdentifier("SharpDox.TestProject.TypeWithTypeArguments<T>.DoIt");

            // Assert            
            Assert.IsNotNull(sdMethod);
        }

        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForMethodWithParameter()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdMethod = sdProject.Solutions.First().Value.Repositories.First().GetMethodByIdentifier("SharpDox.TestProject.TypeWithTypeArguments<T>.DoMore(System.Int32)");

            // Assert            
            Assert.IsNotNull(sdMethod);
        }

        [TestMethod]
        public void ShouldCreateIdentifierCorrectlyForMethodWithTypeParameters()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();

            // Act
            var sdMethod = sdProject.Solutions.First().Value.Repositories.First().GetMethodByIdentifier("SharpDox.TestProject.TypeWithTypeArguments<T>.DoEvenMore<TV>(T, TV)");

            // Assert            
            Assert.IsNotNull(sdMethod);
        }
    }
}
