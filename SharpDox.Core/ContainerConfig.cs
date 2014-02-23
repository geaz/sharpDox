using System.IO;
using System.Reflection;
using Autofac;
using SharpDox.Build;
using SharpDox.Config;
using SharpDox.Core.Config;
using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Local;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.UI;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Core
{
    internal class ContainerConfig
    {
        private IContainer _container;

        private readonly ContainerBuilder _containerBuilder;

        public ContainerConfig()
        {
            _containerBuilder = new ContainerBuilder();
            RegisterDefaultComponents();
        }

        public IContainer BuildContainer()
        {
            return _container ?? (_container = _containerBuilder.Build());
        }

        private void RegisterDefaultComponents()
        {
            RegisterLocalStrings();
            RegisterConfiguration();
            RegisterBuilder();
            RegisterShells();
            RegisterPlugins();
        }

        private void RegisterLocalStrings()
        {
            _containerBuilder.RegisterType<LocalController>().AsSelf().SingleInstance();
            _containerBuilder.RegisterType<CoreStrings>().AsSelf().As<ILocalStrings>().SingleInstance();
            _containerBuilder.RegisterType<SDBuildStrings>().AsSelf().As<ILocalStrings>().SingleInstance();
        }

        private void RegisterConfiguration()
        {
            _containerBuilder.RegisterType<ConfigController>().AsSelf().As<IConfigController>().SingleInstance();
            _containerBuilder.RegisterType<CoreConfigSection>().AsSelf().As<ICoreConfigSection>().As<IConfigSection>().SingleInstance();
        }

        private void RegisterBuilder()
        {
            _containerBuilder.RegisterType<BuildController>().As<IBuildController>().SingleInstance();
            _containerBuilder.RegisterType<BuildMessenger>().As<IBuildMessenger>().SingleInstance();
        }

        private void RegisterShells()
        {
            var shellPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "shells");
            if (Directory.Exists(shellPath))
            {
                RegisterAllInPath(shellPath, "*dll");
            }
        }

        private void RegisterPlugins()
        {
            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            if (Directory.Exists(pluginPath))
            {
                RegisterAllInPath(pluginPath, "*dll");
                RegisterAllPluginsInSubDirectory(pluginPath);
            }
        }

        private void RegisterAllInPath(string path, string filter)
        {
            var possiblePlugins = Directory.EnumerateFiles(path, filter);
            foreach (var possiblePlugin in possiblePlugins)
            {
                var pluginAssembly = Assembly.LoadFrom(possiblePlugin);
               
                _containerBuilder.RegisterAssemblyTypes(pluginAssembly).Where(o => typeof(IShell).IsAssignableFrom(o)).As<IShell>().SingleInstance();
                _containerBuilder.RegisterAssemblyTypes(pluginAssembly).Where(o => typeof(IConfigSection).IsAssignableFrom(o)).AsSelf().As<IConfigSection>().SingleInstance();
                _containerBuilder.RegisterAssemblyTypes(pluginAssembly).Where(o => typeof(ILocalStrings).IsAssignableFrom(o)).AsSelf().As<ILocalStrings>().SingleInstance();
                _containerBuilder.RegisterAssemblyTypes(pluginAssembly).Where(o => typeof(IExporter).IsAssignableFrom(o)).As<IExporter>();
            }
        }

        private void RegisterAllPluginsInSubDirectory(string pluginPath)
        {
            var pluginSubPaths = Directory.EnumerateDirectories(pluginPath);
            foreach (var subPath in pluginSubPaths)
            {
                RegisterAllInPath(subPath, "*dll");
            }
        }
    }
}
