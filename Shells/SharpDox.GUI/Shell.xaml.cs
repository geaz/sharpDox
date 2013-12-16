using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using SharpDox.GUI.Pages;
using SharpDox.GUI.ViewModels;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI
{
    public partial class Shell : IShell
    {
        public event Action OnClose;

        private Button _lastActive;

        private readonly IPage[] _allPages;

        public Shell(SDGuiStrings strings, SharpDoxConfig sharpDoxConfig, IConfigController configController, IPage[] allPages)
        {
            _allPages = allPages;

            DataContext = new ShellViewModel(strings, configController, sharpDoxConfig, ExecuteOnClose);
            Strings = strings;

            InitializeComponent();
            SetMainMenu();
            SetExportDropDown();

            MouseLeftButtonDown += (s, a) => DragMove();
        }

        public void Start(string[] args)
        {
            new Application();
            ShowDialog();
        }

        private void ExecuteOnClose()
        {
            if (OnClose != null) OnClose();
            Close();
        }

        private void PluginComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pluginComboBox.SelectedIndex != 0)
            {
                var page = _allPages.Single(o => o.PageName.Equals(pluginComboBox.SelectedValue));
                lblPageName.Content = page.PageName.ToUpper();
                AnimateShowPage(page);
            }
        }

        private void SetMainMenu()
        {
            AddNavButton(Strings.GeneralSettings, false);
            AddNavButton(Strings.ExportSettings, true);

            IPage buildPage = _allPages.Single(o => o.PageName == Strings.Build);
            ((BuildWindow)buildPage).Shell = this;
            AddNavButton(buildPage.PageName, false);
        }

        private void SetExportDropDown()
        {
            pluginComboBox.Items.Add(Strings.SelectExportSetting);
            pluginComboBox.Items.Add(Strings.VisibilitySettings);

            foreach (var page in _allPages.Where(p => p.PageName != Strings.VisibilitySettings && p.PageName != Strings.GeneralSettings && p.PageName != Strings.Build))
            {
                pluginComboBox.Items.Add(page.PageName);
            }
        }

        private void AddNavButton(string buttonText, bool opensSidebar)
        {
            var tmpButton = 
                new Button
                {
                    Content = buttonText,
                    Style = (Style)TryFindResource("PageButton"),
                    Tag = buttonText
                };

            if(opensSidebar)
            {
                tmpButton.Click += (s, a) => ShowSidebar(s);
                mainNav.Children.Add(tmpButton);
            }
            else
            {
                tmpButton.Click += (s, a) => ShowPage(s);
                mainNav.Children.Add(tmpButton);
            }
        }

        private void ShowSidebar(object sender)
        {
            HidePage();

            var btn = ((Button)sender);
            if (!Equals(btn, _lastActive))
            {
                btn.Style = (Style)TryFindResource("PageButtonActive");
                if (_lastActive != null) _lastActive.Style = (Style)TryFindResource("PageButton");
                _lastActive = btn;

                var da = new ThicknessAnimation
                {
                    From = bottomGrid.Margin,
                    To = new Thickness(10, 0, 10, 0),
                    Duration = new Duration(TimeSpan.FromMilliseconds(250))
                };

                Storyboard.SetTarget(da, bottomGrid);
                Storyboard.SetTargetProperty(da, new PropertyPath(MarginProperty));

                var storyboard = new Storyboard();
                storyboard.Children.Add(da);
                storyboard.Begin();
            }
        }

        private void HideSidebar()
        {
            var da = new ThicknessAnimation
            {
                From = bottomGrid.Margin,
                To = new Thickness(10, 0, 10, 50),
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(da, bottomGrid);
            Storyboard.SetTargetProperty(da, new PropertyPath(MarginProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(da);
            storyboard.Begin();

            pluginComboBox.SelectedIndex = 0;
        }

        private void HidePage()
        {
            var da = new DoubleAnimation
            {
                From = pageContainer.Width,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            var da2 = new DoubleAnimation
            {
                From = pageContainer.Height,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };
            
            Storyboard.SetTarget(da, pageContainer);
            Storyboard.SetTargetProperty(da, new PropertyPath(WidthProperty));
            Storyboard.SetTarget(da2, pageContainer);
            Storyboard.SetTargetProperty(da2, new PropertyPath(HeightProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(da2);
            storyboard.Children.Add(da);
            storyboard.Begin();
        }
        
        private void ShowPage(object sender)
        {
            var btn = ((Button) sender);
            if (!Equals(btn, _lastActive))
            {
                IPage page = _allPages.Single(o => o.PageName.Equals(btn.Tag.ToString()));
                lblPageName.Content = page.PageName.ToUpper();

                HideSidebar();

                btn.Style = (Style)TryFindResource("PageButtonActive");
                if (_lastActive != null) _lastActive.Style = (Style)TryFindResource("PageButton");
                _lastActive = btn;

                AnimateShowPage(page);
            }
        }

        private void AnimateShowPage(IPage page)
        {
            var daFadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };

            var da = new DoubleAnimation
            {
                From = pageContainer.Width,
                To = page.Width + 20,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            var da2 = new DoubleAnimation
            {
                From = pageContainer.Height,
                To = page.Height + 40 >= 280 ? page.Height + 40 : 280,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            var daFadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };

            var storyboard = new Storyboard();

            if (pagePanel.Children.Count > 1)
            {
                Storyboard.SetTarget(daFadeOut, pagePanel.Children[1]);
                Storyboard.SetTargetProperty(daFadeOut, new PropertyPath(OpacityProperty));
                storyboard.Children.Add(daFadeOut);
                pagePanel.Children.RemoveAt(1);
            }

            Storyboard.SetTarget(da, pageContainer);
            Storyboard.SetTargetProperty(da, new PropertyPath(WidthProperty));
            Storyboard.SetTarget(da2, pageContainer);
            Storyboard.SetTargetProperty(da2, new PropertyPath(HeightProperty));
            Storyboard.SetTarget(daFadeIn, (UserControl)page);
            Storyboard.SetTargetProperty(daFadeIn, new PropertyPath(OpacityProperty));

            pagePanel.Children.Add((UserControl)page);

            storyboard.Children.Add(da2);
            storyboard.Children.Add(da);
            storyboard.Children.Add(daFadeIn);
            storyboard.Begin();
        }

        public SDGuiStrings Strings { get; private set; }
        public bool IsGui { get { return true; } }
    }
}