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
        ///     Gets or sets the start of the region.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Start der Region.
        ///     </summary>     
        /// </de>
        public int Start { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the end of the region.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert das Ende der Region.
        ///     </summary>     
        /// </de>
        public int End { get; set; }

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
