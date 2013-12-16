using System.Collections.ObjectModel;
using SharpDox.Model.Repository.Members;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class MethodViewModel : TreeViewItemViewModel
    {
        public MethodViewModel(SDMethod method, TypeViewModel parent, ObservableCollection<string> excludedIdentifiers)
            : base(method.Identifier, parent, excludedIdentifiers)
        {
            Text = method.Name;
            Accessibility = method.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.Resources;component/Icons/Method_{0}.png", Accessibility);
        }
    }
}
