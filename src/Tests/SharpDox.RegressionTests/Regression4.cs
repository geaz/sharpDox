using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;
using System.Reflection;
using System.IO;
using SharpDox.Build.Context;
using SharpDox.Sdk.Exporter;
using System.Collections.Generic;
using SharpDox.Build;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression4
    {
        [TestMethod]
        public void DocumentationShouldContainExceptions()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression4");

            // Assert            
            Assert.AreEqual(2, type.Methods[0].Documentations.GetElementOrDefault("default").Exceptions.Count);
        }
    }
}
