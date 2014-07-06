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
        }

        public void ExcludeAllByAccessibility(string accessibility)
        {
            foreach (var item in Items)
            {
                ExcludeItemByAccessibility(item, accessibility);
            }
        }

        private void ExcludeItemByAccessibility(TreeViewItemViewModel item, string accessibility)
        {
            if (item.Accessibility != null && item.Accessibility.ToLower() == accessibility.ToLower())
            {
                item.IsExcluded = true;
            }
            else
            {
                foreach (var child in item.Children)
                {
                    ExcludeItemByAccessibility(child, accessibility);
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
