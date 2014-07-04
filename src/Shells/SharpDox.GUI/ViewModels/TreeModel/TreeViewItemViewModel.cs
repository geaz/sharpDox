using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    public class TreeViewItemViewModel : INotifyPropertyChanged
    {
        private bool _isHasExcludedChildRunning = false;

        private readonly ObservableCollection<string> _excludedIdentifiers;

        public TreeViewItemViewModel(string identifier, TreeViewItemViewModel parent, ObservableCollection<string> excludedIdentifiers)
        {
            _excludedIdentifiers = excludedIdentifiers;

            Children = new ObservableCollection<TreeViewItemViewModel>();
            Parent = parent;
            Identifier = identifier;
        }

        public void UpdateHasExcludedChild()
        {
            OnPropertyChanged("HasExcludedChild");
        }

        public string Text { get; set; }
        public string Accessibility { get; set; }
        public string Image { get; set; }
        public TreeViewItemViewModel Parent { get; private set; }
        public ObservableCollection<TreeViewItemViewModel> Children { get; private set; }

        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
            private set { _identifier = value; IsExcluded = _excludedIdentifiers.Contains(value); }
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
                if (Children.Count != 0)
                {
                    _isHasExcludedChildRunning = true;

                    var isExcluded = Children.All(c => c.IsExcluded);
                    if (IsExcluded != isExcluded)
                    {
                        IsExcluded = isExcluded;
                    }

                    _isHasExcludedChildRunning = false;
                }

                return Children.Any(c => c.IsExcluded);
            }
        }

        private bool _isExcluded;
        public bool IsExcluded
        {
            get { return _isExcluded; }
            set
            {
                _isExcluded = value;

                if (value && !_excludedIdentifiers.Contains(Identifier))
                {
                    _excludedIdentifiers.Add(Identifier);
                }
                else if(!value && _excludedIdentifiers.Contains(Identifier))
                {
                    _excludedIdentifiers.Remove(Identifier);
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
