using System;
using System.Collections.Generic;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     Represents some project information of the current repository.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einige Projektinformationen des aktuellen Repository.
    ///     </summary>     
    /// </de>
    [Serializable]
    public class SDProjectInfo
    {
        public SDProjectInfo()
        {
            Description = new Dictionary<string, string>();
        }

        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen des Projekts.
        ///     </summary>     
        /// </de>
        public string ProjectName { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the description of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Beschreibung des Projekts.
        ///     </summary>     
        /// </de>
        public Dictionary<string, string> Description { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the version of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Version des Projekts.
        ///     </summary>     
        /// </de>
        public string VersionNumber { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the author of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Autor des Projekts.
        ///     </summary>     
        /// </de>
        public string Author { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the logo path of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Logopfad des Projekts.
        ///     </summary>     
        /// </de>
        public string LogoPath { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the standard documentation language of the project.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die standard Dokumentationssprache.
        ///     </summary>     
        /// </de>
        public string DocLanguage { get; set; }
    }
}
