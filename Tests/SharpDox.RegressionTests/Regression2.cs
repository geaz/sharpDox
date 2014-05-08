using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SharpDox.UML;

namespace SharpDox.RegressionTests
{
    [TestClass]
    public class Regression2
    {
        [TestMethod]
        public void SvgShouldRenderCorrectlyWithArrayReturnType()
        {
            // Arrange
            var steps = TestConfig.GetSteps();
               
            // Act
            var solution = steps.LoadStep.LoadSolution();
            var repository = steps.ParseStep.ParseSolution(solution, new List<string>());
            var type = repository.GetTypeByIdentifier("SharpDox.TestProject.Regression2");            
            var svgDiagram = type.Methods[0].GetSequenceDiagram(repository.GetAllTypes()).ToSvg();

            // Assert            
            Assert.IsFalse(svgDiagram.Contains("]]]>"));
        }
    }
}
