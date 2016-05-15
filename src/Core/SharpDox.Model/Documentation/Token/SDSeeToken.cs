using System;

namespace SharpDox.Model.Documentation.Token
{
    /// <default>
    ///     <summary>
    ///     Represents a reference within an inline documentation.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert eine Referenz innerhalb einer Inline-Dokumentation.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDSeeToken : SDToken
    {
        public SDSeeToken(string attributeValue)
        {
            AttributeValue = attributeValue;
            Role = SDTokenRole.See;
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the identifier of the referenced entity.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Identifier der referenzierten Entität.
        ///     </summary>
        /// </de>
        public string Identifier { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the referenced entity.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen der referenzierten Entität.
        ///     </summary>
        /// </de>
        public string Name { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the namespace of the referenced entity.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namensraum der referenzierten Entität.
        ///     </summary>
        /// </de>
        public string Namespace { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the declaring type of the reference.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den beinhaltenden Typen der referenzierten Entität.
        ///     </summary>
        /// </de>
        public string DeclaringType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the value of the xml attribute.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Wert des XML Attributes.
        ///     </summary>
        /// </de>
        public string AttributeValue { get; set; }
    }
}
