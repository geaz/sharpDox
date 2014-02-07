using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigSectionControl : UserControl
    {
        public static readonly DependencyProperty SectionHeaderProperty = DependencyProperty.Register("SectionHeader", typeof(string), typeof(ConfigSectionControl));
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ConfigSectionControl));
        
        public readonly LocalController _localController;

        public ConfigSectionControl(LocalController localController)
        {
            _localController = localController;
            
            DataContext = this;
            InitializeComponent();
        }

        private void ConfigSectionChanged(IConfigSection configSection)
        {
            configItemPanel.Children.Clear();
            foreach (var configItem in configSection.GetType().GetProperties())
            {
                var displayNameAttribute = (NameAttribute)Attribute.GetCustomAttribute(configItem, typeof(NameAttribute));
                if(displayNameAttribute != null)
                {
                    Binding b = new Binding(configItem.Name);
                    b.Source = configSection;
                    b.Mode = BindingMode.TwoWay;
                    b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                    var mandatoryAttribute = (MandatoryAttribute)Attribute.GetCustomAttribute(configItem, typeof(MandatoryAttribute));
                    var editorTypeAttribute = (ConfigEditorAttribute)Attribute.GetCustomAttribute(configItem, typeof(ConfigEditorAttribute));

                    if(editorTypeAttribute == null && configItem.PropertyType == typeof(string))
                    {          
                        var textItemControl = new ConfigTextControl();
                        textItemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
                        textItemControl.SetBinding(ConfigTextControl.ConfigItemValueProperty, b);
                        textItemControl.WaterMarkText = mandatoryAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory
                                                                                    : _localController.GetLocalStrings<SDGuiStrings>().Optional;
                        textItemControl.WaterMarkColor = mandatoryAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");

                        configItemPanel.Children.Add(textItemControl);
                    }
                    else if (editorTypeAttribute != null && (editorTypeAttribute.Editor == EditorType.Filepicker || editorTypeAttribute.Editor == EditorType.Folderpicker))
                    {
                        var fileSystemControl = new ConfigFileSystemControl();
                        fileSystemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
                        fileSystemControl.SetBinding(ConfigFileSystemControl.ConfigItemValueProperty, b);
                        fileSystemControl.WaterMarkText = mandatoryAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory
                                                                                    : _localController.GetLocalStrings<SDGuiStrings>().Optional;
                        fileSystemControl.WaterMarkColor = mandatoryAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");
                        fileSystemControl.IsFileSelector = editorTypeAttribute.Editor == EditorType.Filepicker;

                        configItemPanel.Children.Add(fileSystemControl);
                    }
                }
            }
        }

        private IConfigSection _configSection;
        public IConfigSection ConfigSection
        {
            get { return _configSection; }
            set 
            { 
                _configSection = value;
                ConfigSectionChanged(value);
            }
        }

        public string SectionHeader
        {
            get { return (string)GetValue(SectionHeaderProperty); }
            set { SetValue(SectionHeaderProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
    }
}
