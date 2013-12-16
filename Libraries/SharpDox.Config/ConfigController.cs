using System;
using System.IO;
using System.Xml.Linq;
using SharpDox.Sdk.Config;
using System.Collections.ObjectModel;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SharpDox.Config
{
    public class ConfigController : IConfigController
    {
        private XmlDocument _recentConfigs = new XmlDocument();

        private readonly IConfigSection[] _configSections;
        private readonly SharpDoxConfig _sharpDoxConfig;
        private readonly ConfigSerializer _configSerializer;

        public event Action OnRecentProjectsChanged;

        public ConfigController(IConfigSection[] configSections, SharpDoxConfig sharpDoxConfig)
        {
            _configSections = configSections;
            _sharpDoxConfig = sharpDoxConfig;
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

                _sharpDoxConfig.PathToConfig = fileToLoad;
                _sharpDoxConfig.ConfigFileName = Path.GetFileNameWithoutExtension(fileToLoad);
                _sharpDoxConfig.IsSaved = true;

                AddRecentConfig(_sharpDoxConfig.ConfigFileName, _sharpDoxConfig.PathToConfig);
            }
        }

        public void Save()
        {
            if (!String.IsNullOrEmpty(_sharpDoxConfig.PathToConfig))
            {
                var xml = _configSerializer.GetSerializedConfigs(_configSections);
                xml.Save(_sharpDoxConfig.PathToConfig);

                _sharpDoxConfig.IsSaved = true;

                AddRecentConfig(_sharpDoxConfig.ConfigFileName, _sharpDoxConfig.PathToConfig);
            }
        }

        public void SaveTo(string fileToSave)
        {
            if (!String.IsNullOrEmpty(fileToSave))
            {
                _sharpDoxConfig.PathToConfig = fileToSave;
                _sharpDoxConfig.ConfigFileName = Path.GetFileNameWithoutExtension(fileToSave);

                var xml = _configSerializer.GetSerializedConfigs(_configSections);
                xml.Save(fileToSave);

                _sharpDoxConfig.IsSaved = true;

                AddRecentConfig(_sharpDoxConfig.ConfigFileName, _sharpDoxConfig.PathToConfig);
            }
        }

        public T GetConfigSection<T>()
        {
            return (T)_configSections.SingleOrDefault(c => c is T);
        }

        private void GetRecentConfigs()
        {
            var recentFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "recent.xml");
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
                config.PropertyChanged += (o, e) => { if (e.PropertyName != "IsSaved") _sharpDoxConfig.IsSaved = false; };
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
                if (property.CanWrite)
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
