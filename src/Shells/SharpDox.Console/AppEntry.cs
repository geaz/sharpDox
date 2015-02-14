using System;
using Autofac;
using SharpDox.Core;

namespace SharpDox.Console
{
    public static class AppEntry
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var mainContainerConfig = new MainContainerConfig();
            mainContainerConfig.RegisterAsSelf<SDConsole>();
            mainContainerConfig.RegisterStrings<SDConsoleStrings>();
            var mainContainer = mainContainerConfig.BuildContainer();

            mainContainer.Resolve<SDConsole>().Start(args);
        }
    }
}
