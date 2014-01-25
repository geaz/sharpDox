using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI.Pages
{
    public partial class GeneralSettings : IPage
    {
        public GeneralSettings(SDGuiStrings strings, SharpDoxConfig config)
		{
            DataContext = config;
            Strings = strings;

            InitializeComponent();
        }

        private void OnBrowseSolution(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Solution (*.sln)|*.sln";
            if (dlg.ShowDialog() == true)
            {
                var tb = ((TextBox)sender);
                tb.Text = dlg.FileName;
            }
        }

        private void OnBrowseLogo(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image File(.png; .jpg; .bmp)|*.png; *.jpg; *.bmp";
            if (dlg.ShowDialog() == true)
            {
                var tb = ((TextBox)sender);
                tb.Text = dlg.FileName;
                tbInputPath.Focus();
            }
        }

        private void OnBrowseFolder(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.ShowDialog();
            if (!String.IsNullOrEmpty(dlg.SelectedPath))
            {
                var tb = ((TextBox)sender);
                tb.Text = dlg.SelectedPath;
            }
        }

        public SDGuiStrings Strings { get; private set; }
        public List<LanguageItem> LanguageList { get { return new LanguageList(); } }
        public string PageName { get { return Strings.GeneralSettings; } }
        public new int Width { get { return int.Parse(mainGrid.Width.ToString()); } }
        public new int Height { get { return int.Parse(mainGrid.Height.ToString()); } }
    }
}
