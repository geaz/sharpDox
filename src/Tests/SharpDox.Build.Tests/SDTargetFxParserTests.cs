using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Model;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Tests
{
    [TestClass]
    public class SDTargetFxParserTests
    {
        [TestMethod]
        public void RecognizesNet40()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.NET40.csproj", KnownTargetFxs.Net40);
        }

        [TestMethod]
        public void RecognizesNet45()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.NET45.csproj", KnownTargetFxs.Net45);
        }

        [TestMethod]
        public void RecognizesNet46()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.NET46.csproj", KnownTargetFxs.Net46);
        }

        [TestMethod]
        public void RecognizesSilverlight5()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.SL5.csproj", KnownTargetFxs.Silverlight5);
        }

        [TestMethod]
        public void RecognizesWindowsPhone80()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.WP80.csproj", KnownTargetFxs.WindowsPhone80);
        }

        [TestMethod]
        public void RecognizesWindowsPhone81Silverlight()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.WPSL81.csproj", KnownTargetFxs.WindowsPhone81Silverlight);
        }

        [TestMethod]
        public void RecognizesWindowsPhone81Runtime()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.WPRT81.csproj", KnownTargetFxs.WindowsPhone81Runtime);
        }

        [TestMethod]
        public void RecognizesWindows81()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.WIN81.csproj", KnownTargetFxs.Windows81);
        }

        [TestMethod]
        public void RecognizesWindows100()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.WIN100.csproj", KnownTargetFxs.Windows100);
        }

        [TestMethod]
        public void RecognizesXamarinAndroid()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.Xamarin.Android.csproj", KnownTargetFxs.XamarinAndroid);
        }

        [TestMethod]
        public void RecognizesXamariniOS()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.Xamarin.iOS.csproj", KnownTargetFxs.XamariniOS);
        }

        [TestMethod]
        public void RecognizesPcl()
        {
            TestProjectFx(@"TestData\TargetFxFiles\Catel.Core.PCL.csproj", KnownTargetFxs.Pcl);
        }

        // Workaround since MsTest doesn't have "TestCase" where we can put in parameters
        private void TestProjectFx(string relativeProjectFile, SDTargetFx expectedFx)
        {
            var assemblyDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            var fullPath = Path.Combine(assemblyDirectory, relativeProjectFile);

            var targetFxParser = new SDTargetFxParser();
            var actualFx = targetFxParser.GetTargetFx(fullPath);

            Assert.AreEqual(expectedFx.Identifier, actualFx.Identifier, string.Format("Failed when determining the target fx for '{0}'", relativeProjectFile));
        }
    }
}