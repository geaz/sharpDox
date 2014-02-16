using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SharpDox.GUI.Command;
using SharpDox.Sdk.Config.Lists;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigCheckBoxListControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigCheckBoxListControl));
        public static readonly DependencyProperty SourceListProperty = DependencyProperty.Register("SourceList", typeof(CheckBoxList), typeof(ConfigCheckBoxListControl), new FrameworkPropertyMetadata(null, OnSourceListChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ConfigCheckBoxListControl));
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(string), typeof(ConfigCheckBoxListControl));

        public readonly SDGuiStrings _strings;

        public ConfigCheckBoxListControl(SDGuiStrings strings)
        {
            _strings = strings;

            DataContext = this;
            InitializeComponent();
        }

        private void UpdateText()
        {
            SelectedItems = Text = string.Join("; ", SourceList.Where(i => i.IsChecked).Select(i => i.Name));
            if (string.IsNullOrEmpty(Text))
            {
                Text = _strings.NoneSelected;
            }
        }

        private static void OnSourceListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ConfigCheckBoxListControl)d;
            control.UpdateText();
        }

        public string ConfigItemDisplayName
        {
            get { return (string)GetValue(ConfigItemDisplayNameProperty); }
            set { SetValue(ConfigItemDisplayNameProperty, value); }
        }

        public CheckBoxList SourceList
        {
            get { return (CheckBoxList)GetValue(SourceListProperty); }
            set { SetValue(SourceListProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string SelectedItems
        {
            get { return (string)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public RelayCommand SelectionChanged
        {
            get { return new RelayCommand(UpdateText, true);}
        }
    }
}
