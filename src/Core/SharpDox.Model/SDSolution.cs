using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public Dictionary<SDTargetFx, SDRepository> Repositories { get; set; }  

        public string SolutionFile { get; private set; }

        public string Name { get { return !string.IsNullOrEmpty(SolutionFile) ? Path.GetFileNameWithoutExtension(SolutionFile) : "Unknown"; } }
    }
}
