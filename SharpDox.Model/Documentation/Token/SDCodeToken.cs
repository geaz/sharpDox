using System;

namespace SharpDox.Model.Documentation.Token
{
    /// <default>
    ///     <summary>
    ///     The code token represents the "code" section of an inline documentation.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Der Code-Token repräsentiert einen Code-Abschnitt einer Inline-Dokumentation.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDCodeToken : SDToken
    {
        public SDCodeToken()
        {
            Role = SDTokenRole.Code;
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether 
        ///     the token represents a code section (code) 
        ///     or a code snippet within the documentation text (c).
        /// </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt,  ob der Token
        ///     eine Code-Sektion (code) oder einen Inline-Code-Abschnitt (c) repräsentiert.
        ///     </summary>
        /// </de>
		public bool IsInline { get; set; }
    }
}
