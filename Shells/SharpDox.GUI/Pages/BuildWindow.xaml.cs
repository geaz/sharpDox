using System;
using System.Windows;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Local;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI.Pages
{
    public partial class BuildWindow : IPage
    {
        private readonly IBuildController _buildController;
        private readonly IBuildMessenger _buildMessenger;
        private readonly SharpDoxConfig _sharpDoxConfig;

        public BuildWindow(SDGuiStrings strings, SharpDoxConfig sharpDoxConfig, IBuildController buildController, IBuildMessenger buildMessenger)
        {
            Strings = strings;

            _sharpDoxConfig = sharpDoxConfig;
            _buildController = buildController;
            _buildMessenger = buildMessenger;
            _buildMessenger.OnBuildMessage += BuilderOnMessage;
            _buildMessenger.OnStepMessage += BuilderOnStepMessage;
            _buildMessenger.OnBuildStopped += BuilderOnStopped;
            _buildMessenger.OnBuildProgress += BuilderOnBuildProgress;
            _buildMessenger.OnStepProgress += BuilderOnStepProgress;

            InitializeComponent();
        }

        private void BuilderOnStopped()
        {
            Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                    () =>
                        {
                            btnStart.IsEnabled = true;
                            btnStop.IsEnabled = false;
                        }
                    ));
        }

        private void BuilderOnMessage(string message)
        {
            Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action( () => WriteMessage(message))
            );
        }

        private void BuilderOnStepMessage(string message)
        {
            Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() => lblStepMessage.Content = message)
            );
        }

        private void BuilderOnBuildProgress(int progressValue)
        {
            Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                    {
                        pbBuildProgress.Value = progressValue;
                        lblProgress.Content = progressValue + " %";
                    })
            );
        }

        private void BuilderOnStepProgress(int progressValue)
        {
            Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() => pbStepProgress.Value = progressValue)
            );
        }

        private void BtnStartClick(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            tbConsoleOut.Clear();

            _buildController.StartBuild(_sharpDoxConfig, true);
        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {
            _buildController.Stop();
        }

        private void WriteMessage(string message)
        {
            tbConsoleOut.Text += message + Environment.NewLine;
            tbConsoleOut.ScrollToEnd();
        }

        public SDGuiStrings Strings { get; set; }
        public string PageName { get { return Strings.Build; } }
        public new int Width { get { return int.Parse(mainGrid.Width.ToString()); } }
        public new int Height { get { return int.Parse(mainGrid.Height.ToString()); } }
        public int Position { get { return 99; } }

        public Shell Shell
        {
            set 
            {
                value.OnClose += () => 
                    {
                        _buildMessenger.OnBuildMessage -= BuilderOnMessage;
                        _buildMessenger.OnStepMessage -= BuilderOnStepMessage;
                        _buildMessenger.OnBuildStopped -= BuilderOnStopped;
                        _buildMessenger.OnBuildProgress -= BuilderOnBuildProgress;
                        _buildMessenger.OnStepProgress -= BuilderOnStepProgress;

                        if (_buildController != null) _buildController.Stop();
                    }; 
            }
        }
    }
}
