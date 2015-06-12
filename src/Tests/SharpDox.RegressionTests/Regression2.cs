using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
            var type = sdProject.Solutions.First().Value.Repositories.First().Value.GetTypeByIdentifier("SharpDox.TestProject.Regression2");
            var svgDiagram = type.Methods[0].GetSequenceDiagram(sdProject.Solutions.First().Value.Repositories.First().Value).ToSvg();

            // Assert            
            Assert.IsFalse(svgDiagram.Template.Contains("]]]>"));
        }
    }
}
