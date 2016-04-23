using System;

namespace SharpDox.Model.Repository
{
    public class SDTypeRef : IComparable<SDTypeRef>
    {
        public SDType Type { get; set; }
        public bool IsPointerType { get; set; }
        public bool IsArrayType { get; set; }

        public string NameWithTypeArguments
        {
            get
            {
                var name = Type.NameWithTypeArguments;
                name += IsPointerType ? "*" : string.Empty;
                name += IsArrayType ? "[]" : string.Empty;
                return name;
            }
        }

        public string LinkedNameWithTypeArguments
        {
            get
            {
                var name = Type.LinkedNameWithTypeArguments;
                name += IsPointerType ? "*" : string.Empty;
                name += IsArrayType ? "[]" : string.Empty;
                return name;
            }
        }

        /// <default>
        ///     <summary>
        ///     Comparer to sort the a list with SDTypeRef.
        ///     </summary>
        ///     <param name="other">SDTypeRef to compare with</param>
        ///     <returns>A value indicating, if the current SDTypeRef is lower or greater then the given one.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Vergleichsmethode, um eine Liste mit <c>SDTypeRef</c>s zu sortieren.
        ///     </summary>
        ///     <param name="other">Ein anderer <c>SDTypeRef</c> mit dem dieser verglichen werden soll.</param>
        ///     <returns>Ein Wert der angibt, ob der aktuelle <c>SDTypeRef</c> "größer" oder "kleiner" als der angegebene ist.</returns>    
        /// </de>
        public int CompareTo(SDTypeRef other)
        {
            return Type.Identifier.CompareTo(other.Type.Identifier);
        }
    }
}
