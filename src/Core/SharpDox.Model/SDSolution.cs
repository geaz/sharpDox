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
