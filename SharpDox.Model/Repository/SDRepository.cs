using System;
using System.Linq;
using System.Collections.Generic;
using SharpDox.Model.Repository.Members;
using SharpDox.Model.Documentation;

namespace SharpDox.Model.Repository
{
    /// <default>
    ///     <summary>
    ///     The repository contains the whole parsed solution.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Das Repository beinhaltet alle Informationen der eingelesenen Lösung.
    ///     </summary>     
    /// </de>
    [Serializable]
	public class SDRepository
    {
		public SDRepository()
		{
            ProjectInfo = new SDProjectInfo();
			Namespaces = new List<SDNamespace>();
			Types = new List<SDType>();
            DocumentationLanguages = new List<string>();
            Articles = new Dictionary<string, List<SDArticle>>();
            Images = new List<string>();

            AddDocumentationLanguage("default");
		}
        
        public void AddDocumentationLanguage(string twoLetterCode)
        {
            if (DocumentationLanguages.SingleOrDefault(d => d == twoLetterCode) == null)
            {
                DocumentationLanguages.Add(twoLetterCode);
            }
        }

		public void AddNamespace(SDNamespace nameSpace)
		{
            if (GetNamespaceByIdentifier(nameSpace.Identifier) == null)
                Namespaces.Add(nameSpace);
		}

		public void AddType(SDType type)
        {
            if(GetTypeByIdentifier(type.Identifier) == null)
                Types.Add(type);
        }

        public void AddNamespaceTypeRelation(string namespaceIdentifier, string typeIdentifier)
		{
            var sdNamespace = GetNamespaceByIdentifier(namespaceIdentifier);
            var sdType = GetTypeByIdentifier(typeIdentifier);

			if (sdNamespace != null && sdType != null && sdNamespace.Types.SingleOrDefault(t => t.Identifier == sdType.Identifier) == null)
			{
                sdNamespace.Types.Add(sdType);
			}
		}

        public Guid GetGuidByIdentifier(string identifier)
        {
            var sdNamespace = GetNamespaceByIdentifier(identifier);
            if (sdNamespace != null) return sdNamespace.Guid;

            var sdType = GetTypeByIdentifier(identifier);
            if (sdType != null) return sdType.Guid;

            var sdConstructor = Types.SelectMany(t => t.Constructors).SingleOrDefault(m => m.Identifier == identifier);
            if (sdConstructor != null) return sdConstructor.Guid;

            var sdMethod = Types.SelectMany(t => t.Methods).SingleOrDefault(m => m.Identifier == identifier);
            if (sdMethod != null) return sdMethod.Guid;

            var sdField = Types.SelectMany(t => t.Fields).SingleOrDefault(f => f.Identifier == identifier);
            if (sdField != null) return sdField.Guid;

            var sdEvent = Types.SelectMany(t => t.Events).SingleOrDefault(f => f.Identifier == identifier);
            if (sdEvent != null) return sdEvent.Guid;

            var sdProperty = Types.SelectMany(t => t.Properties).SingleOrDefault(f => f.Identifier == identifier);
            if (sdProperty != null) return sdProperty.Guid;

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
            return Namespaces.SingleOrDefault(o => o.Identifier == identifier);
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
            return Types.SingleOrDefault(o => o.Identifier == identifier);
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
            var sdConstructor = Types.SelectMany(t => t.Constructors).SingleOrDefault(m => m.Identifier == identifier);
            if (sdConstructor != null) return sdConstructor;

            var sdMethod = Types.SelectMany(t => t.Methods).SingleOrDefault(m => m.Identifier == identifier);
            return sdMethod ?? null;
        }
        
        /// <default>
        ///     <summary>
        ///     Gets a list of all containing namespaces.
        ///     </summary>
        ///     <returns>A list containing all namespaces.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Namensräume.
        ///     </summary>
        ///     <returns>Eine Liste aller Namensräume.</returns> 
        /// </de>
        public List<SDNamespace> GetAllNamespaces()
        {
            return Namespaces.OrderBy(n => n.Fullname).ToList();
        }

        /// <default>
        ///     <summary>
        ///     Gets a list of all containing types.
        ///     </summary>
        ///     <returns>A list containing all types.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Typen.
        ///     </summary>
        ///     <returns>Eine Liste aller Typen.</returns> 
        /// </de>
        public List<SDType> GetAllTypes()
        {
            return Types;
        }

        /// <default>
        ///     <summary>
        ///     Gets the project infos of the parsed solution.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Projektinformationen der eingelesenen Lösung.
        ///     </summary>
        /// </de>
        public SDProjectInfo ProjectInfo { get; private set; }

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
        ///     Gets a list of all available images.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller vorhandenen Bilder.
        ///     </summary>
        /// </de>
        public List<string> Images { get; private set; }

		private List<SDNamespace> Namespaces { get; set; }

		private List<SDType> Types { get; set; }
	}
}
