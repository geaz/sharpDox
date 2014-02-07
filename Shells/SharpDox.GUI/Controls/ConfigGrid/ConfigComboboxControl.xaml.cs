using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SharpDox.Sdk.Config.Lists;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigComboBoxControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigComboBoxControl));
        public static readonly DependencyProperty SourceListProperty = DependencyProperty.Register("SourceList", typeof(ComboBoxList), typeof(ConfigComboBoxControl));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(ConfigComboBoxControl));
        public static readonly DependencyProperty WaterMarkTextProperty = DependencyProperty.Register("WaterMarkText", typeof(string), typeof(ConfigComboBoxControl));
        public static readonly DependencyProperty WaterMarkColorProperty = DependencyProperty.Register("WaterMarkColor", typeof(SolidColorBrush), typeof(ConfigComboBoxControl));

        public ConfigComboBoxControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string ConfigItemDisplayName
        {
            get { return (string)GetValue(ConfigItemDisplayNameProperty); }
            set { SetValue(ConfigItemDisplayNameProperty, value); }
        }

        public ComboBoxList SourceList
        {
            get { return (ComboBoxList)GetValue(SourceListProperty); }
            set { SetValue(SourceListProperty, value); }
        }

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
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
