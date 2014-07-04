using System.Collections.ObjectModel;
using SharpDox.Model.Repository;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class NamespaceViewModel : TreeViewItemViewModel
    {
        public NamespaceViewModel(SDNamespace sdNamespace, ObservableCollection<string> excludedIdentifiers)
            : base(sdNamespace.Fullname, null, excludedIdentifiers)
        {
            Text = sdNamespace.Fullname;
            Image = "pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Namespace_public.png";

            foreach (var sdType in sdNamespace.Types)
            {
                Children.Add(new TypeViewModel(sdType, this, excludedIdentifiers));
            }
        }
    }
}
