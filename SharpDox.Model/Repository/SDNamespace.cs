using System;
using System.Collections.Generic;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     Represents a namespace.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Namensraum.
    ///     </summary>     
    /// </de>
    [Serializable]
    public class SDNamespace
    {
        public SDNamespace(string fullname)
        {
            Guid = Guid.NewGuid();
            Identifier = fullname;
            Fullname = fullname;
            IsProjectStranger = false;

            Types = new List<SDType>();
            Uses = new List<SDNamespace>();
            UsedBy = new List<SDNamespace>();
        }

        /// <default>
        ///     <summary>
        ///     Gets the guid of the namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die GUID des Namensraum.
        ///     </summary>     
        /// </de>
        public Guid Guid { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the unique identifier of the namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den eindeutigen Identifikator des Namensraum.
        ///     </summary>     
        /// </de>
        public string Identifier { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the fullname of the namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den vollen Namen des Namensraum.
        ///     </summary>     
        /// </de>
		public string Fullname { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the name of the declaring assembly.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert den Namen der Assembly.
        ///     </summary>     
        /// </de>
        public string Assemblyname { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the descriptions of the namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Beschreibung des Namensraum.
        ///     </summary>     
        /// </de>
        public Dictionary<string,string> Description { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a value indicating whether the namespace is a project member or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert einen Wert der angibt, ob der Namensraum zum Projekt gehört oder nicht.
        ///     </summary>     
        /// </de>
        public bool IsProjectStranger { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all including types.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller beinhalteten Typen.
        ///     </summary>     
        /// </de>
        public List<SDType> Types { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all namespaces which are used by this namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Namensräume, die innerhalb diesem genutzt werden.
        ///     </summary>     
        /// </de>
        public List<SDNamespace> Uses { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets a list of all namespaces which are using this namespace.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert eine Liste aller Namensräume, die diesen Namensraum nutzen.
        ///     </summary>     
        /// </de>
        public List<SDNamespace> UsedBy { get; private set; }
    }
}
