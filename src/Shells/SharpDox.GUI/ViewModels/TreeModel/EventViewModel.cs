using SharpDox.Model.Repository.Members;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class EventViewModel : TreeViewItemViewModel
    {
        public EventViewModel(SDEvent eve, TypeViewModel parent, ICoreConfigSection sharpDoxConfig)
            : base(eve.Identifier, parent, sharpDoxConfig)
        {
            Text = eve.Name;
            Accessibility = eve.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Event_{0}.png", Accessibility);
        }
    }
}
