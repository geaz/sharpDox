using System;
using System.Collections.Generic;

namespace SharpDox.Model.CallTree
{
    /// <default>
    ///     <summary>
    ///     Represents a statement block which isn't a conditonal one.
    ///     </summary>
    ///     <example>
    ///     <code>
    ///     <![CDATA[
    ///     while(true)
    ///     {
    ///         DoSomething();
    ///     }
    ///     ]]>
    ///     </code>
    ///     </example>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Blockausdruck der keine Bedingung ist.
    ///     </summary>
    ///     <example>
    ///     <code>
    ///     <![CDATA[
    ///     while(true)
    ///     {
    ///         DoSomething();
    ///     }
    ///     ]]>
    ///     </code>         
    ///     </example>
    /// </de>
    [Serializable]
    public class SDBlock : SDNode
    {
        public SDBlock()
        {
            Statements = new List<SDNode>();
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the block expression.
        ///     </summary>
        ///     <example><c>while(x = y)</c> or <c><![CDATA[for(int i = 0; i < x; i++)]]></c></example>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Blockausdruck.
        ///     </summary>
        ///     <example><c>while(x = y)</c> oder <c><![CDATA[for(int i = 0; i < x; i++)]]></c></example>
        /// </de>
        public string Expression { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets all statements in the block.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert alle Ausdrücke innerhalb des Blocks.
        ///     </summary>
        /// </de>
        public List<SDNode> Statements { get; set; }
    }
}