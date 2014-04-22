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
    public class SDType
    {
        public SDType(string identifier, string name, SDNamespace sdNamespace)
        {
            Guid = Guid.NewGuid();
            Identifier = identifier;
            Name = name;
            Namespace = sdNamespace;

            BaseTypes = new List<SDType>();
            ImplementedInterfaces = new List<SDType>();
            UsedBy = new List<SDType>();
            Uses = new List<SDType>();

            TypeParameters = new List<SDTypeParameter>();
            TypeArguments = new List<SDType>();
			Fields = new List<SDField>();
            Constructors = new List<SDMethod>();
			Methods = new List<SDMethod>();
            Events = new List<SDEvent>();
			Properties = new List<SDProperty>();
            NestedTypes = new List<SDType>();
        }

        /// <default>
        ///     <summary>
        ///     Sorts all members of this type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Sortiert alle Mitglieder dieses Typen.
        ///     </summary>     
        /// </de>
        public void SortMembers()
        {
            Fields.Sort();
            Constructors.Sort();
            Methods.Sort();
            Events.Sort();
            Properties.Sort();
        }

        private string GetTypeArgumentText()
        {
            var typeParam = TypeArguments.Select(argument => argument.NameWithTypeArguments).ToList();
            return typeParam.Count != 0 ? "<" + string.Join(", ", typeParam) + ">" : "";
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
        public Dictionary<string, SDDocumentation> Documentation { get; set; }

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
            get { return _name; }
            set
            {
                _name = value;

                if (value == "Void" || value == "Object")
                {
                    _name = _name.ToLower();
                }
            }
        }

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
                var upperName = char.ToUpper(Name[0]) + Name.Substring(1);
                return upperName + GetTypeArgumentText();
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
        public string Fullname { get { return string.Format("{0}.{1}", Namespace.Fullname, NameWithTypeArguments); } }

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
        ///     Gets or sets a list of all type arguments.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Typ-Argumente.
        ///     </summary>     
        /// </de>
        public List<SDType> TypeArguments { get; private set; }

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
        public List<SDTypeParameter> TypeParameters { get; private set; }

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
        public List<SDType> BaseTypes { get; private set; }

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
        public List<SDType> ImplementedInterfaces { get; private set; }

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
        public List<SDType> UsedBy { get; set; }

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
        public List<SDType> Uses { get; set; }

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
		public List<SDField> Fields { get; private set; }

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
        public List<SDMethod> Constructors { get; private set; }

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
		public List<SDMethod> Methods { get; private set; }
        
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
        public List<SDEvent> Events { get; private set; }

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
		public List<SDProperty> Properties { get; private set; }

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
        public List<SDType> NestedTypes { get; private set; }

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

                var inheritedText = ImplementedInterfaces.Count > 0 ? string.Join(", ", ImplementedInterfaces.Select(i => i.Name).ToList()) : string.Empty;
                var baseText = BaseTypes.Count > 0
                                       ? BaseTypes.First().NameWithTypeArguments
                                       : string.Empty;

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
                        list.Add(typeParam.BaseClass.NameWithTypeArguments);
                    }
                    foreach (var interfaceConstraint in typeParam.Interfaces)
                    {
                        list.Add(interfaceConstraint.NameWithTypeArguments);
                    }

                    typeContraints.Append(string.Format("where {0} : {1} ", typeParam.Name, string.Join(", ", list)));
                }

                var syntax = new string[] { Accessibility.ToLower(), desc, Kind.ToLower(), NameWithTypeArguments + inheritedText, typeContraints.ToString() };
                syntax = syntax.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return string.Join(" ", syntax);
            }
        }
    }
}