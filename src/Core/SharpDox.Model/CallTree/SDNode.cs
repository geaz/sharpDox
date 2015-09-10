using System;

namespace SharpDox.Model.CallTree
{
    /// <default>
    ///     <summary>
    ///     Baseclass for all calltree nodes.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Basisklasse für alle Elemente im Aufrufsbaums.
    ///     </summary>
    /// </de>
    [Serializable]
	public class SDNode
	{
        /// <default>
        ///     <summary>
        ///     Gets or sets the role of the node
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Rolle des Elements.
        ///     </summary>
        /// </de>
        public string Role { get; set; }
	}
}