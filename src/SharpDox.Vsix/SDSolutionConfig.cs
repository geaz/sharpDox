using System;
using System.IO;
using System.Linq;
using EnvDTE;

namespace SharpDox.Vsix
{
    internal class SDSolutionConfig
    {
        public SDSolutionConfig(Solution solution)
        {
            SolutionFile = solution.FileName;
            SDoxFile = Directory.GetFiles(SolutionFile, "*.sdox").First();
            LoadStartupAssemblyInfo(solution);
        }

        private void LoadStartupAssemblyInfo(Solution solution)
        {
            var sb = solution.SolutionBuild;
            string msg = "";

            foreach (var item in (Array)sb.StartupProjects)
            {
                msg += item;
            }
            var startupProj = solution.Item(msg);

            var assemblyInfoProjectItem = startupProj.ProjectItems.Item("Properties").ProjectItems.Item(1);
            var assemblyInfoFileCodeModel = assemblyInfoProjectItem.FileCodeModel;
            var l = "";
        }

        public string SolutionFile { get; set; }

        public string SDoxFile { get; private set; }
    }
}
