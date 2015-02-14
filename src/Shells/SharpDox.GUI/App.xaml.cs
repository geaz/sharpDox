using System.Windows;
using Autofac;
using SharpDox.Core;

namespace SharpDox.GUI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainContainerConfig = new MainContainerConfig();
            mainContainerConfig.RegisterAsSelf<Shell>();
            mainContainerConfig.RegisterStrings<SDGuiStrings>();
            var mainContainer = mainContainerConfig.BuildContainer();

            mainContainer.Resolve<Shell>().Show();
        }
    }
}
