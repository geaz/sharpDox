using System.Collections.ObjectModel;
using SharpDox.Model.Repository.Members;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class PropertyViewModel : TreeViewItemViewModel
    {
        public PropertyViewModel(SDProperty property, TypeViewModel parent, ObservableCollection<string> excludedIdentifiers)
            : base(property.Identifier, parent, excludedIdentifiers)
        {
            Text = property.Name;
            Accessibility = property.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.Resources;component/Icons/Properties_{0}.png", Accessibility);
        }
    }
}
