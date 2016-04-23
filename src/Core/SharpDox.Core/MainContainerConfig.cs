using System;
using Autofac;
using SharpDox.Build;
using SharpDox.Core.Config;
using SharpDox.Core.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Local;
using System.IO;
using System.Reflection;
using SharpDox.Build.Roslyn;

namespace SharpDox.Core
{
    public class MainContainerConfig
    {
        private IContainer _container;

        private readonly ContainerBuilder _containerBuilder;

        public MainContainerConfig()
        {
            _containerBuilder = new ContainerBuilder();
            RegisterDefaultComponents();
        }

        public void RegisterAsSelf<T>()
        {
            if (_container != null)
            {
                throw new Exception("Can't add type after building the container");
            }
            _containerBuilder.RegisterType<T>().AsSelf();
        }

        public void RegisterStrings<T>()
        {
            if (_container != null)
            {
                throw new Exception("Can't add type after building the container");
            }
            _containerBuilder.RegisterType<T>().AsSelf().As<ILocalStrings>();
        } 

        public IContainer BuildContainer()
        {
            return _container ?? (_container = _containerBuilder.Build());
        }

        private void RegisterDefaultComponents()
        {
            RegisterLocalStrings();
            RegisterConfiguration();
            RegisterPlugins();
        }

        private void RegisterLocalStrings()
        {
            _containerBuilder.RegisterType<BuildController>().AsSelf().SingleInstance();
            _containerBuilder.RegisterType<BuildMessenger>().AsSelf().SingleInstance();
            _containerBuilder.RegisterType<ConfigController>().As<IConfigController>().SingleInstance();
            _containerBuilder.RegisterType<LocalController>().AsSelf().As<ILocalController>().SingleInstance();
            _containerBuilder.RegisterType<CoreStrings>().AsSelf().As<ILocalStrings>().SingleInstance();
            _containerBuilder.RegisterType<RoslynParser>().As<ICodeParser>().SingleInstance();
            _containerBuilder.RegisterType<SDBuildStrings>().AsSelf().As<ILocalStrings>().SingleInstance();
            _containerBuilder.RegisterType<ParserStrings>().AsSelf().As<ILocalStrings>().SingleInstance();
        }

        private void RegisterConfiguration()
        {
            _containerBuilder.RegisterType<CoreConfigSection>().AsSelf().As<ICoreConfigSection>().As<IConfigSection>().SingleInstance();
        }

        private void RegisterPlugins()
        {
            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "plugins");
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
