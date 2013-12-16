using System.Collections.ObjectModel;
using SharpDox.Model.Repository.Members;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class FieldViewModel : TreeViewItemViewModel
    {
        public FieldViewModel(SDField field, TypeViewModel parent, ObservableCollection<string> excludedIdentifiers)
            : base(field.Identifier, parent, excludedIdentifiers)
        {
            Text = field.Name;
            Accessibility = field.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.Resources;component/Icons/Field_{0}.png", Accessibility);
        }
    }
}
