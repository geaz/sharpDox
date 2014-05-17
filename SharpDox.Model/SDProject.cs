using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;
using System;
using System.Linq;
using System.Collections.Generic;
using SharpDox.Model.Documentation.Article;
using SharpDox.Model.Repository.Members;

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

        public Guid GetGuidByIdentifier(string identifier)
        {
            var sdNamespace = GetNamespaceByIdentifier(identifier);
            if (sdNamespace != null) return sdNamespace.Guid;

            var sdType = GetTypeByIdentifier(identifier);
            if (sdType != null) return sdType.Guid;

            var sdMethod = GetMethodByIdentifier(identifier);
            if (sdMethod != null) return sdMethod.Guid;

            var sdMember = GetMemberByIdentifier(identifier);
            if (sdMember != null) return sdMember.Guid;

            return Guid.Empty;
        }

        /// <default>
        ///     <summary>
        ///     Returns a namespace, referenced by its identifier.
        ///     </summary>
        ///     <param name="identifier">The identifier of the namespace.</param>
        ///     <returns>The namespace if it is available.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namensraum mit dem angegebenen Identifikator.
        ///     </summary>
        ///     <param name="identifier">Der Identifikator des Namensraum.</param>
        ///     <returns>Der Namensraum, falls dieser vorhanden ist.</returns>   
        /// </de>
        public SDNamespace GetNamespaceByIdentifier(string identifier)
        {
            SDNamespace sdNamespace = null;
            foreach (var repository in Repositories.Values)
            {
                sdNamespace = repository.GetNamespaceByIdentifier(identifier);
                if (sdNamespace != null) break;
            }

            return sdNamespace;
        }

        /// <default>
        ///     <summary>
        ///     Returns a type, referenced by its identifier.
        ///     </summary>
        ///     <param name="identifier">The identifier of the type.</param>
        ///     <returns>The type, if it is available.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Typen mit dem angegebenen Identifikator.
        ///     </summary>
        ///     <param name="identifier">Der Identifikator des Typen.</param>
        ///     <returns>Der Typ, falls dieser vorhanden ist.</returns>  
        /// </de>
        public SDType GetTypeByIdentifier(string identifier)
        {
            SDType sdType = null;
            foreach (var repository in Repositories.Values)
            {
                sdType = repository.GetTypeByIdentifier(identifier);
                if (sdType != null) break;
            }

            return sdType;
        }

        /// <default>
        ///     <summary>
        ///     Returns a method, referenced by its identifier.
        ///     </summary>
        ///     <param name="identifier">The identifier of the method.</param>
        ///     <returns>The method, if it is available.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Methode mit dem angegebenen Identifikator.
        ///     </summary>
        ///     <param name="identifier">Der Identifikator der Methode.</param>
        ///     <returns>Die Methode, falls diese vorhanden ist.</returns>  
        /// </de>
        public SDMethod GetMethodByIdentifier(string identifier)
        {
            SDMethod sdMethod = null;
            foreach (var repository in Repositories.Values)
            {
                sdMethod = repository.GetMethodByIdentifier(identifier);
                if (sdMethod != null) break;
            }

            return sdMethod;
        }

        /// <default>
        ///     <summary>
        ///     Returns a member other than a method/constructor, referenced by its identifier.
        ///     </summary>
        ///     <param name="identifier">The identifier of the member.</param>
        ///     <returns>The member, if it is available.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert das Mitglied mit dem angegebenen Identifikator (außer Methoden / Konstruktoren).
        ///     </summary>
        ///     <param name="identifier">Der Identifikator des Mitglieds.</param>
        ///     <returns>Das Mitglied, falls dieses vorhanden ist.</returns>  
        /// </de>
        public SDMember GetMemberByIdentifier(string identifier)
        {
            SDMember sdMember = null;
            foreach (var repository in Repositories.Values)
            {
                sdMember = repository.GetMemberByIdentifier(identifier);
                if (sdMember != null) break;
            }

            return sdMember;
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
