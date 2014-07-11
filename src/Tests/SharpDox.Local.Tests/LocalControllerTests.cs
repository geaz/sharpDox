using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Sdk.Local;

namespace SharpDox.Local.Tests
{
    [TestClass]
    public class LocalControllerTests
    {
        [TestMethod]
        public void ShouldGetTestLocalStrings()
        {
            // Arrange   
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var strings = localController.GetLocalStrings<TestLocalStrings>();

            // Assert
            Assert.IsNotNull(strings);
            Assert.AreEqual("TestString1", strings.TestString1);
        }

        [TestMethod]
        public void ShouldGetLocalizedDEStrings()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var strings = localController.GetLocalStrings<TestLocalStrings>();

            // Assert
            Assert.AreEqual("TestString1DE", strings.TestString1);
        }

        [TestMethod]
        public void ShouldGetLocalizedDEStringByName()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var strings = localController.GetLocalString(typeof(TestLocalStrings), "TestString1");

            // Assert
            Assert.AreEqual("TestString1DE", strings);
        }

        [TestMethod]
        public void ShouldGetDELocalStrings()
        {
            // Arrange
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var localStrings = localController.GetLocalStringsOrDefault<TestLocalStrings>("de");

            // Assert
            Assert.AreEqual("TestString1DE", localStrings.TestString1);
        }

        [TestMethod]
        public void ShouldGetDefaultLocalStrings()
        {
            // Arrange
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var localStrings = localController.GetLocalStringsOrDefault<TestLocalStrings>("default");

            // Assert
            Assert.AreEqual("TestString1", localStrings.TestString1);
        }

        [TestMethod]
        public void ShouldGetDefaultLocalStringsBecauseLanguageNotFound()
        {
            // Arrange
            var localController = new LocalController(new ILocalStrings[] { new TestLocalStrings() });

            // Act
            var localStrings = localController.GetLocalStringsOrDefault<TestLocalStrings>("fr");

            // Assert
            Assert.AreEqual("TestString1", localStrings.TestString1);
        }
    }
}
