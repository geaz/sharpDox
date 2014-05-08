using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Exceptions;

namespace SharpDox.Build.NRefactory.Loader
{
    internal class CSharpProject
    {
        public CSharpProject(CSharpSolution solution, string title, string fileName)
        {
            Files = new List<CSharpFile>();
            CompilerSettings = new CompilerSettings();
            Solution = solution;
            Title = title;
            FileName = fileName;

            LoadCSharpProject(solution, fileName);
        }

        public CSharpFile GetFile(string fileName)
        {
            return Files.Single(f => f.FileName == fileName);
        }

        public override string ToString()
        {
            return string.Format("[CSharpProject AssemblyName={0}]", AssemblyName);
        }

        private void LoadCSharpProject(CSharpSolution solution, string fileName)
        {
            try
            {
                var msbuildProject = LoadAndInitProject(solution, fileName);

                var pc = CreateCSharpProjectContent(fileName);
                pc = AddCompileFilesToProject(msbuildProject, pc);
                pc = AddAllAssemblyReferences(solution, msbuildProject, pc);
                pc = AddAllProjectReferences(msbuildProject, pc);

                ProjectContent = pc;
            }
            catch (InvalidProjectFileException invalidproject)
            {
                Trace.TraceWarning(invalidproject.ToString());
            }
        }

        private Project LoadAndInitProject(CSharpSolution solution, string fileName)
        {
            var globalProperties = new Dictionary<string, string>();
            globalProperties.Add("SolutionDir", solution.Directory);

            var msbuildProject = 
                new Project(fileName, globalProperties, null, 
                ProjectCollection.GlobalProjectCollection, ProjectLoadSettings.IgnoreMissingImports);

            AssemblyName = msbuildProject.GetPropertyValue("AssemblyName");
            CompilerSettings.AllowUnsafeBlocks = GetBoolProperty(msbuildProject, "AllowUnsafeBlocks") ?? false;
            CompilerSettings.CheckForOverflow = GetBoolProperty(msbuildProject, "CheckForOverflowUnderflow") ?? false;

            var defineConstants = msbuildProject.GetPropertyValue("DefineConstants");
            foreach (var symbol in defineConstants.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                CompilerSettings.ConditionalSymbols.Add(symbol.Trim());

            return msbuildProject;
        }

        private IProjectContent CreateCSharpProjectContent(string fileName)
        {
            IProjectContent pc = new CSharpProjectContent();
            pc = pc.SetAssemblyName(AssemblyName);
            pc = pc.SetProjectFileName(fileName);
            pc = pc.SetCompilerSettings(CompilerSettings);
            return pc;
        }

        private IProjectContent AddCompileFilesToProject(Project msbuildProject, IProjectContent pc)
        {
            foreach (var item in msbuildProject.GetItems("Compile"))
            {
                var filepath = Path.Combine(msbuildProject.DirectoryPath, item.EvaluatedInclude);
                if (File.Exists(filepath))
                {
                    var file = new CSharpFile(this, filepath);
                    Files.Add(file);
                }
            }

            pc = pc.AddOrUpdateFiles(Files.Select(f => f.UnresolvedTypeSystemForFile));
            return pc;
        }

        private IProjectContent AddAllAssemblyReferences(CSharpSolution solution, Project msbuildProject, IProjectContent pc)
        {
            foreach (var assemblyFile in ResolveAssemblyReferences(msbuildProject))
            {
                var assembly = solution.LoadAssembly(assemblyFile);
                pc = pc.AddAssemblyReferences(new[] { assembly });
            }
            return pc;
        }

        private static IProjectContent AddAllProjectReferences(Project msbuildProject, IProjectContent pc)
        {
            foreach (var item in msbuildProject.GetItems("ProjectReference"))
            {
                var referencedFileName = Path.Combine(msbuildProject.DirectoryPath, item.EvaluatedInclude);
                referencedFileName = Path.GetFullPath(referencedFileName);

                pc = pc.AddAssemblyReferences(new[] {new ProjectReference(referencedFileName)});
            }
            return pc;
        }

        private IEnumerable<string> ResolveAssemblyReferences(Project project)
        {
            var projectInstance = project.CreateProjectInstance();
            projectInstance.SetProperty("BuildingProject", "false");
            project.SetProperty("DesignTimeBuild", "true");

            projectInstance.Build("ResolveAssemblyReferences", new[] { new ConsoleLogger(LoggerVerbosity.Minimal) });
            var items = projectInstance.GetItems("_ResolveAssemblyReferenceResolvedFiles");
            var baseDirectory = Path.GetDirectoryName(this.FileName);
            return items.Select(i => Path.Combine(baseDirectory, i.GetMetadataValue("Identity")));
        }

        private static bool? GetBoolProperty(Project p, string propertyName)
        {
            var val = p.GetPropertyValue(propertyName);
            bool result;
            if (bool.TryParse(val, out result))
                return result;
            else
                return null;
        }

        public CSharpSolution Solution { get; private set; }
        public string Title { get; private set; }
        public string AssemblyName { get; private set; }
        public string FileName { get; private set; }
        public List<CSharpFile> Files { get; private set; }
        public CompilerSettings CompilerSettings { get; private set; }
        public IProjectContent ProjectContent { get; private set; }
        public ICompilation Compilation { get; set; }
    }
}
