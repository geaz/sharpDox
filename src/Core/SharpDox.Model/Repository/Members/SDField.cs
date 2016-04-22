using SharpDox.Model.Documentation;
using System;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a field.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert ein Field.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDField : SDMemberBase
    {
        public SDField(string identifier)
        {
            Identifier = identifier;
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the returntype of the field.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Rückgabetyp des Fields.
        ///     </summary>
        /// </de>
        public SDTypeRef ReturnType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the field is constant or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieses Feld "konstant" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsConst { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the field is readonly or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieses Feld "readonly" ist oder nicht.
        ///     </summary>
        /// </de>
        public bool IsReadonly { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the constant value.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den konstanten Wert.
        ///     </summary>
        /// </de>
        public string ConstantValue { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the syntax of the field.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Fields.
        ///     </summary>
        /// </de>
        public override string Syntax
        {
            get
            {
                var desc = string.Empty;
                if (IsConst)
                    desc = "const";
                else if (IsReadonly)
                    desc = "readonly";

                return string.Join(" ", new string[] { Accessibility, desc, ReturnType.NameWithTypeArguments, Name });
            }
        }

        public override SDTemplate SyntaxTemplate
        {
            get
            {
                var desc = string.Empty;
                if (IsConst)
                    desc = "const";
                else if (IsReadonly)
                    desc = "readonly";

                return new SDTemplate(string.Join(" ", new string[] { Accessibility, desc, ReturnType.LinkedNameWithTypeArguments, Name }));
            }
        }
    }
}
