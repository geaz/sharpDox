using Microsoft.Win32;
using SharpDox.GUI.Command;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SharpDox.GUI.Controls.ConfigGrid
{
    public partial class ConfigFileSystemControl : UserControl
    {
        public static readonly DependencyProperty ConfigItemDisplayNameProperty = DependencyProperty.Register("ConfigItemDisplayName", typeof(string), typeof(ConfigFileSystemControl));
        public static readonly DependencyProperty ConfigItemValueProperty = DependencyProperty.Register("ConfigItemValue", typeof(string), typeof(ConfigFileSystemControl));
        public static readonly DependencyProperty WaterMarkTextProperty = DependencyProperty.Register("WaterMarkText", typeof(string), typeof(ConfigFileSystemControl));
        public static readonly DependencyProperty WaterMarkColorProperty = DependencyProperty.Register("WaterMarkColor", typeof(SolidColorBrush), typeof(ConfigFileSystemControl));
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(ConfigFileSystemControl));
        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register("ButtonCommand", typeof(RelayCommand), typeof(ConfigFileSystemControl));
        
        public ConfigFileSystemControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void SetMode()
        {
            if(IsFileSelector)
            {
                ButtonText = "\uF15C";
                ButtonCommand = new RelayCommand(() => 
                {
                    var dlg = new OpenFileDialog();
                    if (!string.IsNullOrEmpty(OpenFileFilter))
                    {
                        dlg.Filter = OpenFileFilter;
                    }

                    if (dlg.ShowDialog() == true)
                    {
                        ConfigItemValue = dlg.FileName;
                    }
                }, true);
            }
            else
            {
                ButtonText = "\uF07C";
                ButtonCommand = new RelayCommand(() =>
                {
                    var dlg = new System.Windows.Forms.FolderBrowserDialog();
                    dlg.ShowNewFolderButton = true;
                    dlg.ShowDialog();
                    if (!String.IsNullOrEmpty(dlg.SelectedPath))
                    {
                        ConfigItemValue = dlg.SelectedPath;
                    }
                }, true);
            }
        }

        private bool _isFileSelector;
        public bool IsFileSelector
        {
            get { return _isFileSelector; }
            set
            {
                _isFileSelector = value;
                SetMode();
            }
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

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public RelayCommand ButtonCommand
        {
            get { return (RelayCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public string OpenFileFilter { get; set; }
    }
}
