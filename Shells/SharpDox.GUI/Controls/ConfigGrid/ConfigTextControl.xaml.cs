using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigTextControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigTextControl));
        public static readonly DependencyProperty ConfigItemValueProperty = DependencyProperty.Register("ConfigItemValue", typeof(string), typeof(ConfigTextControl));
        public static readonly DependencyProperty WaterMarkTextProperty = DependencyProperty.Register("WaterMarkText", typeof(string), typeof(ConfigTextControl));
        public static readonly DependencyProperty WaterMarkColorProperty = DependencyProperty.Register("WaterMarkColor", typeof(SolidColorBrush), typeof(ConfigTextControl));

        public ConfigTextControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string ConfigItemDisplayName
        {
            get { return (string)GetValue(ConfigItemDisplayNameProperty); }
            set { SetValue(ConfigItemDisplayNameProperty, value); }
        }

        public string ConfigItemValue
        {
            get { return (string)GetValue(ConfigItemValueProperty); }
            set { SetValue(ConfigItemValueProperty, value); }
        }

        public string WaterMarkText
        {
            get { return (string)GetValue(WaterMarkTextProperty); }
            set { SetValue(WaterMarkTextProperty, value); }
        }

        public SolidColorBrush WaterMarkColor
        {
            get { return (SolidColorBrush)GetValue(WaterMarkColorProperty); }
            set { SetValue(WaterMarkColorProperty, value); }
        }
    }
}
