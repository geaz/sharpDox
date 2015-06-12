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

        public void AppendRepository(SDRepository repository)
        {
            if (Repositories.ContainsKey(repository.TargetFx))
            {
                Repositories[repository.TargetFx].MergeWith(repository);
            }
            else
            {
                Repositories.Add(repository.TargetFx, repository);
            }
        }

        public Dictionary<SDTargetFx, SDRepository> Repositories { get; set; }  

        public string SolutionFile { get; private set; }

        public string Name { get { return !string.IsNullOrEmpty(SolutionFile) ? Path.GetFileNameWithoutExtension(SolutionFile) : "Unknown"; } }
    }
}
