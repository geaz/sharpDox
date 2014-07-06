using SharpDox.Model.Repository;
using SharpDox.Sdk.Config;

namespace SharpDox.GUI.ViewModels.TreeModel
{
    internal class TypeViewModel : TreeViewItemViewModel
    {
        public TypeViewModel(SDType type, NamespaceViewModel parent, ICoreConfigSection sharpDoxConfig)
            : base(type.Identifier, parent, sharpDoxConfig)
        {
            Text = type.NameWithTypeArguments;
            Accessibility = type.Accessibility;
            Image = string.Format("pack://application:,,,/SharpDox.GUI;component/Resources/Icons/Class_{0}.png", Accessibility);

            foreach (var sdEvent in type.Events)
            {
                Children.Add(new EventViewModel(sdEvent, this, sharpDoxConfig));
            }

            foreach (var sdField in type.Fields)
            {
                Children.Add(new FieldViewModel(sdField, this, sharpDoxConfig));
            }

            foreach (var sdMethod in type.Constructors)
            {
                Children.Add(new MethodViewModel(sdMethod, this, sharpDoxConfig));
            }

            foreach (var sdMethod in type.Methods)
            {
                Children.Add(new MethodViewModel(sdMethod, this, sharpDoxConfig));
            }

            foreach (var sdProperty in type.Properties)
            {
                Children.Add(new PropertyViewModel(sdProperty, this, sharpDoxConfig));
            }
        }
    }
}
