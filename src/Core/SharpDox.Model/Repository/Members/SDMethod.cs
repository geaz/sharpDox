using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SharpDox.Model.CallTree;
using SharpDox.Model.Documentation;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a method.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repr�sentiert eine Methode.
    ///     </summary>
    /// </de>
    [Serializable]
    [DebuggerDisplay("{Name}")]
    public class SDMethod : SDMemberBase
    {
        public SDMethod(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;

            TypeParameters = new SortedList<SDTypeParameter>();
            Parameters = new List<SDParameter>();
			Calls = new List<SDNode>();
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is a constructor or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dies ein Konstruktor ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsCtor { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is public or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "public" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsPublic { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is private or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "private" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsPrivate { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is protected or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "protected" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsProtected { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is sealed or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "sealed" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsSealed { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is abstract or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "abstract" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsAbstract { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method override another one.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode eine andere �berschreibt.
        ///     </summary>
        /// </de>
        public bool IsOverride { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is virtual or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "virtual" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsVirtual { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the method is static or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob diese Methode "static" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsStatic { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the namespace of the method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namensraum der Methode.
        ///     </summary>
        /// </de>
        public string Namespace { get; set; }
       
        /// <default>
        ///     <summary>
        ///     Gets or sets the returntype of the method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den R�ckgabetypen der Methode.
        ///     </summary>
        /// </de>
        public SDTypeRef ReturnType { get; set; }
    
        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all parameters.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Parameter.
        ///     </summary>
        /// </de>
        public List<SDParameter> Parameters { get; set; }

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
        public SortedList<SDTypeParameter> TypeParameters { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of methods which are called by this method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Methoden die von dieser aufgerufen werden.
        ///     </summary>
        /// </de>
		public List<SDNode> Calls { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets the signature of the method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Signatur der Methode.
        ///     </summary>
        /// </de>
        public string Signature
        {
            get
            {
                var typeParam = TypeParameters.Select(parameter => parameter.Name).ToList();
                var typeParamText = typeParam.Count != 0 ? "<" + string.Join(", ", typeParam) + ">" : "";
                var param = Parameters.Select(parameter => parameter.ParamType.NameWithTypeArguments + " " + parameter.Name).ToList();

                return Name + typeParamText + "(" + string.Join(", ", param) + ")";
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets the signature of the method.
        ///     Where project types are markdown links.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Signatur der Methode.
        ///     Projekttypen sind Markdown Links.
        ///     </summary>
        /// </de>
        public string LinkedSignature
        {
            get
            {
                var typeParam = TypeParameters.Select(parameter => parameter.Name).ToList();
                var typeParamText = typeParam.Count != 0 ? "<" + string.Join(", ", typeParam) + ">" : "";
                var param = Parameters.Select(parameter => parameter.ParamType.LinkedNameWithTypeArguments + " " + parameter.Name).ToList();

                return Name + typeParamText + "(" + string.Join(", ", param) + ")";
            }
        }

        /// <default>
        ///     <summary>
        ///     Gets the syntax of the method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax der Methode.
        ///     </summary>
        /// </de>
        public override string Syntax
        {
            get
            {
                var desc = IsAbstract ? "abstract" : string.Empty;
                desc = IsVirtual ? "virtual" : desc;
                desc = IsStatic ? "static" : desc;

                var syntaxItems = new string[] { Accessibility, desc, ReturnType.NameWithTypeArguments, Signature };
                syntaxItems = syntaxItems.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return string.Join(" ", syntaxItems);
            }
        }
        
        public override SDTemplate SyntaxTemplate
        {
            get
            {
                var desc = IsAbstract ? "abstract" : string.Empty;
                desc = IsVirtual ? "virtual" : desc;
                desc = IsStatic ? "static" : desc;

                var syntaxItems = new string[] { Accessibility, desc, ReturnType.LinkedNameWithTypeArguments, LinkedSignature };
                syntaxItems = syntaxItems.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return new SDTemplate(string.Join(" ", syntaxItems));
            }
        }  
    }
}
