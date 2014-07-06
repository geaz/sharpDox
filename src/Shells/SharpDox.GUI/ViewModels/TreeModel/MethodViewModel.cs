using SharpDox.Model.Repository.Members;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class MethodViewModel : TreeViewItemViewModel
    {
        public MethodViewModel(SDMethod method, TypeViewModel parent, ICoreConfigSection sharpDoxConfig)
            : base(method.Identifier, parent, sharpDoxConfig)
        {
            Text = method.Name;
            Accessibility = method.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Method_{0}.png", Accessibility);
        }
    }
}
