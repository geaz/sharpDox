using System;
using System.Collections.Generic;
using Microsoft.Win32;
using SharpDox.GUI.Command;
using SharpDox.GUI.Windows;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;
using System.Linq;

namespace SharpDox.GUI.ViewModels
{
    internal class MenuItemViewModel
    {
        public string Text { get; set; }
        public RelayCommand Command { get; set; }
    }

    internal class ShellViewModel : ViewModelBase
    {
        private BuildView _buildWindow;

        private readonly IConfigController _configController;
        private readonly IBuildController _buildController;
        private readonly Action _onCloseHandle;

        public ShellViewModel(SDGuiStrings strings, IConfigController configController, IBuildController buildController, Action onCloseHandle)
        {
            _onCloseHandle = onCloseHandle;

            _buildController = buildController;
            _buildController.BuildMessenger.OnBuildProgress += (i) => { BuildProgress = i; };
            _buildController.BuildMessenger.OnStepProgress += (i) => { StepProgress = i; };            

            _configController = configController;
            _configController.OnRecentProjectsChanged += RecentProjectsChanged;

            Strings = strings;
            BuildButtonText = Strings.Build;

            Config = configController.GetConfigSection<ICoreConfigSection>();
            ConfigSections = configController.GetAllConfigSections().ToList();

            _buildWindow = new BuildView(Strings, _buildController.BuildMessenger);

            RecentProjectsChanged();
        }

        public SDGuiStrings Strings { get; private set; }
        public ICoreConfigSection Config { get; private set; }

        private void RecentProjectsChanged()
        {
            var recentProjects = new List<MenuItemViewModel>();
            foreach (var recentProject in _configController.RecentProjects)
            {
                recentProjects.Add(new MenuItemViewModel { Text = recentProject.Value, Command = new RelayCommand(() => { _configController.Load(recentProject.Key); }, true) });
            }
            RecentProjects = recentProjects;
        }

        private IEnumerable<IConfigSection> _configSections;
        public IEnumerable<IConfigSection> ConfigSections
        {
            get { return _configSections; }
            set { _configSections = value; OnPropertyChanged("ConfigSections"); }
        }

        private bool _isRecentProjectsVisible = false;
        public bool IsRecentProjectsVisible
        {
            get { return _isRecentProjectsVisible; }
            set { _isRecentProjectsVisible = value; OnPropertyChanged("IsRecentProjectsVisible"); }
        }

        private IEnumerable<MenuItemViewModel> _recentProjects;
        public IEnumerable<MenuItemViewModel> RecentProjects
        {
            get { return _recentProjects; }
            set { _recentProjects = value; IsRecentProjectsVisible = value.Count() > 0; OnPropertyChanged("RecentProjects"); }
        }

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? new RelayCommand(() => _onCloseHandle(), true);
            }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
            }
        }

        private RelayCommand _newConfigCommand;
        public RelayCommand NewConfigCommand
        {
            get
            {
                return _newConfigCommand ?? new RelayCommand(() => _configController.New(), true);
            }
            set
            {
                _newConfigCommand = value;
                OnPropertyChanged("NewConfigCommand");
            }
        }

        private RelayCommand _loadConfigCommand;
        public RelayCommand LoadConfigCommand
        {
            get
            {
                return _loadConfigCommand ?? new RelayCommand(() =>
                {
                    var dlg = new OpenFileDialog
                    {
                        DefaultExt = ".sdox",
                        Filter = "SharpDox File(.sdox)|*.sdox"
                    };

                    if (dlg.ShowDialog() == true)
                    {
                        _configController.Load(dlg.FileName);
                    }
                }, true);
            }
            set
            {
                _loadConfigCommand = value;
                OnPropertyChanged("LoadConfigCommand");
            }
        }

        private RelayCommand _saveConfigCommand;
        public RelayCommand SaveConfigCommand
        {
            get
            {
                return _saveConfigCommand ?? new RelayCommand(() =>
                {
                    if (!string.IsNullOrEmpty(Config.PathToConfig))
                    {
                        _configController.Save();
                    }
                    else
                    {
                        var dlg = new SaveFileDialog
                        {
                            DefaultExt = ".sdox",
                            Filter = "SharpDox File(.sdox)|*.sdox"
                        };

                        if (dlg.ShowDialog() == true)
                        {
                            _configController.SaveTo(dlg.FileName);
                        }
                    }
                }, true);
            }
            set
            {
                _saveConfigCommand = value;
                OnPropertyChanged("SaveConfigCommand");
            }
        }

        private RelayCommand _saveToConfigCommand;
        public RelayCommand SaveToConfigCommand
        {
            get
            {
                return _saveToConfigCommand ?? new RelayCommand(() =>
                {
                    var dlg = new SaveFileDialog
                    {
                        DefaultExt = ".sdox",
                        Filter = "SharpDox File(.sdox)|*.sdox"
                    };

                    if (dlg.ShowDialog() == true)
                    {
                        _configController.SaveTo(dlg.FileName);
                    }
                }, true);
            }
            set
            {
                _saveToConfigCommand = value;
                OnPropertyChanged("SaveToConfigCommand");
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

        private RelayCommand _buildButtonCommand;
        public RelayCommand BuildButtonCommand
        {
            get
            {
                return _buildButtonCommand ?? new RelayCommand(() =>
                {
                    if (BuildButtonText == Strings.Build)
                    {
                        BuildButtonText = Strings.Abort;
                        _buildController.StartBuild(_configController.GetConfigSection<ICoreConfigSection>(), true);
                    }
                    else
                    {
                        BuildButtonText = Strings.Build;
                        _buildController.Stop();
                    }
                }, true);
            }
            set
            {
                _buildButtonCommand = value;
                OnPropertyChanged("BuildButtonCommand");
            }
        }

        private RelayCommand _openBuildWindowCommand;
        public RelayCommand OpenBuildWindowCommand
        {
            get
            {
                return _openBuildWindowCommand ?? new RelayCommand(() =>
                {
                    _buildWindow.Show();
                }, true);
            }
            set
            {
                _openBuildWindowCommand = value;
                OnPropertyChanged("OpenBuildWindowCommand");
            }
        }
    }
}
