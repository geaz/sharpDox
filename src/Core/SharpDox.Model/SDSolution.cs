using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharpDox.Model.Repository;

namespace SharpDox.Model
{  
    [Serializable]
    [DebuggerDisplay("{Name}")]
    public class SDSolution
    {
        public SDSolution(string solutionFile)
        {
            SolutionFile = solutionFile;
            Repositories = new List<SDRepository>();
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
            var sdRepository = Repositories.SingleOrDefault(r => r.TargetFx == targetFx);
            if (sdRepository == null)
            {
                sdRepository = new SDRepository();
                sdRepository.TargetFx = targetFx;
                Repositories.Add(sdRepository);
            }
            return sdRepository;
        }

        /// <default>
        ///     <summary>
        ///     Returns all <see cref="SDNamespace"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDRepository"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDNamespace"/> in the current <see cref="SDSolution"/> grouped by it's <see cref="SDRepository"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDNamespace"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDRepository"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDNamespace"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDRepository"/>.</returns>
        /// </de>
        public Dictionary<string, Dictionary<SDRepository, SDNamespace>> GetAllNamespaces()
        {
            var sdNamespaces = new Dictionary<string, Dictionary<SDRepository, SDNamespace>>();
            foreach (var repository in Repositories)
            {
                foreach (var repoNamespace in repository.GetAllNamespaces())
                {
                    var sdNamespace = new Dictionary<SDRepository, SDNamespace>();
                    if (sdNamespaces.ContainsKey(repoNamespace.Identifier))
                    {
                        sdNamespace = sdNamespaces[repoNamespace.Identifier];
                    }
                    else
                    {
                        sdNamespaces.Add(repoNamespace.Identifier, sdNamespace);
                    }

                    sdNamespace.Add(repository, repoNamespace);
                }
            }
            return sdNamespaces;
        }

        /// <default>
        ///     <summary>
        ///     Returns all <see cref="SDType"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDRepository"/>.
        ///     </summary>
        ///     <returns>All <see cref="SDType"/>s in the current <see cref="SDSolution"/> grouped by it's <see cref="SDRepository"/>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle <see cref="SDType"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDRepository"/>.
        ///     </summary>     
        ///     <returns>Alle <see cref="SDType"/>s in der aktuellen <see cref="SDSolution"/> gruppiert bei dem jeweiligen <see cref="SDRepository"/>.</returns>
        /// </de>
        public Dictionary<string, Dictionary<SDRepository, SDType>> GetAllTypes()
        {
            var sdTypes = new Dictionary<string, Dictionary<SDRepository, SDType>>();
            foreach (var repository in Repositories)
            {
                foreach (var repoType in repository.GetAllTypes())
                {
                    var sdType = new Dictionary<SDRepository, SDType>();
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
                        sdType.Add(repository, repoType);
                    }
                }
            }
            return sdTypes;
        }

        public List<SDRepository> Repositories { get; set; }  

        public string SolutionFile { get; private set; }

        public string Name { get { return !string.IsNullOrEmpty(SolutionFile) ? Path.GetFileNameWithoutExtension(SolutionFile) : "Unknown"; } }
    }
}
