using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDox.Model.Documentation;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     Represents a type.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Typen.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDType : IComparable<SDType>
    {
        public SDType(string identifier, string name, SDNamespace sdNamespace)
        {
            Guid = Guid.NewGuid();
            Identifier = identifier;
            Name = name;
            Namespace = sdNamespace;

            Documentations = new SDLanguageItemCollection<SDDocumentation>();

            BaseTypes = new SortedList<SDType>();
            ImplementedInterfaces = new SortedList<SDType>();
            UsedBy = new SortedList<SDType>();
            Uses = new SortedList<SDType>();

            TypeParameters = new SortedList<SDTypeParameter>();
            TypeArguments = new SortedList<SDType>();
            Fields = new SortedList<SDField>();
            Constructors = new SortedList<SDMethod>();
            Methods = new SortedList<SDMethod>();
            Events = new SortedList<SDEvent>();
            Properties = new SortedList<SDProperty>();
            NestedTypes = new SortedList<SDType>();
        }

        private string GetInheritText(bool linked)
        {
            var inheritedText = ImplementedInterfaces.Count > 0 ? 
                string.Join(", ", ImplementedInterfaces.Select(i => linked ? i.LinkedNameWithArguments : 
                i.NameWithTypeArguments).ToList()) : string.Empty;

            var baseText = string.Empty;
            if (BaseTypes.Any())
            {
                baseText = linked ? BaseTypes.First().LinkedNameWithArguments : BaseTypes.First().NameWithTypeArguments;
            }

            if (inheritedText != string.Empty && baseText != string.Empty)
            {
                inheritedText += ", " + baseText;
                inheritedText = " : " + inheritedText;
            }
            else if (inheritedText != string.Empty)
            {
                inheritedText = " : " + inheritedText;
            }
            else if (baseText != string.Empty)
            {
                inheritedText = " : " + baseText;
            }

            return inheritedText;
        }

        private string GetTypeConstraintText(bool linked)
        {
            var typeContraints = new StringBuilder();
            foreach (var typeParam in TypeParameters)
            {
                var list = new List<string>();
                if (typeParam.HasDefaultConstructorConstraint)
                {
                    list.Add("new()");
                }
                if (typeParam.HasReferenceTypeConstraint)
                {
                    list.Add("class");
                }
                if (typeParam.HasValueTypeConstraint)
                {
                    list.Add("struct");
                }
                if (typeParam.BaseClass != null)
                {
                    if (linked) list.Add(typeParam.BaseClass.LinkedNameWithArguments);
                    else list.Add(typeParam.BaseClass.NameWithTypeArguments);
                }
                foreach (var interfaceConstraint in typeParam.Interfaces)
                {
                    if (linked) list.Add(interfaceConstraint.LinkedNameWithArguments);
                    else list.Add(interfaceConstraint.NameWithTypeArguments);
                }

                typeContraints.Append(string.Format("where {0} : {1} ", typeParam.Name, string.Join(", ", list)));
            }
            return typeContraints.ToString();
        }

        /// <default>
        ///     <summary>
        ///     Gets the guid of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die GUID des Typen.
        ///     </summary>     
        /// </de>
        public Guid Guid { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the unique identifier of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den eindeutigen Identifikator des Typen.
        ///     </summary>     
        /// </de>
        public string Identifier { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the shortened unique identifier of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den gekürzten eindeutigen Identifikator des Mitglieds.
        ///     </summary>     
        /// </de>
        public string ShortIdentifier { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the documentation of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Dokumentation des Typen.
        ///     </summary>     
        /// </de>
        public SDLanguageItemCollection<SDDocumentation> Documentations { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the region in which the type is defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Region in der der Typ definiert ist.
        ///     </summary>     
        /// </de>
        public SDRegion Region { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the accessibility of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Zugriffsebene des Typen.
        ///     </summary>     
        /// </de>
        public string Accessibility { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is abstract or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ "abstract" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsAbstract { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is a reference or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ ein Referenztyp ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsReferenceType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is sealed or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ "sealed" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsSealed { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is shadowing or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ "shadowing" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsShadowing { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is static or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ "static" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsStatic { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is synthetic or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Typ "synthetic" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsSynthetic { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the type is a project member or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob der Typ zum Projekt gehört oder nicht.
        ///     </summary>     
        /// </de>
	    public bool IsProjectStranger { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Typen.
        ///     </summary>     
        /// </de>
        private string _name;
        public string Name
        {
            get
            {
                return CSharpName ?? _name;
            }
            set { _name = value; }
        }

        public string CSharpName { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets the name of the type. Including the type parameters.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Typen inklusive der Typ-Parameter.
        ///     </summary>     
        /// </de>
        public string NameWithTypeArguments
        {
            get
            {
                var typeParam = TypeArguments.Select(argument => argument.NameWithTypeArguments).ToList();
                var typeArguments = typeParam.Count != 0 ? "<" + string.Join(", ", typeParam) + ">" : "";

                return Name + typeArguments;
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets the name of the type. Including the type parameters.
        ///     Where project types are markdown links.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Typen inklusive der Typ-Parameter.
        ///     Projekttypen sind Markdown Links.
        ///     </summary>     
        /// </de>
        public string LinkedNameWithArguments
        {
            get
            {
                var linkedName = IsProjectStranger ? Name : string.Format("[{0}]({{{{type-link:{1}}}}})", Name, Fullname);
                if(ArrayElementType != null && !ArrayElementType.IsProjectStranger)
                {
                    linkedName = ArrayElementType.LinkedNameWithArguments + "[]";
                }

                var linkedTypeArguments =
                    TypeArguments.Count > 0 ?
                    string.Format("<{0}>", string.Join(", ", TypeArguments.Select(t => t.IsProjectStranger ? t.Name : string.Format("[{0}]({{{{type-link:{1}}}}})", t.Name, t.Fullname)))) :
                    string.Empty;


                return linkedName + linkedTypeArguments;
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the namespace of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namensraum des Typen.
        ///     </summary>     
        /// </de>
        public SDNamespace Namespace { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets the fullname of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den vollen Namen des Typen.
        ///     </summary>     
        /// </de>
        public string Fullname
        {
            get
            {
                var typeParam = TypeArguments.Select(argument => argument.NameWithTypeArguments).ToList();
                var typeArguments = typeParam.Count != 0 ? "<" + string.Join(", ", typeParam) + ">" : "";

                return string.Format("{0}.{1}", Namespace.Fullname, _name + typeArguments);
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the kind of the type (class, interface, ...).
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Art des Typen (class, interface, ...).
        ///     </summary>     
        /// </de>
        public string Kind { get; set; }

        /// <default>
        ///     <summary>
        ///     If the type is an array element, this property returns the underlying element type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Falls der Typ ein Arrayelement ist, liefert diese Property den zugrunde liegenden Typen.
        ///     </summary>     
        /// </de>>
        public SDType ArrayElementType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all type arguments.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Typ-Argumente.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> TypeArguments { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all type parameters.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Typ-Parameter.
        ///     </summary>     
        /// </de>
        public SortedList<SDTypeParameter> TypeParameters { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all basetypes.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Basistypen.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> BaseTypes { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all implemented interfaces.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller implementierter Interfaces.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> ImplementedInterfaces { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all types using this type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller <c>SDType</c>s die diesen Typen benutzen.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> UsedBy { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all types used by this type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller <c>SDType</c>s die von diesem Typen benutzt werden.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> Uses { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all fields.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller <c>SDField</c>s.
        ///     </summary>     
        /// </de>
        public SortedList<SDField> Fields { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all constructors.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Konstruktoren.
        ///     </summary>     
        /// </de>
        public SortedList<SDMethod> Constructors { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all methods.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Methoden.
        ///     </summary>     
        /// </de>
        public SortedList<SDMethod> Methods { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all events.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Events.
        ///     </summary>     
        /// </de>
        public SortedList<SDEvent> Events { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all properties.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Eigenschaften.
        ///     </summary>     
        /// </de>
        public SortedList<SDProperty> Properties { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all nested types.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller eingebetteter Typen.
        ///     </summary>     
        /// </de>
        public SortedList<SDType> NestedTypes { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the syntax of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Typen.
        ///     </summary>     
        /// </de>
        public string Syntax
        {
            get
            {
                var desc = IsAbstract && Kind.ToLower() != "interface" ? "abstract" : string.Empty;
                desc = IsStatic ? "static" : desc;                             
                            
                var syntax = new string[] { Accessibility.ToLower(), desc, Kind.ToLower(), NameWithTypeArguments + GetInheritText(false), GetTypeConstraintText(false) };
                syntax = syntax.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return string.Join(" ", syntax);
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets the syntax of the type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Typen.
        ///     </summary>     
        /// </de>
        public SDTemplate SyntaxTemplate
        {
            get
            {
                var desc = IsAbstract && Kind.ToLower() != "interface" ? "abstract" : string.Empty;
                desc = IsStatic ? "static" : desc;

                var syntax = new string[] { Accessibility.ToLower(), desc, Kind.ToLower(), LinkedNameWithArguments + GetInheritText(true), GetTypeConstraintText(true) };
                syntax = syntax.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return new SDTemplate(string.Join(" ", syntax));
            }
        }

        /// <default>
        ///     <summary>
        ///     Comparer to sort the a list with SDTypes.
        ///     </summary>
        ///     <param name="other">SDType to compare with</param>
        ///     <returns>A value indicating, if the current SDType is lower or greater then the given one.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Vergleichsmethode, um eine Liste mit <c>SDType</c>s zu sortieren.
        ///     </summary>
        ///     <param name="other">Ein anderer <c>SDType</c> mit dem dieser verglichen werden soll.</param>
        ///     <returns>Ein Wert der angibt, ob der aktuelle <c>SDType</c> "größer" oder "kleiner" als der angegebene ist.</returns>    
        /// </de>
        public int CompareTo(SDType other)
        {
            return Identifier.CompareTo(other.Identifier);
        }
    }
}