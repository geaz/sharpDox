using System;
using System.Diagnostics;
using Autofac;
using log4net.Config;
using SharpDox.Core;

namespace SharpDox.Console
{
    public static class AppEntry
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure();

                var mainContainerConfig = new MainContainerConfig();
                mainContainerConfig.RegisterAsSelf<SDConsole>();
                mainContainerConfig.RegisterStrings<SDConsoleStrings>();
                var mainContainer = mainContainerConfig.BuildContainer();
                
                mainContainer.Resolve<SDConsole>().Start(args);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
