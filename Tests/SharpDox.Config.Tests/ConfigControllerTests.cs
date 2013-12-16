using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Local;

namespace SharpDox.Config.Tests
{
    [TestClass]
    public class ConfigControllerTests
    {
        [TestMethod]
        public void ShouldExecutePropertyChanged()
        {
            // Arrange
            var testConfig = new TestConfig();
            var wasRaised = false;

            // System under Test
            var configController = new ConfigController(new IConfigSection[] { testConfig }, new SharpDoxConfig(new SharpDoxStrings()));
            testConfig.PropertyChanged += (a, s) => { wasRaised = true; };

            // Act
            testConfig.TestVar1 = "6fa5a650-25f3-4c86-a515-37e3163a54e9";

            // Assert
            Assert.IsTrue(wasRaised);
        }
    }
}
