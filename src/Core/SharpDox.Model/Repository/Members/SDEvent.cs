using System;
using SharpDox.Model.Documentation;

namespace SharpDox.Model.Repository.Members
{
    /// <default>
    ///     <summary>
    ///     Represents a event.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert ein Event.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDEvent : SDMember
    {
        public SDEvent(string identifier)
        {
            Identifier = identifier;
        }

        /// <default>
        ///     <summary>
        ///     Returns the syntax of the event.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Syntax des Events.
        ///     </summary>
        /// </de>
        public override string Syntax
        {
            get
            {
                return string.Join(" " , new string[] { Accessibility, Name });
            }
        }

        public override SDTemplate SyntaxTemplate { get { return new SDTemplate(Syntax); } }
    }
}
