using SharpDox.Model.Repository;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class NamespaceViewModel : TreeViewItemViewModel
    {
        public NamespaceViewModel(SDNamespace sdNamespace, ICoreConfigSection sharpDoxConfig)
            : base(sdNamespace.Fullname, null, sharpDoxConfig)
        {
            Text = sdNamespace.Fullname;
            Image = "pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Namespace_public.png";

            foreach (var sdType in sdNamespace.Types)
            {
                Children.Add(new TypeViewModel(sdType, this, sharpDoxConfig));
            }
        }
    }
}
