using System;
using SharpDox.Build;
using SharpDox.GUI.Command;
using SharpDox.Sdk.Config;
using System.ComponentModel;
using SharpDox.GUI.ViewModels.TreeModel;
using System.Windows;
using System.Windows.Threading;
using SharpDox.Model;

namespace SharpDox.GUI.ViewModels
{
    internal class VisibilityViewModel : ViewModelBase
    {
        private readonly ICoreConfigSection _sharpDoxConfig;
        private readonly BuildController _buildController;
        private readonly Action _onCloseHandle;

        public VisibilityViewModel(ICoreConfigSection sharpDoxConfig, BuildController buildController, Action onCloseHandle)
        {
            _sharpDoxConfig = sharpDoxConfig;
            _buildController = buildController;
            _onCloseHandle = onCloseHandle;

            sharpDoxConfig.PropertyChanged += ConfigChanged;

            buildController.BuildMessenger.OnBuildCompleted += ParseCompleted;
            buildController.BuildMessenger.OnBuildFailed += ParseStopped;
        }

        private void ConfigChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "InputFile" && _sharpDoxConfig.InputFile != null)
            {
                RefreshTreeView();
            }
            else if (args.PropertyName == "InputFile" && _sharpDoxConfig.InputFile == null)
            {
                TreeView = new VisibilityItemList(_sharpDoxConfig);
                TreeLoaded = false;
            }
        }

        private void ParseCompleted(SDProject sdProject)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => {
                    TreeView = new VisibilityItemList(_sharpDoxConfig);

                    if (sdProject != null)
                    {
                        foreach (var repository in sdProject.Solutions)
                        {
                            /*foreach (var sdNamespace in repository.Value.GetAllNamespaces())
                            {
                                TreeView.Add(new NamespaceViewModel(sdNamespace, _sharpDoxConfig));
                                TreeLoaded = true;
                            }*/
                        }
                    }

                    IsTreeRefreshing = false;
                }));
        }

        private void ParseStopped()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    TreeView = new VisibilityItemList(_sharpDoxConfig);
                    IsTreeRefreshing = false;
                }));
        }

        private void RefreshTreeView()
        {
            IsTreeRefreshing = true;
            _buildController.StartParse(_sharpDoxConfig, true);     
        }

        private bool _treeLoaded = false;
        public bool TreeLoaded
        {
            get { return _treeLoaded; }
            set { _treeLoaded = value; OnPropertyChanged("TreeLoaded"); }
        }

        private bool _isTreeRefreshing;
        public bool IsTreeRefreshing
        {
            get { return _isTreeRefreshing; }
            set { _isTreeRefreshing = value; OnPropertyChanged("IsTreeRefreshing"); }
        }

        private VisibilityItemList _treeView;
        public VisibilityItemList TreeView
        {
            get { return _treeView; }
            set { _treeView = value; OnPropertyChanged("TreeView"); }
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

        private ParameterRelayCommand<string> _showHideCommand;
        public ParameterRelayCommand<string> ShowHideCommand
        {
            get
            {
                return _showHideCommand ?? new ParameterRelayCommand<string>(s => TreeView.SwitchAllByAccessibility(s), true);
            }
            set
            {
                _showHideCommand = value;
                OnPropertyChanged("ShowHideCommand");
            }
        }

        private RelayCommand _includeAllCommand;
        public RelayCommand IncludeAllCommand
        {
            get
            {
                return _includeAllCommand ?? new RelayCommand(() => TreeView.IncludeAll(), true);
            }
            set
            {
                _includeAllCommand = value;
                OnPropertyChanged("IncludeAllCommand");
            }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? new RelayCommand(RefreshTreeView, true);
            }
            set
            {
                _refreshCommand = value;
                OnPropertyChanged("RefreshCommand");
            }
        }
    }
}
