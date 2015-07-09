using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Model
{  
    [Serializable]
    [DebuggerDisplay("{Name}")]
    public class SDSolution
    {
        public SDSolution(string solutionFile)
        {
            SolutionFile = solutionFile;
            Repositories = new Dictionary<SDTargetFx, SDRepository>();
        }

        /// <default>
        ///     <summary>
        ///     Gets an existing <see cref="SDRepository"/> with the given target framework.
        ///     Or adds and returns a new one.
        ///     </summary>
        ///     <param name="targetFx">The target framework of the <see cref="SDRepository"/>.</param>
        ///     <returns>Existing or new <see cref="SDRepository"/> with the given target framework.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert ein neues oder bestehendes <see cref="SDRepository"/> mit dem gegebenen Framework.
        ///     </summary>     
        ///     <param name="targetFx">Das Zielframework des <see cref="SDRepository"/>.</param>
        ///     <returns>Neues oder bestehendes <see cref="SDRepository"/>.</returns>
        /// </de>
        public SDRepository GetExistingOrNew(SDTargetFx targetFx)
        {
            SDRepository sdRepository;
            if (Repositories.ContainsKey(targetFx))
            {
                sdRepository = Repositories[targetFx];
            }
            else
            {
                sdRepository = new SDRepository();
                sdRepository.TargetFx = targetFx;
                Repositories.Add(targetFx, sdRepository);
            }
            return sdRepository;
        }

        /// <default>
        ///     <summary>
        ///     Returns all <see cref="SDNamespace"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDNamespace"/> in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDNamespace"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDNamespace"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.</returns>
        /// </de>
        public Dictionary<string, Dictionary<SDTargetFx, SDNamespace>> GetAllNamespaces()
        {
            var sdNamespaces = new Dictionary<string, Dictionary<SDTargetFx, SDNamespace>>();
            foreach (var repository in Repositories)
            {
                foreach (var repoNamespace in repository.Value.GetAllNamespaces())
                {
                    var sdNamespace = new Dictionary<SDTargetFx, SDNamespace>();
                    if (sdNamespaces.ContainsKey(repoNamespace.Identifier))
                    {
                        sdNamespace = sdNamespaces[repoNamespace.Identifier];
                    }
                    else
                    {
                        sdNamespaces.Add(repoNamespace.Identifier, sdNamespace);
                    }

                    sdNamespace.Add(repository.Key, repoNamespace);
                }
            }
            return sdNamespaces;
        }

        /// <default>
        ///     <summary>
        ///     Returns all <see cref="SDType"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDType"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDType"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDType"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.</returns>
        /// </de>
        public Dictionary<string, Dictionary<SDTargetFx, SDType>> GetAllTypes()
        {
            var sdTypes = new Dictionary<string, Dictionary<SDTargetFx, SDType>>();
            foreach (var repository in Repositories)
            {
                foreach (var repoType in repository.Value.GetAllTypes())
                {
                    var sdType = new Dictionary<SDTargetFx, SDType>();
                    if (!repoType.IsProjectStranger)
                    {
                        if (sdTypes.ContainsKey(repoType.Identifier))
                        {
                            sdType = sdTypes[repoType.Identifier];
                        }
                        else
                        {
                            sdTypes.Add(repoType.Identifier, sdType);
                        }
                        sdType.Add(repository.Key, repoType);
                    }
                }
            }
            return sdTypes;
        }

        /// <default>
        ///     <summary>
        ///     Returns all <see cref="SDMember"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDMember"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDTargetFx"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDMember"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDMember"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDTargetFx"/>.</returns>
        /// </de>
        public Dictionary<string, Dictionary<SDTargetFx, SDMember>> GetAllMembers()
        {
            var sdMembers = new Dictionary<string, Dictionary<SDTargetFx, SDMember>>();
            foreach (var repository in Repositories)
            {
                foreach (var repoType in repository.Value.GetAllTypes())
                {
                    var sdMember = new Dictionary<SDTargetFx, SDMember>();
                    if (!repoType.IsProjectStranger)
                    {
                        AddMembers(sdMembers, sdMember, repoType.Fields, repository.Key);
                        AddMembers(sdMembers, sdMember, repoType.Constructors, repository.Key);
                        AddMembers(sdMembers, sdMember, repoType.Methods, repository.Key);
                        AddMembers(sdMembers, sdMember, repoType.Events, repository.Key);
                        AddMembers(sdMembers, sdMember, repoType.Properties, repository.Key);
                    }
                }
            }
            return sdMembers;
        }

        private void AddMembers(
            Dictionary<string, Dictionary<SDTargetFx, SDMember>> sdMembers, Dictionary<SDTargetFx, SDMember> sdMember, 
            IEnumerable<SDMember> members, SDTargetFx targetFx)
        {
            foreach (var member in members)
            {
                if (sdMembers.ContainsKey(member.Identifier))
                {
                    sdMember = sdMembers[member.Identifier];
                }
                else
                {
                    sdMembers.Add(member.Identifier, sdMember);
                }
                sdMember.Add(targetFx, member);
            }
        }

        public Dictionary<SDTargetFx, SDRepository> Repositories { get; set; }  

        public string SolutionFile { get; private set; }

        public string Name { get { return !string.IsNullOrEmpty(SolutionFile) ? Path.GetFileNameWithoutExtension(SolutionFile) : "Unknown"; } }
    }
}
