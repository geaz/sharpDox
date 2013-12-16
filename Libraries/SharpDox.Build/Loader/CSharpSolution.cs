using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Utils;
using Microsoft.Build.Evaluation;

namespace SharpDox.Build.Loader
{
    internal class CSharpSolution
    {
        private readonly ConcurrentDictionary<string, IUnresolvedAssembly> _assemblyDict =
                new ConcurrentDictionary<string, IUnresolvedAssembly>(Platform.FileNameComparer);

        private readonly Regex _projectLinePattern =
                    new Regex("Project\\(\"(?<TypeGuid>.*)\"\\)\\s+=\\s+\"(?<Title>.*)\",\\s*\"(?<Location>.*)\",\\s*\"(?<Guid>.*)\"");

        private readonly SDBuildStrings _strings;
        private readonly BuildMessenger _buildMessenger;

        public CSharpSolution(SDBuildStrings strings, BuildMessenger buildMessenger)
        {
            _buildMessenger = buildMessenger;
            _strings = strings;

            Projects = new List<CSharpProject>();
        }

        public void LoadSolution(string pathToSolutionFile)
        {
            Directory = Path.GetDirectoryName(pathToSolutionFile);
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            if (FileIsSolution(pathToSolutionFile))
            {
                LoadSolutionFile(pathToSolutionFile);
            }
            else if (FileIsProject(pathToSolutionFile))
            {
                LoadProjectFile(Path.GetFileName(pathToSolutionFile), Path.GetDirectoryName(pathToSolutionFile));
            }

            LoadProjectCompilations();
        }

        public IUnresolvedAssembly LoadAssembly(string assemblyFileName)
        {
            return _assemblyDict.GetOrAdd(assemblyFileName, file => new CecilLoader().LoadAssemblyFile(file));
        }

        public IEnumerable<CSharpFile> AllFiles
        {
            get { return Projects.SelectMany(p => p.Files); }
        }

        public CSharpFile GetFile(string fileName)
        {
            return AllFiles.First(f => f.FileName == fileName);
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

        private void LoadSolutionFile(string pathToSolutionFile)
        {
            var projectFiles = ExtractProjectFiles(pathToSolutionFile);

            var i = 1;
            foreach (var projectFile in projectFiles)
            {
                LoadProjectFile(projectFile.Key, projectFile.Value);
                PostProgress(projectFiles.Count, i++);
            }
        }

        private Dictionary<string, string> ExtractProjectFiles(string pathToSolutionFile)
        {
            var lines = File.ReadLines(pathToSolutionFile).ToList();
            var projectFiles = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var match = _projectLinePattern.Match(line);
                if (match.Success)
                {
                    var typeGuid = match.Groups["TypeGuid"].Value;
                    var title = match.Groups["Title"].Value;
                    var location = match.Groups["Location"].Value;
                    switch (typeGuid.ToUpperInvariant())
                    {
                        case "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}": // C# project
                            var projectFile = Path.Combine(Directory, location);
                            if (File.Exists(projectFile))
                            {
                                projectFiles.Add(title, projectFile);
                            }
                            break;
                    }
                }
            }
            return projectFiles;
        }

        private void LoadProjectFile(string title, string pathToProjectFile)
        {
            _buildMessenger.ExecuteOnStepMessage(_strings.ReadingProject + ": " + title);

            var project = new CSharpProject(this, title, pathToProjectFile);
            Projects.Add(project);
        }

        private void PostProgress(int projectFilesCount, double currentProjectIndex)
        {
            var percentage = (currentProjectIndex / projectFilesCount) * 35 + 15;
            _buildMessenger.ExecuteOnStepProgress((int)percentage);
        }

        private void LoadProjectCompilations()
        {
            var solutionSnapshot = new DefaultSolutionSnapshot(Projects.Select(p => p.ProjectContent));
            foreach (var project in Projects)
            {
                project.Compilation = solutionSnapshot.GetCompilation(project.ProjectContent);
            }
        }

        public string Directory { get; private set; }
        public List<CSharpProject> Projects { get; private set; }
    }
}