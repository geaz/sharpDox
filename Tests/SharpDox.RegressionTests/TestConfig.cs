using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDox.Build.Context;
using SharpDox.Build;
using SharpDox.Sdk.Config;
using Moq;
using System.IO;
using System.Reflection;

namespace SharpDox.RegressionTests
{
    internal static class TestConfig
    {
        internal static Steps GetSteps()
        {
            var testProjectPath =
                Path.Combine(
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(Regression1)).Location),
                "..", "..", "..", "SharpDox.TestProject", "SharpDox.TestProject.csproj");

            var configController = Mock.Of<IConfigController>();
            var coreConfig = Mock.Of<ICoreConfigSection>(
                c => c.InputFile == testProjectPath &&
                c.ProjectName == "TestProject" &&
                c.DocLanguage == "en");

            return new Steps(coreConfig, new SDBuildStrings(), configController, new BuildMessenger());
        }
    }
}
