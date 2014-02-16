using SharpDox.GUI.Command;
using System;
using System.Windows;
using System.Windows.Controls;
using SharpDox.GUI.Windows;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigVisibilityControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigVisibilityControl));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ConfigVisibilityControl));

        private VisibilityEditorView _visibilityEditor;

        private readonly SDGuiStrings _strings;
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly IBuildController _buildController;

        public ConfigVisibilityControl(SDGuiStrings strings, ICoreConfigSection coreConfigSection, IBuildController buildController)
        {
            _strings = strings;
            _coreConfigSection = coreConfigSection;
            _buildController = buildController;

            coreConfigSection.PropertyChanged += ExcludedIdentifiersChanged;
            ExcludedIdentifiersChanged(null, null);

            DataContext = this;
            InitializeComponent();
        }

        void ExcludedIdentifiersChanged(object sender, EventArgs e)
        {
            if (_coreConfigSection.ExcludedIdentifiers.Count == 0)
            {
                Text = string.Format("{0} {1} {2}", _strings.None, _strings.Elements, _strings.Excluded);
            }
            else
            {
                Text = string.Format("{0} {1} {2}", _coreConfigSection.ExcludedIdentifiers.Count, _strings.Elements, _strings.Excluded);
            }
        }

        public string ConfigItemDisplayName
        {
            get { return (string)GetValue(ConfigItemDisplayNameProperty); }
            set { SetValue(ConfigItemDisplayNameProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public RelayCommand ButtonCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _visibilityEditor = _visibilityEditor ?? new VisibilityEditorView(_strings, _coreConfigSection, _buildController);
                    _visibilityEditor.Show();
                }, true);
            }
        }
    }
}
