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

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression1
    {
        [TestMethod]
        public void VoidShouldNotBeUpperCase()
        {
            // Arrange
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression1");

            // Assert            
            Assert.AreEqual("void", type.Methods[0].ReturnType.Name);
        }
    }
}
