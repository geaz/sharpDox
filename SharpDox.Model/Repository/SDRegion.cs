using System;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     Represents a region.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert eine Region.
    ///     </summary>     
    /// </de>
    [Serializable]
    public class SDRegion
    {
        /// <default>
        ///     <summary>
        ///     Gets or sets the column within the region begins.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Spalte in der die Region beginnt.
        ///     </summary>     
        /// </de>
        public int BeginColumn { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the line within the region begins.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Zeile in der die Region beginnt.
        ///     </summary>     
        /// </de>
        public int BeginLine { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the column within the region ends.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Spalte in der die Region endet.
        ///     </summary>     
        /// </de>
        public int EndColumn { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the line within the region ends.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Zeile in der die Region endet.
        ///     </summary>     
        /// </de>
        public int EndLine { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the file within the region is defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen der Datei in der die Region definiert ist.
        ///     </summary>     
        /// </de>
        public string Filename { get; set; }
    }
}
