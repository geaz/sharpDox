using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;
using System;
using System.Linq;
using System.Collections.Generic;
using SharpDox.Model.Documentation.Article;

namespace SharpDox.Model
{
    /// <default>
    ///     <summary>
    ///     Represents a sharpDox project.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert einige Projektinformationen des aktuellen Repository.
    ///     </summary>     
    /// </de>
    [Serializable]
    public class SDProject
    {
        public SDProject()
        {
            DocumentationLanguages = new List<string>();
            Description = new Dictionary<string, string>();            
            Articles = new Dictionary<string, List<SDArticle>>();
            Images = new List<string>();
            Repositories = new Dictionary<string, SDRepository>();

            AddDocumentationLanguage("default");
        }

        public void AddRepository(string solutionFile)
        {
            if(!Repositories.ContainsKey(solutionFile))
            {
                Repositories.Add(solutionFile, new SDRepository());
            }
        }

        public void AddDocumentationLanguage(string twoLetterCode)
        {
            if (!DocumentationLanguages.Contains(twoLetterCode))
            {
                DocumentationLanguages.Add(twoLetterCode);
            }
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

        /// <default>
        ///     <summary>
        ///     Gets a list of all available documentation languages.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller vorhandenen Dokumentationssprachen.
        ///     </summary>
        /// </de>
        public List<string> DocumentationLanguages { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets a list of all available images.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller vorhandenen Bilder.
        ///     </summary>
        /// </de>
        public List<string> Images { get; private set; }

        /// <default>
        ///     <summary>
        ///     Gets the structure of the parsed navigation files including all parsed articles.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Struktur der eingelesenen Navigationsdateien einschließlich aller Artikel.
        ///     </summary>
        /// </de>
        public Dictionary<string, List<SDArticle>> Articles { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets a list of all repositories of this project. The key is the filepath to the solution.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Repositories des Projekts. Der Key ist der Dateipfad zur Solution.
        ///     </summary>
        /// </de>
        public Dictionary<string, SDRepository> Repositories { get; private set; }
    }
}
