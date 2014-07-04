using System.Collections.ObjectModel;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class VisibilityItemList : ObservableCollection<TreeViewItemViewModel>
    {
        public void IncludeAll()
        {
            foreach (var item in Items)
            {
                IncludeItem(item);
            }
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
