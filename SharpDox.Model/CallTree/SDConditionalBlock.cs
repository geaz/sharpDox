using System;
using System.Collections.Generic;

namespace SharpDox.Model.CallTree
{
    /// <default>
    ///     <summary>
    ///     Represents a conditional statement block.
    ///     </summary>
    ///     <example>
    ///     <code>
    ///     <![CDATA[
    ///     if(x = y)
    ///     {
    ///         [TrueStatements]
    ///     }
    ///     else
    ///     {
    ///         [FalseStatements]
    ///     }
    ///     ]]>
    ///     </code>
    ///     </example>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Blockausdruck der eine Bedingung ist.
    ///     </summary>
    ///     <example>
    ///     <code>
    ///     <![CDATA[
    ///     if(x = y)
    ///     {
    ///         [TrueStatements]
    ///     }
    ///     else
    ///     {
    ///         [FalseStatements]
    ///     }
    ///     ]]>
    ///     </code>         
    ///     </example>
    /// </de>
    [Serializable]
    public class SDConditionalBlock : SDNode
    {
        public SDConditionalBlock()
	    {
			TrueStatements = new List<SDNode>();
            FalseStatements = new List<SDNode>();
	    }

        /// <default>
        ///     <summary>
        ///     Gets or sets the conditional expression.
        ///     </summary>
        ///     <example><c>if(x = y)</c></example>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Bedingung.
        ///     </summary>
        ///     <example>Ein kleiner test <c>if(x = y)</c> mit inline code</example>
        /// </de>
        public string Expression { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets all statements in the true branch.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert alle Ausdrücke innerhalb If-Blocks.
        ///     </summary>
        /// </de>
        public List<SDNode> TrueStatements { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets all statements in the false branch.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert alle Ausdrücke innerhalb des Else-Blocks.
        ///     </summary>
        /// </de>
        public List<SDNode> FalseStatements { get; set; }
    }
}
