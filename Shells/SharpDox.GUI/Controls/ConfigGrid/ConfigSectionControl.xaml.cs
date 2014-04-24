using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SharpDox.Sdk.Config.Lists;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigSectionControl : UserControl
    {
        public static readonly DependencyProperty SectionHeaderProperty = DependencyProperty.Register("SectionHeader", typeof(string), typeof(ConfigSectionControl));
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ConfigSectionControl));

        private readonly LocalController _localController;
        private readonly IExporter[] _allExporters;
        private readonly IBuildController _buildController;
        private readonly ICoreConfigSection _coreConfigSection;

        public ConfigSectionControl(LocalController localController, ICoreConfigSection coreConfigSection, IExporter[] allExporters, IBuildController buildController)
        {
            _localController = localController;
            _coreConfigSection = coreConfigSection;
            _allExporters = allExporters;
            _buildController = buildController;

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

                    var requiredAttribute = (RequiredAttribute)Attribute.GetCustomAttribute(configItem, typeof(RequiredAttribute));
                    var editorTypeAttribute = (ConfigEditorAttribute)Attribute.GetCustomAttribute(configItem, typeof(ConfigEditorAttribute));

                    if (ConfigSection.Guid == new Guid("FEACBCE2-8290-4D90-BB05-373B9D7DBBFC") && configItem.Name == "ExcludedIdentifiers")
                    {
                        CreateVisibilityControl(displayNameAttribute);
                    }
                    else if (editorTypeAttribute != null && editorTypeAttribute.Editor == EditorType.CheckBoxList && ConfigSection.Guid == new Guid("FEACBCE2-8290-4D90-BB05-373B9D7DBBFC") && configItem.Name == "ActivatedExporters")
                    {
                        var exporterList = new CheckBoxList();
                        foreach (var exporter in _allExporters)
                        {
                            exporterList.Add(exporter.ExporterName);
                        }

                        CreateCheckboxListControl(displayNameAttribute, exporterList, b);
                    }
                    else if (editorTypeAttribute != null && editorTypeAttribute.Editor == EditorType.CheckBoxList && editorTypeAttribute.SourceListType != null)
                    {
                        CreateCheckboxListControl(displayNameAttribute, (CheckBoxList)Activator.CreateInstance(editorTypeAttribute.SourceListType) ,b);
                    }
                    else if(editorTypeAttribute != null && editorTypeAttribute.Editor == EditorType.Markdown)
                    {
                        CreateMarkdownControl(displayNameAttribute, requiredAttribute, b);
                    }
                    else if(editorTypeAttribute == null && configItem.PropertyType == typeof(string))
                    {
                        CreateTextboxControl(displayNameAttribute, requiredAttribute, b);
                    }
                    else if (editorTypeAttribute == null && configItem.PropertyType == typeof(bool))
                    {
                        CreateCheckboxControl(displayNameAttribute, b);
                    }
                    else if (editorTypeAttribute != null && editorTypeAttribute.Editor == EditorType.ComboBox && editorTypeAttribute.SourceListType != null)
                    {
                        CreateComboboxControl(displayNameAttribute, requiredAttribute, editorTypeAttribute, b);
                    }
                    else if (editorTypeAttribute != null && editorTypeAttribute.Editor == EditorType.Colorpicker)
                    {
                        CreateColorpickerControl(displayNameAttribute, b);
                    }
                    else if (editorTypeAttribute != null && (editorTypeAttribute.Editor == EditorType.Filepicker || editorTypeAttribute.Editor == EditorType.Folderpicker))
                    {
                        CreateFilesystemControl(displayNameAttribute, requiredAttribute, editorTypeAttribute, b);
                    }
                }
            }
        }

        private void CreateVisibilityControl(NameAttribute displayNameAttribute)
        {
            var visibilityControl = new ConfigVisibilityControl(_localController.GetLocalStrings<SDGuiStrings>(), _coreConfigSection, _buildController);
            visibilityControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);

            configItemPanel.Children.Add(visibilityControl);
        }

        private void CreateCheckboxListControl(NameAttribute displayNameAttribute, CheckBoxList sourceList, Binding b)
        {
            var checkBoxListControl = new ConfigCheckBoxListControl(_localController.GetLocalStrings<SDGuiStrings>());
            checkBoxListControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            checkBoxListControl.SourceList = sourceList;
            checkBoxListControl.SetBinding(ConfigCheckBoxListControl.SelectedItemsProperty, b);

            configItemPanel.Children.Add(checkBoxListControl);
        }

        private void CreateMarkdownControl(NameAttribute displayNameAttribute, RequiredAttribute requiredAttribute, Binding b)
        {
            var markdownItemControl = new ConfigMarkdownControl(_localController.GetLocalStrings<SDGuiStrings>());
            markdownItemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            markdownItemControl.SetBinding(ConfigMarkdownControl.ConfigItemValueProperty, b);
            markdownItemControl.WaterMarkText = requiredAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory : _localController.GetLocalStrings<SDGuiStrings>().Optional;
            markdownItemControl.WaterMarkColor = requiredAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");

            configItemPanel.Children.Add(markdownItemControl);
        }

        private void CreateTextboxControl(NameAttribute displayNameAttribute, RequiredAttribute requiredAttribute, Binding b)
        {
            var textItemControl = new ConfigTextControl();
            textItemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            textItemControl.SetBinding(ConfigTextControl.ConfigItemValueProperty, b);
            textItemControl.WaterMarkText = requiredAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory : _localController.GetLocalStrings<SDGuiStrings>().Optional;
            textItemControl.WaterMarkColor = requiredAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");

            configItemPanel.Children.Add(textItemControl);
        }

        private void CreateCheckboxControl(NameAttribute displayNameAttribute, Binding b)
        {
            var boolItemControl = new ConfigBoolControl();
            boolItemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            boolItemControl.SetBinding(ConfigBoolControl.ConfigItemValueProperty, b);

            configItemPanel.Children.Add(boolItemControl);
        }

        private void CreateComboboxControl(NameAttribute displayNameAttribute, RequiredAttribute requiredAttribute, ConfigEditorAttribute editorTypeAttribute, Binding b)
        {
            var dropDownControl = new ConfigComboBoxControl();
            dropDownControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            dropDownControl.SourceList = (ComboBoxList)Activator.CreateInstance(editorTypeAttribute.SourceListType);
            dropDownControl.SetBinding(ConfigComboBoxControl.SelectedValueProperty, b);
            dropDownControl.WaterMarkText = requiredAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory : _localController.GetLocalStrings<SDGuiStrings>().Optional;
            dropDownControl.WaterMarkColor = requiredAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");

            configItemPanel.Children.Add(dropDownControl);
        }

        private void CreateColorpickerControl(NameAttribute displayNameAttribute, Binding b)
        {
            var colorControl = new ConfigColorControl();
            colorControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            colorControl.SetBinding(ConfigColorControl.ConfigItemValueProperty, b);

            configItemPanel.Children.Add(colorControl);
        }

        private void CreateFilesystemControl(NameAttribute displayNameAttribute, RequiredAttribute requiredAttribute, ConfigEditorAttribute editorTypeAttribute, Binding b)
        {
            var fileSystemControl = new ConfigFileSystemControl();
            fileSystemControl.ConfigItemDisplayName = _localController.GetLocalString(displayNameAttribute.LocalType, displayNameAttribute.DisplayName);
            fileSystemControl.SetBinding(ConfigFileSystemControl.ConfigItemValueProperty, b);
            fileSystemControl.WaterMarkText = requiredAttribute != null ? _localController.GetLocalStrings<SDGuiStrings>().Mandatory : _localController.GetLocalStrings<SDGuiStrings>().Optional;
            fileSystemControl.WaterMarkColor = requiredAttribute != null ? (SolidColorBrush)TryFindResource("Color_FadedRed") : (SolidColorBrush)TryFindResource("Color_FadedGray");
            fileSystemControl.IsFileSelector = editorTypeAttribute.Editor == EditorType.Filepicker;
            fileSystemControl.OpenFileFilter = editorTypeAttribute.OpenFileFilter;

            configItemPanel.Children.Add(fileSystemControl);
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
