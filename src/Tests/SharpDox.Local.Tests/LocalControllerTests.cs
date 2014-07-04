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
    }
}
