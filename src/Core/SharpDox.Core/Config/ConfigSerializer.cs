using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SharpDox.Sdk;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;

namespace SharpDox.Core.Config
{
    internal class ConfigSerializer
    {
        public XDocument GetSerializedConfigs(IConfigSection[] configs)
        {
            var configXml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("SDConfig"));
            SerializeConfigSections(configs, configXml);
            return configXml;
        }

        public void SetDeserializedConfigs(XDocument xmlConfig, IConfigSection[] configs)
        {
            foreach (var section in xmlConfig.Root.Elements())
            {
                var config = GetConfigSection(configs, section);
                if (config != null)
                {
                    GetConfigValuesFromXml(section, config);
                }
            }
        }

        private void SerializeConfigSections(IEnumerable<IConfigSection> configs, XDocument configXml)
        {
            foreach (var section in configs)
            {
                var xmlSection = new XElement("section", new XAttribute("guid", section.Guid.ToString()));
                SerializeProperties(section, xmlSection);
                configXml.Root.Add(xmlSection);
            }
        }

        private void SerializeProperties(IConfigSection section, XElement xmlSection)
        {
            foreach (var item in section.GetType().GetProperties())
            {
                if (PropertyIsRelevant(item, section))
                {
                    SerializeProperty(xmlSection, item, section);
                }
            }
        }

        private bool PropertyIsRelevant(PropertyInfo item, IConfigSection section)
        {
            return item.GetValue(section, null) != null && !item.Name.Equals("Guid") && !item.Name.Equals("IsSaved");
        }

        private void SerializeProperty(XElement xmlSection, PropertyInfo item, IConfigSection section)
        {
            if (item.PropertyType.Name == "ObservableCollection`1")
            {
                var xmlProperty = new XElement("item", new XAttribute("key", item.Name));
                var list = (IEnumerable<string>)item.GetValue(section, null);
                AddValueToProperty(xmlProperty, string.Join(";", list));
                xmlSection.Add(xmlProperty);
            }
            else if (item.PropertyType.Name == "SDPath")
            {
                var path = item.GetValue(section, null) as SDPath;
                if (path != null)
                {
                    path.UpdatePath();

                    var xmlProperty = new XElement(item.Name);
                    xmlProperty.Add(new XElement("Relative")
                    {
                        Value = path.RelativePath
                    });
                    xmlProperty.Add(new XElement("Full")
                    {
                        Value = path.FullPath
                    });

                    xmlSection.Add(xmlProperty);
                }
            }
            else
            {
                var xmlProperty = new XElement("item", new XAttribute("key", item.Name));
                AddValueToProperty(xmlProperty, item.GetValue(section, null).ToString());
                xmlSection.Add(xmlProperty);
            }
        }

        private void AddValueToProperty(XElement propertyElement, string value)
        {
            var xmlAtrribute = new XAttribute("value", value);
            propertyElement.Add(xmlAtrribute);
        }

        private IConfigSection GetConfigSection(IEnumerable<IConfigSection> configs, XElement section)
        {
            var config = configs.Where(o => o.Guid.ToString() == section.Attribute("guid").Value)
                           .ToList()
                           .SingleOrDefault();
            return config;
        }

        private void GetConfigValuesFromXml(XElement section, IConfigSection config)
        {
            foreach (var item in section.Elements())
            {
                GetAndSetConfigValue(config, item);
            }
        }

        private void GetAndSetConfigValue(IConfigSection config, XElement item)
        {
            var propertyName = string.Empty;

            var keyAttribute = item.Attribute("key");
            if (keyAttribute != null)
            {
                propertyName = keyAttribute.Value;
            }
            else
            {
                propertyName = item.Name.LocalName;
            }
            
            var property = config.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var excludeAttribute = (ExcludeAttribute)Attribute.GetCustomAttribute(property, typeof(ExcludeAttribute));
                if (excludeAttribute == null)
                {
                    if (property.PropertyType.Name == "Boolean")
                    {
                        property.SetValue(config, item.Attribute("value").Value.ToLower() == "true", null);
                    }
                    else if (property.PropertyType.Name == "ObservableCollection`1")
                    {
                        property.SetValue(config, new ObservableCollection<string>(item.Attribute("value").Value.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()), null);
                    }
                    else if (property.PropertyType == typeof(SDPath) && item.Element("Full") != null)
                    {
                        var fullPath = item.Element("Full").Value;
                        var relativePath = item.Element("Relative").Value;

                        var path = new SDPath(fullPath, relativePath);
                        property.SetValue(config, path, null);
                    }
                    else if (property.PropertyType == typeof(SDPath))
                    {
                        // Backwards compatibility code
                        var fullPath = item.Attribute("value").Value;

                        var path = new SDPath(fullPath);
                        property.SetValue(config, path, null);
                    }
                    else
                    {
                        property.SetValue(config, item.Attribute("value").Value, null);
                    }
                }
            }
        }
    }
}
