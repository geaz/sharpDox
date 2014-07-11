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
            var sdProject = TestConfig.ParseProject();
               
            // Act
            var type = sdProject.GetTypeByIdentifier("SharpDox.TestProject.Regression2");
            var svgDiagram = type.Methods[0].GetSequenceDiagram(sdProject).ToSvg();

            // Assert            
            Assert.IsFalse(svgDiagram.Template.Contains("]]]>"));
        }
    }
}
