using System;
using SharpDox.Local;
using SharpDox.Sdk.Config;
using System.Windows.Controls;
using SharpDox.Sdk.Config.Attributes;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigGridControl : UserControl
    {
        private readonly IConfigSection[] _configSections;
        private readonly LocalController _localController;

        public ConfigGridControl(IConfigSection[] configSections, LocalController localController)
        {
            _localController = localController;
            _configSections = configSections;

            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            foreach (var configSection in _configSections)
            {
                AddSection(configSection);
            }
            ExpandFirstSection();
        }

        private void AddSection(IConfigSection configSection)
        {
            var displayNameAttribute = (NameAttribute)Attribute.GetCustomAttribute(configSection.GetType(), typeof(NameAttribute));
            if (displayNameAttribute != null)
            {        
                var configSectionControl = new ConfigSectionControl(_localController);
                configSectionControl.SectionHeader = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
                configSectionControl.ConfigSection = configSection;

                configPanel.Children.Add(configSectionControl);
            }
        }

        private void ExpandFirstSection()
        {
            if(configPanel.Children.Count > 0)
            {
                ((ConfigSectionControl)configPanel.Children[0]).IsExpanded = true;
            }
        }
    }
}
