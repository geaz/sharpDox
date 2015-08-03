using System;
using System.Collections.Generic;
using SharpDox.Model.Documentation;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a type member.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert ein Typ-Mitglied.
    ///     </summary>
    /// </de>
    [Serializable]
    public abstract class SDMember : IComparable<SDMember>
    {
        protected SDMember()
        {
            Guid = Guid.NewGuid();
            Documentations = new SDLanguageItemCollection<SDDocumentation>();
        }

        /// <default>
        ///     <summary>
        ///     Gets the guid of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die GUID des Mitglieds.
        ///     </summary>     
        /// </de>
        public Guid Guid { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the unique identifier of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den eindeutigen Identifikator des Mitglieds.
        ///     </summary>     
        /// </de>
        public string Identifier { get; protected set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the region in which the members is defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Region in der das Mitglied steht.
        ///     </summary>     
        /// </de>
        public SDRegion Region { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the documentation collection of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Dokumentations-Kollektion des Mitglieds.
        ///     </summary>     
        /// </de>
        public SDLanguageItemCollection<SDDocumentation> Documentations { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the name of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namen des Mitglieds.
        ///     </summary>     
        /// </de>
        public string Name { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the declaring type of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den beinhaltenden Typen des Mitglieds.
        ///     </summary>     
        /// </de>
        public SDType DeclaringType { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the accessibility of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Zugriffsebene des Mitglieds.
        ///     </summary>     
        /// </de>
        public string Accessibility { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the syntax of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Mitglieds.
        ///     </summary>     
        /// </de>
        public abstract string Syntax { get; }

        /// <default>
        ///     <summary>
        ///     Returns the syntax of the member.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Mitglieds.
        ///     </summary>     
        /// </de>
        public abstract SDTemplate SyntaxTemplate { get; }

        /// <default>
        ///     <summary>
        ///     Comparer to sort the a list with SDMembers.
        ///     </summary>
        ///     <param name="other">SDMember to compare with</param>
        ///     <returns>A value indicating, if the current SDMember is lower or greater then the given one.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Vergleichsmethode, um eine Liste mit <c>SDMember</c>s zu sortieren.
        ///     </summary>
        ///     <param name="other">Ein anderer <c>SDMember</c> mit dem dieser verglichen werden soll.</param>
        ///     <returns>Ein Wert der angibt, ob der aktuelle <c>SDMember</c> "größer" oder "kleiner" als der angegebene ist.</returns>    
        /// </de>
        public int CompareTo(SDMember other)
        {
            return Identifier.CompareTo(other.Identifier);
        }
    }
}
