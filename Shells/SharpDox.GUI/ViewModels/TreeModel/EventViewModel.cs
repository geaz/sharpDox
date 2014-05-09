using System.Collections.ObjectModel;
using SharpDox.Model.Repository.Members;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class EventViewModel : TreeViewItemViewModel
    {
        public EventViewModel(SDEvent eve, TypeViewModel parent, ObservableCollection<string> excludedIdentifiers)
            : base(eve.Identifier, parent, excludedIdentifiers)
        {
            Text = eve.Name;
            Accessibility = eve.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Event_{0}.png", Accessibility);
        }
    }
}
