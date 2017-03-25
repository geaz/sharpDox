using System;
using System.Linq;
using SharpDox.Build;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Local;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigGridControl
    {
        private readonly IConfigController _configController;
        private readonly ILocalController _localController;
        private readonly IExporter[] _allExporters;
        private readonly BuildController _buildController;

        public ConfigGridControl(IConfigController configController, IExporter[] allExporters, ILocalController localController, BuildController buildController)
        {
            _allExporters = allExporters;
            _localController = localController;
            _configController = configController;
            _buildController = buildController;

            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            foreach (var configSection in _configController.GetAllConfigSections())
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
                var configSectionControl = new ConfigSectionControl(_localController, _configController.GetConfigSection<ICoreConfigSection>(), _allExporters, _buildController);
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
