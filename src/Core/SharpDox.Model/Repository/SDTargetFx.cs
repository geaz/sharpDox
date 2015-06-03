using System;

namespace SharpDox.Model.Repository
{
    [Serializable]
    public class SDTargetFx : IComparable<SDTargetFx>
    {
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
        public string Identifier { get; set; }

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
        public string Name { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the description.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Beschreibung.
        ///     </summary>     
        /// </de>
        public string Description { get; set; }

        /// <default>
        ///     <summary>
        ///     Comparer to sort the a list with SDTargetFxs.
        ///     </summary>
        ///     <param name="other">SDTargetFx to compare with</param>
        ///     <returns>A value indicating, if the current SDTargetFx is lower or greater then the given one.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Vergleichsmethode, um eine Liste mit <c>SDTargetFx</c>s zu sortieren.
        ///     </summary>
        ///     <param name="other">Ein anderer <c>SDTargetFx</c> mit dem dieser verglichen werden soll.</param>
        ///     <returns>Ein Wert der angibt, ob der aktuelle <c>SDType</c> "größer" oder "kleiner" als der angegebene ist.</returns>    
        /// </de>
        public int CompareTo(SDTargetFx other)
        {
            return Identifier.CompareTo(other.Identifier);
        }
    }
}