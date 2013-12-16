namespace SharpDox.Model.CallTree
{
    /// <default>
    ///     <summary>
    ///     All roles for the calltree nodes.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Alle Rollen die ein Element im Ausdrucksbaum annehmen kann.
    ///     </summary>
    /// </de>
    public static class SDNodeRole
    {
        /// <default>
        ///     <summary>
        ///     Represents a Do-While-Loop.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine Do-While-Schleife.
        ///     </summary>
        /// </de>
        public const string DoWhileLoop = "DoWhileLoop";

        /// <default>
        ///     <summary>
        ///     Represents a While-Loop.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine While-Schleife.
        ///     </summary>
        /// </de>
        public const string WhileLoop = "WhileLoop";

        /// <default>
        ///     <summary>
        ///     Represents a For-Loop.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine For-Schleife.
        ///     </summary>
        /// </de>
        public const string ForLoop = "ForLoop";

        /// <default>
        ///     <summary>
        ///     Represents a For-Each-Loop.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine For-Each-Schleife.
        ///     </summary>
        /// </de>
        public const string ForEach = "ForEach";

        /// <default>
        ///     <summary>
        ///     Represents a Conditional-Block.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert einen Blockausdruck der eine Bedingung ist.
        ///     </summary>
        /// </de>
        public const string Conditional = "Conditional";

        /// <default>
        ///     <summary>
        ///     Represents a Switch-Block.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine Switch-Anweisung.
        ///     </summary>
        /// </de>
        public const string Switch = "Switch";

        /// <default>
        ///     <summary>
        ///     Represents a Case-Block.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Repräsentiert eine Case-Anweisung.
        ///     </summary>
        /// </de>
        public const string Case = "Case";

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
        public const string Target = "Target";
    }
}