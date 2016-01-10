using System.Linq;
using SharpDox.Model.Documentation;
using System;
using System.Collections.Generic;
using SharpDox.Model.Documentation.Article;
using SharpDox.Model.Repository;

namespace SharpDox.Model
{
    /// <default>
    ///     <summary>
    ///     Represents a sharpDox project. <c>Test</c>
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
            Descriptions = new SDLanguageItemCollection<SDTemplate>();
            Articles = new SDLanguageItemCollection<List<SDArticle>>();
            Tokens = new Dictionary<string, string>();
            Images = new List<string>();
            Solutions = new Dictionary<string, SDSolution>();

            AddDocumentationLanguage("default");
        }

        public void AddSolution(string solutionFile)
        {
            if(!Solutions.ContainsKey(solutionFile))
            {
                Solutions.Add(solutionFile, new SDSolution(solutionFile));
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
        ///     Returns all <see cref="SDTargetFx"/>s available in the current <see cref="SDProject"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDTargetFx"/>s available in the current <see cref="SDProject"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDTargetFx"/>s im aktuellem <see cref="SDProject"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDTargetFx"/>s im aktuellem <see cref="SDProject"/>.</returns>
        /// </de>
        public List<SDTargetFx> GetAllAvailableTargetFxs()
        {
            var targetFxs = new List<SDTargetFx>();
            foreach (var sdSolution in Solutions)
            {
                foreach (var sdRepository in sdSolution.Value.Repositories)
                {
                    if (targetFxs.SingleOrDefault(t => t.Identifier == sdRepository.TargetFx.Identifier) == null)
                    {
                        targetFxs.Add(sdRepository.TargetFx);
                    }
                }
            }
            return targetFxs;
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
        public SDLanguageItemCollection<SDTemplate> Descriptions { get; set; }

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
        ///     Gets or sets the project url.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert Projekturl.
        ///     </summary>     
        /// </de>
        public string ProjectUrl { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets or sets the author url.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Setzt oder liefert die Autorenurl.
        ///     </summary>     
        /// </de>
        public string AuthorUrl { get; set; }

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
        public SDLanguageItemCollection<List<SDArticle>> Articles { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets all defined text tokens (see file tokens.sdt).
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle definierten Text-Bausteine (siehe Datei tokens.sdt).
        ///     </summary>
        /// </de>
        public Dictionary<string, string> Tokens { get; set; }

        /// <default>
        ///     <summary>
        ///     Gets a list of all solutions of this project. The key is the filepath to the solution.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Solutions des Projekts. Der Key ist der Dateipfad zur Solution.
        ///     </summary>
        /// </de>
        public Dictionary<string, SDSolution> Solutions { get; private set; }
    }
}
