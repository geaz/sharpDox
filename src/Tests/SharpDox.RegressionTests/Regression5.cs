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
    public class Regression5
    {
        [TestMethod]
        public void RepositoryShouldContainNestedTypes()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression5");

            // Assert            
            Assert.AreEqual(1, type.NestedTypes.Count);
        }
    }
}
