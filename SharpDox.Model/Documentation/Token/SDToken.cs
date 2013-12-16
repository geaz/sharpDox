using System;

namespace SharpDox.Model.Documentation.Token
{
    /// <default>
    ///     <summary>
    ///     Represents simple text in an inline documentation and is also the
    ///     base for the <see cref="SDSeeToken">SDSeeToken</see> and 
    ///     the <see cref="SDCodeToken">SDCodeToken</see>.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen "einfachen" Text in einer Inline-Dokumentation.
    ///     Außerdem ist es die BAsis-Klasse für <see cref="SDSeeToken">SDSeeToken</see> und 
    ///     dem <see cref="SDCodeToken">SDCodeToken</see>.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDToken
    {
        /// <default>
        ///     <summary>
        ///     Gets or sets the role of the token. 
        ///     One of <see cref="SDTokenRole">SDTokenRole</see>.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Rolle des Token.
        ///     Eins von <see cref="SDTokenRole">SDTokenRole</see>.
        ///     </summary>
        /// </de>
        public string Role { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the documentation text.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Dokumentationstext.
        ///     </summary>
        /// </de>
        public virtual string Text { get; set; } 
    }
}
