using System.Collections.Generic;
using System.Linq;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using SharpDox.UML.Class.Model;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramParser
    {
        public ClassDiagram CreateClassDiagram(SDType type, bool connectedDiagram = true)
        {
            var attribute = type.IsAbstract && type.Kind.ToLower() != "interface" ? "abstract" : string.Empty;
            attribute = type.IsStatic ? "static" : attribute;

            var classDiagram = new ClassDiagram(type.Identifier, type.Name, type.Kind, type.Accessibility, attribute);

            if (connectedDiagram)
            {
                ParseTypes(classDiagram.BaseTypes, type.BaseTypes);
                ParseTypes(classDiagram.ImplementedInterfaces, type.ImplementedInterfaces);
                ParseTypes(classDiagram.Uses, type.Uses);
                ParseTypes(classDiagram.UsedBy, type.UsedBy);
            }

            ParseFields(classDiagram, type.Fields);
            ParseProperties(classDiagram, type.Properties);
            ParseConstructors(classDiagram, type.Constructors);
            ParseMethods(classDiagram, type.Methods);
            ParseEvents(classDiagram, type.Events);

            return classDiagram;
        }

        private void ParseTypes(List<ClassDiagram> classDiagramList, IEnumerable<SDType> sdTypes)
        {
            foreach (var sdType in sdTypes)
            {
                classDiagramList.Add(CreateClassDiagram(sdType, false));
            }
        }

        private void ParseFields(ClassDiagram classDiagram, IEnumerable<SDField> fields)
        {
            foreach (var field in fields.OrderBy(o => o.Name))
            {
                classDiagram.FieldRows.Add(new ClassDiagramRow(field.Identifier, "Field", field.Accessibility, field.Name));
            }
        }

        private void ParseProperties(ClassDiagram classDiagram, IEnumerable<SDProperty> properties)
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

                classDiagram.PropertyRows.Add(new ClassDiagramRow(property.Identifier, "Properties", property.Accessibility, property.Name + getSet));
            }
        }

        private void ParseConstructors(ClassDiagram classDiagram, IEnumerable<SDMethod> constructors)
        {
            foreach (var constructor in constructors.OrderBy(o => o.Name))
            {
                classDiagram.ConstructorRows.Add(new ClassDiagramRow(constructor.Identifier, "Method", constructor.Accessibility, constructor.Name));
            }
        }

        private void ParseMethods(ClassDiagram classDiagram, IEnumerable<SDMethod> methods)
        {
            foreach (var method in methods.OrderBy(o => o.Name))
            {
                classDiagram.MethodRows.Add(new ClassDiagramRow(method.Identifier, "Method", method.Accessibility, method.Name));
            }
        }

        private void ParseEvents(ClassDiagram classDiagram, IEnumerable<SDEvent> events)
        {
            foreach (var ev in events.OrderBy(o => o.Name))
            {
                classDiagram.EventRows.Add(new ClassDiagramRow(ev.Identifier, "Event", ev.Accessibility, ev.Name));
            }
        }
    }
}
