using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using SharpDox.Model.CallTree;

namespace SharpDox.UML.Tests
{
    [TestClass]
    public class DiagramExtensionTests
    {
        [TestMethod]
        public void ShouldReturnTrueForSequenceDiagramIsEmpty()
        {
            //Arrange
            var sdMethod = new SDMethod("SharpDox.UML.Tests.DiagramExtensionsTests.Method1", "Method1");

            //Act
            var isEmpty = sdMethod.IsSequenceDiagramEmpty();

            //Assert
            Assert.AreEqual(true, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnTrueForSequenceDiagramIsEmptyBecauseOnlyCallsToStrangers()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            var sdMethod = new SDMethod("SharpDox.UML.Tests.DiagramExtensionsTests.Method1", "Method1");

            var sdTypeStranger = new SDType("System.String", "String", new SDNamespace("System"));
            var sdMethodStranger = new SDMethod("System.String.Method1", "Method1");

            var sdNode = new SDTargetNode();
            sdNode.CalledType = sdTypeStranger;
            sdNode.CalledMethod = sdMethodStranger;
            sdNode.CallerMethod = sdMethod;
            sdNode.CallerType = sdType;

            sdMethod.Calls.Add(sdNode);

            //Act
            var isEmpty = sdMethod.IsSequenceDiagramEmpty();

            //Assert
            Assert.AreEqual(true, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnFalseForSequenceDiagramIsEmpty()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            sdType.IsProjectStranger = false;
            var sdMethod = new SDMethod("SharpDox.UML.Tests.DiagramExtensionsTests.Method1", "Method1");

            var sdNode = new SDTargetNode();
            sdNode.CalledMethod = new SDMethod("SharpDox.UML.Tests.DiagramExtensionsTests.Method2", "Method2");
            sdNode.CallerMethod = sdMethod;
            sdNode.CallerType = sdType;
            sdNode.CalledType = sdType;

            //Add this node twice, because the check will skipp call number one
            //because this one is usually the first call to the method itself
            sdMethod.Calls.Add(sdNode);
            sdMethod.Calls.Add(sdNode);

            //Act
            var isEmpty = sdMethod.IsSequenceDiagramEmpty();

            //Assert
            Assert.AreEqual(false, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnTrueForClassDiagramIsEmpty()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));

            //Act
            var isEmpty = sdType.IsClassDiagramEmpty();

            //Assert
            Assert.AreEqual(true, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnFalseForClassDiagramIsEmptyBecauseFieldPresent()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            sdType.Fields.Add(new SDField("SharpDox.UML.Tests.DiagramExtensionsTests.Field1"));

            //Act
            var isEmpty = sdType.IsClassDiagramEmpty();

            //Assert
            Assert.AreEqual(false, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnFalseForClassDiagramIsEmptyBecausePropertyPresent()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            sdType.Properties.Add(new SDProperty("SharpDox.UML.Tests.DiagramExtensionsTests.Property1"));

            //Act
            var isEmpty = sdType.IsClassDiagramEmpty();

            //Assert
            Assert.AreEqual(false, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnFalseForClassDiagramIsEmptyBecauseMethodPresent()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            sdType.Methods.Add(new SDMethod("SharpDox.UML.Tests.DiagramExtensionsTests.Method1", "Method1"));

            //Act
            var isEmpty = sdType.IsClassDiagramEmpty();

            //Assert
            Assert.AreEqual(false, isEmpty);
        }

        [TestMethod]
        public void ShouldReturnFalseForClassDiagramIsEmptyBecauseEventPresent()
        {
            //Arrange
            var sdType = new SDType("SharpDox.UML.Tests.DiagramExtensionsTests", "DiagramExtensionTests", new SDNamespace("SharpDox.UML.Tests"));
            sdType.Events.Add(new SDEvent("SharpDox.UML.Tests.DiagramExtensionsTests.Event1"));

            //Act
            var isEmpty = sdType.IsClassDiagramEmpty();

            //Assert
            Assert.AreEqual(false, isEmpty);
        }
    }
}
