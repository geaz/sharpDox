using System.Collections.Generic;
using System.Linq;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using SharpDox.UML.Class.Model;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramParser
    {
        private ClassDiagram _classDiagram;

        public ClassDiagram CreateClassDiagram(SDType type)
        {
            var attribute = type.IsAbstract && type.Kind.ToLower() != "interface" ? "abstract" : string.Empty;
            attribute = type.IsStatic ? "static" : attribute;

            _classDiagram = new ClassDiagram(type.Identifier, type.Name, type.Kind, type.Accessibility, attribute);

            ParseFields(type.Fields);
            ParseProperties(type.Properties);
            ParseConstructors(type.Constructors);
            ParseMethods(type.Methods);
            ParseEvents(type.Events);

            return _classDiagram;
        }

        private void ParseFields(IEnumerable<SDField> fields)
        {
            foreach (var field in fields.OrderBy(o => o.Name))
            {
                _classDiagram.FieldRows.Add(new ClassDiagramRow(field.Identifier, "Field", field.Accessibility, field.Name));
            }
        }

        private void ParseProperties(IEnumerable<SDProperty> properties)
        {
            foreach (var property in properties.OrderBy(o => o.Name))
            {
                var getSet = "";
                if (property.CanGet && property.CanSet)
                    getSet = " { get; set; }";
                else if (property.CanGet)
                    getSet = " { get; }";
                else if (property.CanSet)
                    getSet = " { set; }";

                _classDiagram.PropertyRows.Add(new ClassDiagramRow(property.Identifier, "Properties", property.Accessibility, property.Name + getSet));
            }
        }

        private void ParseConstructors(IEnumerable<SDMethod> constructors)
        {
            foreach (var constructor in constructors.OrderBy(o => o.Name))
            {
                _classDiagram.ConstructorRows.Add(new ClassDiagramRow(constructor.Identifier, "Method", constructor.Accessibility, constructor.Name));
            }
        }

        private void ParseMethods(IEnumerable<SDMethod> methods)
        {
            foreach (var method in methods.OrderBy(o => o.Name))
            {
                _classDiagram.MethodRows.Add(new ClassDiagramRow(method.Identifier, "Method", method.Accessibility, method.Name));
            }
        }

        private void ParseEvents(IEnumerable<SDEvent> events)
        {
            foreach (var ev in events.OrderBy(o => o.Name))
            {
                _classDiagram.EventRows.Add(new ClassDiagramRow(ev.Identifier, "Event", ev.Accessibility, ev.Name));
            }
        }
    }
}
