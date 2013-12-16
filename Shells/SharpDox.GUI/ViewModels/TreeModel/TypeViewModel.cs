using System.Collections.ObjectModel;
using SharpDox.Model.Repository;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class TypeViewModel : TreeViewItemViewModel
    {
        public TypeViewModel(SDType type, NamespaceViewModel parent, ObservableCollection<string> excludedIdentifiers)
            : base(type.Identifier, parent, excludedIdentifiers)
        {
            Text = type.NameWithTypeParam;
            Accessibility = type.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.Resources;component/Icons/Class_{0}.png", Accessibility);

            foreach (var sdEvent in type.Events)
            {
                Children.Add(new EventViewModel(sdEvent, this, excludedIdentifiers));
            }

            foreach (var sdField in type.Fields)
            {
                Children.Add(new FieldViewModel(sdField, this, excludedIdentifiers));
            }

            foreach (var sdMethod in type.Constructors)
            {
                Children.Add(new MethodViewModel(sdMethod, this, excludedIdentifiers));
            }

            foreach (var sdMethod in type.Methods)
            {
                Children.Add(new MethodViewModel(sdMethod, this, excludedIdentifiers));
            }

            foreach (var sdProperty in type.Properties)
            {
                Children.Add(new PropertyViewModel(sdProperty, this, excludedIdentifiers));
            }
        }
    }
}
