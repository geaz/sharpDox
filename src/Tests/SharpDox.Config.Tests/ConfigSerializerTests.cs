using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Core.Config;
using SharpDox.Sdk.Config;

namespace SharpDox.Config.Tests
{
    [TestClass]
    public class ConfigSerializerTests
    {
        private const string CONFIG_XML = "<SDConfig>" +
                                            "<section guid=\"6bc727ba-1391-4e9f-95b1-6c800eec9799\">" +
                                            "<item key=\"TestVar1\" value=\"6fa5a650-25f3-4c86-a515-37e3163a54e9\" />" +
                                            "</section>" +
                                            "</SDConfig>";

        [TestMethod]
        public void ShouldReturnCorrectXml()
        {
            // Arrange
            var testConfig = new TestConfig { TestVar1 = "6fa5a650-25f3-4c86-a515-37e3163a54e9" };

            // System under Test
            var configSerializer = new ConfigSerializer();

            // Act
            var xmlDocument = configSerializer.GetSerializedConfigs(new IConfigSection[] { testConfig });

            // Assert
            Assert.AreEqual(CONFIG_XML.Replace(" ", ""), xmlDocument.ToString().Replace("\r\n", "").Replace(" ", ""));
        }

        [TestMethod]
        public void ShouldReturnCorrectConfig()
        {
            // Arrange
            var testConfig = new TestConfig();

            // System under Test
            var configSerializer = new ConfigSerializer();

            // Act
            configSerializer.SetDeserializedConfigs(XDocument.Parse(CONFIG_XML), new IConfigSection[] { testConfig });

            // Assert
            Assert.AreEqual("6fa5a650-25f3-4c86-a515-37e3163a54e9", testConfig.TestVar1);
        }
    }
}
