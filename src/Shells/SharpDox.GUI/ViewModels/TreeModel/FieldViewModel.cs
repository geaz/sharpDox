using SharpDox.Model.Repository.Members;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class FieldViewModel : TreeViewItemViewModel
    {
        public FieldViewModel(SDField field, TypeViewModel parent, ICoreConfigSection sharpDoxConfig)
            : base(field.Identifier, parent, sharpDoxConfig)
        {
            Text = field.Name;
            Accessibility = field.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Field_{0}.png", Accessibility);
        }
    }
}
