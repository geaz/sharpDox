using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using SharpDox.Model.Repository.Members;

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
    [DebuggerDisplay("{TargetFx.Identifier}")]
    public class SDRepository
    {
        public SDRepository()
        {
            TargetFx = KnownTargetFxs.Unknown;
            Namespaces = new SortedDictionary<string, SDNamespace>();
            Types = new Dictionary<string, SDType>();
            Methods = new Dictionary<string, SDMethod>();
            Members = new Dictionary<string, SDMember>();

            KnownReferences.AddKnownNamespaces(this);
            KnownReferences.AddKnownTypes(this);
        }

        public void AddNamespace(SDNamespace sdNamespace)
        {
            if (!Namespaces.ContainsKey(sdNamespace.Identifier))
                Namespaces.Add(sdNamespace.Identifier, sdNamespace);
        }

        public void RemoveNamespace(SDNamespace sdNamespace)
        {
            Namespaces.Remove(sdNamespace.Identifier);
        }

        public void AddType(SDType sdType)
        {
            if (!Types.ContainsKey(sdType.Identifier))
            {
                Types.Add(sdType.Identifier, sdType);
                sdType.Namespace.Types.Add(sdType);
            }
        }

        public void AddMethod(SDMethod sdMethod)
        {
            if (!Methods.ContainsKey(sdMethod.Identifier))
            {
                Methods.Add(sdMethod.Identifier, sdMethod);
            }
        }

        public void AddMember(SDMember sdMember)
        {
            if (!Members.ContainsKey(sdMember.Identifier))
            {
                Members.Add(sdMember.Identifier, sdMember);
            }
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
        ///     Returns a member, referenced by its identifier.
        ///     </summary>
        ///     <param name="identifier">The identifier of the member.</param>
        ///     <returns>The member, if it is available.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert das Mitglied mit dem angegebenen Identifikator.
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
        ///     Gets a list of all namespaces (no project strangers).
        ///     </summary>
        ///     <returns>A list containing all namespaces.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert eine Liste aller Namensräume (ohne projektfremde Namespaces).
        ///     </summary>
        ///     <returns>Eine Liste aller Namensräume.</returns> 
        /// </de>
        public List<SDNamespace> GetAllNamespaces()
        {
            return Namespaces.Select(n => n.Value).Where(n => !n.IsProjectStranger).ToList();
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
        
        public SDTargetFx TargetFx { get; set; }

        private SortedDictionary<string, SDNamespace> Namespaces { get; set; }

        private Dictionary<string, SDType> Types { get; set; }

        private Dictionary<string, SDMethod> Methods { get; set; }

        private Dictionary<string, SDMember> Members { get; set; }
    }
}
