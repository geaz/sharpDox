using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    public class TreeViewItemViewModel : INotifyPropertyChanged
    {
        private bool _isHasExcludedChildRunning = false;

        private readonly ICoreConfigSection _sharpDoxConfig;

        public TreeViewItemViewModel(string identifier, TreeViewItemViewModel parent, ICoreConfigSection sharpDoxConfig)
        {
            _sharpDoxConfig = sharpDoxConfig;

            Children = new ObservableCollection<TreeViewItemViewModel>();
            Children.CollectionChanged += (s, a) => UpdateHasExcludedChild();

            Parent = parent;
            Identifier = identifier;
            IsExcluded = _sharpDoxConfig.ExcludedIdentifiers.Contains(identifier);
        }

        public void UpdateHasExcludedChild()
        {
            //if all children are excluded, the parent is excluded too
            if (Children.Count != 0)
            {
                _isHasExcludedChildRunning = true;
                IsExcluded = Children.All(c => c.IsExcluded);
                _isHasExcludedChildRunning = false;
            }

            OnPropertyChanged("HasExcludedChild");
            if (Parent != null)
                Parent.UpdateHasExcludedChild();
        }

        public string Text { get; set; }
        public string Image { get; set; }
        public string Identifier { get; private set; }
        public TreeViewItemViewModel Parent { get; private set; }
        public ObservableCollection<TreeViewItemViewModel> Children { get; private set; }

        private string _accessibility;
        public string Accessibility
        {
            get { return _accessibility; }
            set
            {
                _accessibility = value;
                if (value.ToLower() == "private" && _sharpDoxConfig.ExcludePrivate)
                {
                    IsExcluded = true;
                }
                if (value.ToLower() == "protected" && _sharpDoxConfig.ExcludeProtected)
                {
                    IsExcluded = true;
                }
                if (value.ToLower() == "internal" && _sharpDoxConfig.ExcludeInternal)
                {
                    IsExcluded = true;
                }
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");

                if (_isExpanded && Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        public bool HasExcludedChild
        {
            get
            {
                return Children.Any(c => c.IsExcluded || c.HasExcludedChild);
            }
        }

        private bool _isExcluded;
        public bool IsExcluded
        {
            get { return _isExcluded; }
            set
            {
                if (value == _isExcluded)
                    return;

                _isExcluded = value;

                if (value && !_sharpDoxConfig.ExcludedIdentifiers.Contains(Identifier))
                {
                    _sharpDoxConfig.ExcludedIdentifiers.Add(Identifier);
                }
                else if (!value && _sharpDoxConfig.ExcludedIdentifiers.Contains(Identifier))
                {
                    _sharpDoxConfig.ExcludedIdentifiers.Remove(Identifier);
                }
                
                if (Parent != null)
                {
                    Parent.UpdateHasExcludedChild();
                }

                if (!_isHasExcludedChildRunning)
                {
                    foreach (var child in Children) child.IsExcluded = value;
                }

                OnPropertyChanged("IsExcluded");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
