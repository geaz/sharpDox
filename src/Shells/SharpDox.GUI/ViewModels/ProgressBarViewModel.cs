using SharpDox.GUI.Windows;
using SharpDox.Sdk.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SharpDox.GUI.ViewModels
{
    internal enum BuildStatus
    {
        Success,
        Error,
        Running,
        Stopped
    }

    internal class ProgressBarViewModel : ViewModelBase
    {
        private readonly SDGuiStrings _strings;

        public ProgressBarViewModel(IBuildController buildController, SDGuiStrings strings)
        {
            _strings = strings;
            BuildButtonText = strings.Build;

            buildController.BuildMessenger.OnBuildProgress += (i) => { if (i == 0) ChangeProgress(BuildStatus.Running); BuildProgress = i; };
            buildController.BuildMessenger.OnStepProgress += (i) => { StepProgress = i; };

            buildController.BuildMessenger.OnBuildCompleted += (p) => ChangeProgress(BuildStatus.Success);
            buildController.BuildMessenger.OnBuildFailed += () => ChangeProgress(BuildStatus.Error);
            buildController.BuildMessenger.OnBuildStopped += () => ChangeProgress(BuildStatus.Stopped);
        }

        private void ChangeProgress(BuildStatus status)
        {
            var color = Color.FromArgb(255, 47, 201, 31); 
            var buildButtonText = _strings.Build;
            var progressIndicator = string.Empty;
            var staticIndicatorVisible = false;

            if(status == BuildStatus.Error)
            {
                color = Color.FromArgb(255, 241, 37, 47);
                progressIndicator = "\uF071";
                staticIndicatorVisible = true;
            }
            else if (status == BuildStatus.Running)
            {
                color = Color.FromArgb(255, 38, 156, 245); 
                buildButtonText = _strings.Abort;
                progressIndicator = "\uF021";
            }
            else if (status == BuildStatus.Success)
            {
                progressIndicator = "\uF14A";
                staticIndicatorVisible = true;
            }

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    BuildButtonText = buildButtonText;
                    ProgressColor = new SolidColorBrush(color);
                    ProgressIndicator = progressIndicator;
                    StaticIndicatorVisible = staticIndicatorVisible;
                    DynamicIndicatorVisible = !staticIndicatorVisible;
                }));
        }

        private string _buildButtonText;
        public string BuildButtonText
        {
            get { return _buildButtonText; }
            set
            {
                _buildButtonText = value;
                OnPropertyChanged("BuildButtonText");
            }
        }

        private string _progressIndicator;
        public string ProgressIndicator
        {
            get { return _progressIndicator; }
            set
            {
                _progressIndicator = value;
                OnPropertyChanged("ProgressIndicator");
            }
        }

        private bool _staticIndicatorVisible;
        public bool StaticIndicatorVisible
        {
            get { return _staticIndicatorVisible; }
            set
            {
                _staticIndicatorVisible = value;
                OnPropertyChanged("StaticIndicatorVisible");
            }
        }

        private bool _dynamicIndicatorVisible;
        public bool DynamicIndicatorVisible
        {
            get { return _dynamicIndicatorVisible; }
            set
            {
                _dynamicIndicatorVisible = value;
                OnPropertyChanged("DynamicIndicatorVisible");
            }
        }

        private SolidColorBrush _progressColor;
        public SolidColorBrush ProgressColor
        {
            get { return _progressColor; }
            set
            {
                _progressColor = value;
                OnPropertyChanged("ProgressColor");
            }
        }

        private int _buildProgress;
        public int BuildProgress
        {
            get { return _buildProgress; }
            set
            {
                _buildProgress = value;
                OnPropertyChanged("BuildProgress");
            }
        }

        private int _stepProgress;
        public int StepProgress
        {
            get { return _stepProgress; }
            set
            {
                _stepProgress = value;
                OnPropertyChanged("StepProgress");
            }
        }
    }
}
