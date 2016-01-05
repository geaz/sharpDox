using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDox.Build.Roslyn
{
    internal class RoslynLoader
    {
        public void LoadSolutionFile(string solutionFile)
        {
            var workspace = MSBuildWorkspace.Create();
            if (FileIsSolution(solutionFile))
            {
                var solution = workspace.OpenSolutionAsync(solutionFile).Result;
            }
            else if (FileIsSolution(solutionFile))
            {
                var project = workspace.OpenProjectAsync(solutionFile).Result;
            }            
        }

        private static bool FileIsSolution(string pathToSolutionFile)
        {
            var extension = Path.GetExtension(pathToSolutionFile);
            return extension != null && extension.ToUpper().Equals(".SLN");
        }

        private static bool FileIsProject(string pathToSolutionFile)
        {
            var extension = Path.GetExtension(pathToSolutionFile);
            return extension != null && extension.ToUpper().Equals(".CSPROJ");
        }
    }
}
