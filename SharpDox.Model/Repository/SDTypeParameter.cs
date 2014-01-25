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
    public class SDTypeParameter
    {
        public SDTypeParameter()
        {
            Interfaces = new List<SDType>();
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
        ///     Gets the base class of this type parameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Basisklasse des Typparameters.
        ///     </summary>     
        /// </de>
        public SDType BaseClass { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets the interface set of this type parameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Interfaces des Typparameters.
        ///     </summary>     
        /// </de>
        public List<SDType> Interfaces { get; private set; }

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
    }
}