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
    public class SDField : SDMember
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
        public SDType ReturnType { get; set; }

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
                return string.Join(" ", new string[] { Accessibility, ReturnType.NameWithTypeArguments, Name });
            }
        }
    }
}
