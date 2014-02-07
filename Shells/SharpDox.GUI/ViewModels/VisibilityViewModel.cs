using System;
using SharpDox.GUI.Command;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Build;
using System.ComponentModel;
using SharpDox.Model.Repository;
using SharpDox.GUI.ViewModels.TreeModel;
using System.Windows;
using System.Windows.Threading;

namespace SharpDox.GUI.ViewModels
{
    internal class VisibilityViewModel : ViewModelBase
    {
        private readonly ICoreConfigSection _sharpDoxConfig;
        private readonly IBuildController _buildController;

        public VisibilityViewModel(ICoreConfigSection sharpDoxConfig, IBuildController buildController, IBuildMessenger buildMessenger)
        {
            _sharpDoxConfig = sharpDoxConfig;
            _buildController = buildController;

            sharpDoxConfig.PropertyChanged += ConfigChanged;
            buildMessenger.OnParseCompleted += ParseCompleted;
        }

        private void ConfigChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "InputPath" && !string.IsNullOrEmpty(_sharpDoxConfig.InputPath))
            {
                RefreshTreeView();
            }
            else if (args.PropertyName == "InputPath" && string.IsNullOrEmpty(_sharpDoxConfig.InputPath))
            {
                TreeView = new VisibilityItemList();
            }
        }

        private void ParseCompleted(SDRepository repository)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => {
                    TreeView = new VisibilityItemList();

                    if (repository != null)
                    {
                        foreach (var sdNamespace in repository.GetAllNamespaces())
                        {
                            TreeView.Add(new NamespaceViewModel(sdNamespace,
                                _sharpDoxConfig.ExcludedIdentifiers));
                        }
                    }

                    IsTreeRefreshing = false;
                }));
        }

        private void RefreshTreeView()
        {
            IsTreeRefreshing = true;
            _buildController.StartParse(_sharpDoxConfig, true);     
        }

        private bool _isTreeRefreshing;
        public bool IsTreeRefreshing
        {
            get { return _isTreeRefreshing; }
            set { _isTreeRefreshing = value; OnPropertyChanged("IsTreeRefreshing"); }
        }

        private VisibilityItemList _treeView = new VisibilityItemList();
        public VisibilityItemList TreeView
        {
            get { return _treeView; }
            set { _treeView = value; OnPropertyChanged("TreeView"); }
        }

        private ParameterRelayCommand<string> _showHideCommand;
        public ParameterRelayCommand<string> ShowHideCommand
        {
            get
            {
                return _showHideCommand ?? new ParameterRelayCommand<string>(s => TreeView.ExcludeAllByAccessibility(s), true);
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
