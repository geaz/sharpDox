using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigBoolControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigBoolControl));
        public static readonly DependencyProperty ConfigItemValueProperty = DependencyProperty.Register("ConfigItemValue", typeof(bool), typeof(ConfigBoolControl));

        public ConfigBoolControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string ConfigItemDisplayName
        {
            get { return (string)GetValue(ConfigItemDisplayNameProperty); }
            set { SetValue(ConfigItemDisplayNameProperty, value); }
        }

        public bool ConfigItemValue
        {
            get { return (bool)GetValue(ConfigItemValueProperty); }
            set { SetValue(ConfigItemValueProperty, value); }
        }
    }
}
