using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;

namespace SharpDox.Core.Config
{
    public class ConfigController : IConfigController
    {
        private XmlDocument _recentConfigs = new XmlDocument();

        private readonly IConfigSection[] _configSections;
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly ConfigSerializer _configSerializer;

        public event Action OnRecentProjectsChanged;

        public ConfigController(IConfigSection[] configSections, ICoreConfigSection coreConfigSection)
        {
            _configSections = configSections;
            _coreConfigSection = coreConfigSection;
            _configSerializer = new ConfigSerializer();

            RecentProjects = new List<KeyValuePair<string, string>>();
            GetRecentConfigs();

            RegisterEventOnConfigs(configSections);
            New();
        }

        public void New()
        {
            ResetConfigs();
        }

        public void Load(string fileToLoad)
        {
            if (File.Exists(fileToLoad))
            {
                ResetConfigs();
                               
                _configSerializer.SetDeserializedConfigs(XDocument.Load(fileToLoad), _configSections);

                _coreConfigSection.PathToConfig = fileToLoad;
                _coreConfigSection.ConfigFileName = Path.GetFileNameWithoutExtension(fileToLoad);
                _coreConfigSection.IsSaved = true;

                AddRecentConfig(_coreConfigSection.ConfigFileName, _coreConfigSection.PathToConfig);
            }
        }

        public void Save()
        {
            if (!String.IsNullOrEmpty(_coreConfigSection.PathToConfig))
            {
                var xml = _configSerializer.GetSerializedConfigs(_configSections);
                xml.Save(_coreConfigSection.PathToConfig);

                _coreConfigSection.IsSaved = true;

                AddRecentConfig(_coreConfigSection.ConfigFileName, _coreConfigSection.PathToConfig);
            }
        }

        public void SaveTo(string fileToSave)
        {
            if (!String.IsNullOrEmpty(fileToSave))
            {
                _coreConfigSection.PathToConfig = fileToSave;
                _coreConfigSection.ConfigFileName = Path.GetFileNameWithoutExtension(fileToSave);

                var xml = _configSerializer.GetSerializedConfigs(_configSections);
                xml.Save(fileToSave);

                _coreConfigSection.IsSaved = true;

                AddRecentConfig(_coreConfigSection.ConfigFileName, _coreConfigSection.PathToConfig);
            }
        }

        public IEnumerable<IConfigSection> GetAllConfigSections()
        {
            return _configSections;
        }

        public T GetConfigSection<T>()
        {
            return (T)_configSections.SingleOrDefault(c => c is T);
        }

        private void GetRecentConfigs()
        {
            #if DEBUG
            var recentFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "recent.xml");
            #else 
            var recentFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "recent.xml");
            #endif

            if (File.Exists(recentFile))
            {
                _recentConfigs.Load(recentFile);
                var nodes = _recentConfigs.GetElementsByTagName("projectfile");
                foreach (XmlElement node in nodes)
                {
                    RecentProjects.Add(new KeyValuePair<string, string>(node.Attributes["path"].Value, node.Attributes["name"].Value));
                }
            }

            ExecuteOnRecentProjectsChanged();
        }

        private void RegisterEventOnConfigs(IConfigSection[] configSections)
        {
            foreach (var config in configSections)
            {
                config.PropertyChanged += (o, e) => { if (e.PropertyName != "IsSaved") _coreConfigSection.IsSaved = false; };
            }
        }

        private void ResetConfigs()
        {
            foreach (var config in _configSections)
            {
                ResetAllProperties(config);
            }
        }

        private void ResetAllProperties(IConfigSection config)
        {
            foreach (var property in config.GetType().GetProperties())
            {
                var excludeAttribute = (ExcludeAttribute)Attribute.GetCustomAttribute(property, typeof(ExcludeAttribute));
                if (property.CanWrite && excludeAttribute == null)
                {
                    if (property.PropertyType.Name == "ObservableCollection`1")
                    {
                        property.SetValue(config, new ObservableCollection<string>(), null);
                    }
                    else
                    {
                        property.SetValue(config, null, null);
                    }
                }
            }
        }

        private void AddRecentConfig(string name, string pathToConfig)
        {
            var keyValue = RecentProjects.SingleOrDefault(s => s.Key == pathToConfig);
            if (keyValue.Equals(null) || keyValue.Value != name)
            {
                RecentProjects.Insert(0, new KeyValuePair<string, string>(pathToConfig, name));
            }

            if (RecentProjects.Count > 5)
            {
                RecentProjects.RemoveAt(5);
            }

            _recentConfigs = new XmlDocument();
            var root = _recentConfigs.CreateElement("recentprojects");

            foreach (var recentFile in RecentProjects)
            {
                var file = _recentConfigs.CreateElement("projectfile");

                var nameAttr = _recentConfigs.CreateAttribute("name");
                nameAttr.Value = recentFile.Value;

                var pathAttr = _recentConfigs.CreateAttribute("path");
                pathAttr.Value = recentFile.Key;

                file.Attributes.Append(nameAttr);
                file.Attributes.Append(pathAttr);

                root.AppendChild(file);
            }
            _recentConfigs.AppendChild(root);
            _recentConfigs.Save(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "recent.xml"));

            ExecuteOnRecentProjectsChanged();
        }

        private void ExecuteOnRecentProjectsChanged()
        {
            var handlers = OnRecentProjectsChanged;
            if (handlers != null)
            {
                handlers();
            }
        }

        public List<KeyValuePair<string, string>> RecentProjects { get; private set; }
    }
}
