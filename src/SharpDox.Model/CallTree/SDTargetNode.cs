using System;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Model.CallTree
{
    /// <default>
    ///     <summary>
    ///     Represents a methodcall.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Methodenaufruf.
    ///     </summary>
    /// </de>
    [Serializable]
	public class SDTargetNode : SDNode
	{
	    public SDTargetNode()
	    {
	        Role = SDNodeRole.Target;
	    }

        /// <default>
        ///     <summary>
        ///     Gets or sets a reference to the calling type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Referenz auf den aufrufenden Typen.
        ///     </summary>
        /// </de>
        public SDType CallerType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a reference to the calling method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Referenz auf die aufrufende Methode.
        ///     </summary>
        /// </de>
		public SDMethod CallerMethod { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a reference to the called type.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Referenz auf den aufgerufenen Typen.
        ///     </summary>
        /// </de>
        public SDType CalledType { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a reference to the called method.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Referenz auf die aufgerufenen Methode.
        ///     </summary>
        /// </de>
        public SDMethod CalledMethod { get; set; }
	}
}
