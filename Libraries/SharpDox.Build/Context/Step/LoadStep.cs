using System.Linq;
using SharpDox.Build.Loader;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class LoadStep
    {
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;

        public LoadStep(ICoreConfigSection coreConfigSection, SDBuildStrings sdBuildStrings, BuildMessenger buildMessenger)
        {
            _coreConfigSection = coreConfigSection;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;
        }

        public CSharpSolution LoadSolution()
        {
            _buildMessenger.ExecuteOnStepProgress(0);
            _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.LoadingSolution);

            var solution = new CSharpSolution(_sdBuildStrings, _buildMessenger);
            solution.LoadSolution(_coreConfigSection.InputPath);
            PostSolutionLoadSummary(solution);

            _buildMessenger.ExecuteOnStepProgress(100);

            return solution;
        }

        private void PostSolutionLoadSummary(CSharpSolution solution)
        {
            _buildMessenger.ExecuteOnBuildMessage(
                                                  string.Format(_sdBuildStrings.ProjectsLoaded,
                                                                solution.AllFiles.Sum(f => f.LinesOfCode),
                                                                solution.AllFiles.Sum(f => f.OriginalText.Length) / 1024.0 / 1024.0,
                                                                solution.AllFiles.Count(),
                                                                solution.Projects.Count));
        }
    }
}
