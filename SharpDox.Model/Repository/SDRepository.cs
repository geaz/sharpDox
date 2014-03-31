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
        private Dictionary<string, string> _uniqueMethodUrls = new Dictionary<string, string>();
        private Dictionary<string, int> _methodNameCount = new Dictionary<string, int>();

        public SDRepository()
        {
            ProjectInfo = new SDProjectInfo();
            Namespaces = new Dictionary<string, SDNamespace>();
            Types = new Dictionary<string, SDType>();
            Methods = new Dictionary<string, SDMethod>();
            Members = new Dictionary<string, SDMember>();
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

        public void AddNamespace(SDNamespace sdNamespace)
        {
            if (!Namespaces.ContainsKey(sdNamespace.Identifier))
                Namespaces.Add(sdNamespace.Identifier, sdNamespace);
        }

        public void AddType(SDType sdType)
        {
            if (!Types.ContainsKey(sdType.Identifier))
                Types.Add(sdType.Identifier, sdType);
        }

        public void AddMethod(SDMethod sdMethod)
        {
            if (!Methods.ContainsKey(sdMethod.Identifier))
            {
                sdMethod.InternalIdentifier = GetUniqueShortMethodIdentifier(sdMethod);
                Methods.Add(sdMethod.Identifier, sdMethod);
            }
        }

        public void AddMember(SDMember sdMember)
        {
            if (!Members.ContainsKey(sdMember.Identifier))
            {
                sdMember.InternalIdentifier = sdMember.Name;
                Members.Add(sdMember.Identifier, sdMember);
            }
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
            Namespaces.TryGetValue(identifier, out sdNamespace);

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
            Types.TryGetValue(identifier, out sdType);

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
            Methods.TryGetValue(identifier, out sdMethod);

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
            Members.TryGetValue(identifier, out sdMember);

            if (sdMember == null)
            {
                var sdMethod = GetMethodByIdentifier(identifier);
                if (sdMethod != null) sdMember = sdMethod;
            }

            return sdMember;
        }

        /// <default>
        ///     <summary>
        ///     Gets a list of all namespaces.
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
            return Namespaces.Select(n => n.Value).ToList();
        }

        /// <default>
        ///     <summary>
        ///     Gets a list of all types.
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
            return Types.Select(n => n.Value).ToList();
        }

        /// <default>
        ///     <summary>
        ///     Gets a list of all methods.
        ///     </summary>
        ///     <returns>A list containing all methods.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Methoden.
        ///     </summary>
        ///     <returns>Eine Liste aller Methoden.</returns> 
        /// </de>
        public List<SDMethod> GetAllMethods()
        {
            return Methods.Select(n => n.Value).ToList();
        }

        private string GetUniqueShortMethodIdentifier(SDMethod sdMethod)
        {
            if (!_uniqueMethodUrls.ContainsKey(sdMethod.Identifier))
            {
                if (!_methodNameCount.ContainsKey(sdMethod.Name))
                {
                    _methodNameCount.Add(sdMethod.Name, 1);
                }
                else
                {
                    _methodNameCount[sdMethod.Name]++;
                }

                _uniqueMethodUrls.Add(sdMethod.Identifier, string.Format("{0}{1}", sdMethod.Name, _methodNameCount[sdMethod.Name]));
            }

            return _uniqueMethodUrls[sdMethod.Identifier];
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

        private Dictionary<string, SDNamespace> Namespaces { get; set; }

        private Dictionary<string, SDType> Types { get; set; }

        private Dictionary<string, SDMethod> Methods { get; set; }

        private Dictionary<string, SDMember> Members { get; set; }
    }
}
