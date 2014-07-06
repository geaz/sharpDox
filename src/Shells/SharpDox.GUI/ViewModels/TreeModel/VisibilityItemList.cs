using System.Collections.ObjectModel;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class VisibilityItemList : ObservableCollection<TreeViewItemViewModel>
    {
        private readonly ICoreConfigSection _sharpDoxConfig;

        public VisibilityItemList(ICoreConfigSection sharpDoxConfig)
        {
            _sharpDoxConfig = sharpDoxConfig;
        }

        public void IncludeAll()
        {
            foreach (var item in Items)
            {
                IncludeItem(item);
            }

            _sharpDoxConfig.ExcludedIdentifiers.Clear();
            _sharpDoxConfig.ExcludePrivate = false;
            _sharpDoxConfig.ExcludeProtected = false;
            _sharpDoxConfig.ExcludeInternal = false;
        }

        public void SwitchAllByAccessibility(string accessibility)
        {
            foreach (var item in Items)
            {
                SwitchItemByAccessibility(item, accessibility);
            }
        }

        private void SwitchItemByAccessibility(TreeViewItemViewModel item, string accessibility)
        {
            if (item.Accessibility != null && item.Accessibility.ToLower() == accessibility.ToLower())
            {
                item.IsExcluded = !item.IsExcluded;
            }
            else
            {
                foreach (var child in item.Children)
                {
                    SwitchItemByAccessibility(child, accessibility);
                }
            }
        }

        private void IncludeItem(TreeViewItemViewModel item)
        {
            item.IsExcluded = false;
            foreach (var child in item.Children)
            {
                IncludeItem(child);
            }
        }
    }
}
