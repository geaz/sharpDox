using System;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a parameter of a method.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Parameter.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDParameter
    {
        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the parameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Parameters.
        ///     </summary>
        /// </de>
		public string Name { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the type of the parameter.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Typen des Parameters.
        ///     </summary>
        /// </de>
        public SDType ParamType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the constant value, if defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den konstanten Wert, falls einer definiert wurde.
        ///     </summary>
        /// </de>
		public object ConstantValue { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the parameter is constant or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Parameter konstant ist oder nicht.
        ///     </summary>
        /// </de>
		public bool IsConst { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the parameter is optional or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Parameter optional ist oder nicht.
        ///     </summary>
        /// </de>
		public bool IsOptional { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the parameter is a reference or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Parameter eine Referenz ist oder nicht.
        ///     </summary>
        /// </de>
		public bool IsRef { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the parameter is a out parameter or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob dieser Parameter ein "out" Parameter ist oder nicht.
        ///     </summary>
        /// </de>
		public bool IsOut { get; set; }
    }
}
