using SharpDox.GUI.Command;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigMarkdownControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigMarkdownControl));
        public static readonly DependencyProperty ConfigItemValueProperty = DependencyProperty.Register("ConfigItemValue", typeof(string), typeof(ConfigMarkdownControl));
        public static readonly DependencyProperty WaterMarkTextProperty = DependencyProperty.Register("WaterMarkText", typeof(string), typeof(ConfigMarkdownControl));
        public static readonly DependencyProperty WaterMarkColorProperty = DependencyProperty.Register("WaterMarkColor", typeof(SolidColorBrush), typeof(ConfigMarkdownControl));

        private readonly SDGuiStrings _strings;

        public ConfigMarkdownControl(SDGuiStrings strings)
        {
            _strings = strings;

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

        public RelayCommand ItalicButtonCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var index = tbValue.CaretIndex;
                    tbValue.Focus();

                    tbValue.Text = tbValue.Text.Remove(tbValue.SelectionStart, tbValue.SelectionLength);
                    tbValue.Text = tbValue.Text.Insert(index, string.Format("*{0}*", _strings.Text));
                    tbValue.Select(index + 1, _strings.Text.Length);
                }, true);
            }
        }

        public RelayCommand BoldButtonCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var index = tbValue.CaretIndex;
                    tbValue.Focus();

                    tbValue.Text = tbValue.Text.Remove(tbValue.SelectionStart, tbValue.SelectionLength);
                    tbValue.Text = tbValue.Text.Insert(index, string.Format("**{0}**", _strings.Text));
                    tbValue.Select(index + 2, _strings.Text.Length);
                }, true);
            }
        }

        public RelayCommand LinkButtonCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var index = tbValue.CaretIndex;
                    tbValue.Focus();

                    tbValue.Text = tbValue.Text.Remove(tbValue.SelectionStart, tbValue.SelectionLength);
                    tbValue.Text = tbValue.Text.Insert(index, string.Format("[{0}]({1} \"{2}\")", _strings.LinkText, _strings.Link, _strings.LinkTitle));                    
                }, true);
            }
        }
    }
}
