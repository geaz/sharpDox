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
        ///     Gets or sets the start line of the region.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Startzeile der Region.
        ///     </summary>     
        /// </de>
        public int StartLine { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the end line of the region.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Endzeile der Region.
        ///     </summary>     
        /// </de>
        public int EndLine { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the file path within the region is defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Pfad der Datei in der die Region definiert ist.
        ///     </summary>     
        /// </de>
        public string FilePath { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the file name within the region is defined.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen der Datei in der die Region definiert ist.
        ///     </summary>     
        /// </de>
        public string Filename { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the content of the file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Inhalt der Datei.
        ///     </summary>     
        /// </de>
        public string Content { get; set; }
    }
}
