using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Sdk;

namespace SharpDox.Config.Tests
{
    [TestClass]
    public class SDPathTests
    {
        [TestMethod]
        public void CorrectlyUpdatesThePaths()
        {
            var path = new SDPath();

            path.UpdatePath("c:\\source\\someproject\\output", "c:\\source\\someproject");

            Assert.AreEqual("c:\\source\\someproject\\output", path.FullPath);
            Assert.AreEqual("output", path.RelativePath);

            path.UpdatePath("c:\\completelydifferent\\output", "c:\\source\\someproject");

            Assert.AreEqual("c:\\completelydifferent\\output", path.FullPath);
            Assert.AreEqual("..\\..\\completelydifferent\\output", path.RelativePath);

            var resolvedPath = path.ResolvePath("c:\\source\\someproject\\superdeepdirectory");

            Assert.AreEqual("c:\\source\\completelydifferent\\output", resolvedPath);
        }

        [TestMethod]
        public void CanResolveRelativePaths()
        {
            var path = new SDPath();

            path.UpdatePath("c:\\source\\someproject\\output", "c:\\source\\someproject");

            var resolvedPath = path.ResolvePath("c:\\buildagent\\CI_WS\\1234");

            Assert.AreEqual("c:\\buildagent\\CI_WS\\1234\\output", resolvedPath);
        }

        [TestMethod]
        public void FallsBackToFullPathsIfRelativePathsAreMissing()
        {
            var path = new SDPath("c:\\source\\someproject\\output", null);

            var resolvedPath = path.ResolvePath("c:\\buildagent\\CI_WS\\1234");

            Assert.AreEqual("c:\\source\\someproject\\output", resolvedPath);
        }
    }
}
