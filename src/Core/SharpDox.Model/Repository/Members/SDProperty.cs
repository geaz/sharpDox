using SharpDox.Model.Documentation;
using System;
using System.Linq;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a property.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Eigenschaft.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDProperty : SDMemberBase
    {
        public SDProperty(string identifier)
        {
            Identifier = identifier;
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the type of the property.
        ///     </summary>
        /// </default>
        /// <de>
        ///     Setzt oder liefert den Typen der Eigenschaft.
        /// </de>
        public SDType ReturnType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the property can be get.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,
        ///     ob die Eigenschaft einen Wert zurückliefert.
        ///     </summary>
        /// </de>
        public bool CanGet { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the property can be set.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,
        ///     ob die Eigenschaft gesetzt werden kann.
        ///     </summary>
        /// </de>
        public bool CanSet { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the method is abstract or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,
        ///     ob die Eigenschaft abstrakt ist.
        ///     </summary>
        /// </de>
        public bool IsAbstract { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the method overrides another one.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,
        ///     ob die Eigenschaft eine andere überschreibt.
        ///     </summary>
        /// </de>
        public bool IsOverride { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the method is virtual or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,
        ///     ob die Eigenschaft virtuell ist.
        ///     </summary>
        /// </de>
        public bool IsVirtual { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets the syntax of the property.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax der Eigenschaft zurück.
        ///     </summary>
        /// </de>
        public override string Syntax
        {
            get
            {
                var desc = IsAbstract ? "abstract" : string.Empty;
                desc = IsOverride ? "override" : desc;
                desc = IsVirtual ? "virtual" : desc;

                var getSet = "";
                if (CanGet && CanSet)
                    getSet = "{ get; set; }";
                else if (CanGet)
                    getSet = "{ get; }";
                else if (CanSet)
                    getSet = "{ set; }";

                var syntax = new string[] { Accessibility, desc, ReturnType.NameWithTypeArguments, Name, getSet };
                syntax = syntax.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return string.Join(" ", syntax);
            }
        }

        public override SDTemplate SyntaxTemplate
        {
            get
            {
                var desc = IsAbstract ? "abstract" : string.Empty;
                desc = IsOverride ? "override" : desc;
                desc = IsVirtual ? "virtual" : desc;

                var getSet = "";
                if (CanGet && CanSet)
                    getSet = "{ get; set; }";
                else if (CanGet)
                    getSet = "{ get; }";
                else if (CanSet)
                    getSet = "{ set; }";

                var syntax = new string[] { Accessibility, desc, ReturnType.LinkedNameWithArguments, Name, getSet };
                syntax = syntax.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                return new SDTemplate(string.Join(" ", syntax));
            }
        }
    }
}
