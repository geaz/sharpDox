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
            var steps = TestConfig.GetSteps();
               
            // Act
            var solution = steps.LoadStep.LoadSolution();
            var repository = steps.ParseStep.ParseSolution(solution, new List<string>());
            var type = repository.GetTypeByIdentifier("SharpDox.TestProject.Regression1");

            // Assert            
            Assert.AreEqual(type.Methods[0].ReturnType.Name, "void");
        }
    }
}
