using System;
using System.Collections.Generic;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     Represents a typeparameter.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Typparameter.
    ///     </summary>     
    /// </de>
    [Serializable]
    public class SDTypeParameter : IComparable<SDTypeParameter>
    {
        public SDTypeParameter()
        {
            ConstraintTypes = new List<SDTypeRef>();
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the typeparameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Typparameters.
        ///     </summary>     
        /// </de>
        public string Name { get; set; }
        
        /// <default>
        ///     <summary>
        ///     Gets the constraint types of this type parameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die ConstraintTypes des Typparameters.
        ///     </summary>     
        /// </de>
        public List<SDTypeRef> ConstraintTypes { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets an value indicating whether the type parameter has the 'new()' constraint.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob der Typparameter die 'new()'-Einschränkung besitzt.
        ///     </summary>     
        /// </de>
        public bool HasDefaultConstructorConstraint { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets an value indicating whether the type parameter has the 'class' constraint.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob der Typparameter die 'class'-Einschränkung besitzt.
        ///     </summary>     
        /// </de>
        public bool HasReferenceTypeConstraint { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets an value indicating whether the type parameter has the 'struct' constraint.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob der Typparameter die 'struct'-Einschränkung besitzt.
        ///     </summary>     
        /// </de>
        public bool HasValueTypeConstraint { get; set; }

        /// <default>
        ///     <summary>
        ///     Comparer to sort the a list with SDTypeParameters.
        ///     </summary>
        ///     <param name="other">SDTypeParameter to compare with</param>
        ///     <returns>A value indicating, if the current SDTypeParameter is lower or greater then the given one.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Vergleichsmethode, um eine Liste mit <c>SDTypeParameter</c>s zu sortieren.
        ///     </summary>
        ///     <param name="other">Ein anderer <c>SDTypeParameter</c> mit dem dieser verglichen werden soll.</param>
        ///     <returns>Ein Wert der angibt, ob der aktuelle <c>SDTypeParameter</c> "größer" oder "kleiner" als der angegebene ist.</returns>    
        /// </de>
        public int CompareTo(SDTypeParameter other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}