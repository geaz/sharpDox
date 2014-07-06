using SharpDox.Model.Repository.Members;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class PropertyViewModel : TreeViewItemViewModel
    {
        public PropertyViewModel(SDProperty property, TypeViewModel parent, ICoreConfigSection sharpDoxConfig)
            : base(property.Identifier, parent, sharpDoxConfig)
        {
            Text = property.Name;
            Accessibility = property.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Properties_{0}.png", Accessibility);
        }
    }
}
