using System;
using System.Collections.Generic;

namespace SharpDox.Model.Documentation.Article
{
    /// <default>
    ///     <summary>
    ///     Represents an article in the solution path.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einen Artikel im Lösungpfad.
    ///     </summary>
    /// </de>
    [Serializable]
    public class SDArticle
    {
        public SDArticle()
        {
            Guid = Guid.NewGuid();
            Children = new List<SDArticle>();
        }

        /// <default>
        ///     <summary>
        ///     Returns the guid of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die GUID des Artikels.
        ///     </summary>
        /// </de>
        public Guid Guid { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the filename of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Dateinamen des Artikels.
        ///     </summary>
        /// </de>
        public string Filename { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the title of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Titel des Artikels.
        ///     </summary>
        /// </de>
        public string Title { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the content of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Inhalt des Artikels.
        ///     </summary>
        /// </de>
        public string Content { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the parent of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den übergeordneten Artikel des Artikels.
        ///     </summary>
        /// </de>
        public SDArticle Parent { get; set; }

        /// <default>
        ///     <summary>
        ///     Returns the children of the article.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die untergeordneten Artikel des Artikels.
        ///     </summary>
        /// </de>
        public List<SDArticle> Children { get; set; }
    }
}
