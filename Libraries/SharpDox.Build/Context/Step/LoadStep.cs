using System.Linq;
using SharpDox.Build.Loader;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class LoadStep
    {
        private readonly SharpDoxConfig _config;
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;

        public LoadStep(SharpDoxConfig config, SDBuildStrings sdBuildStrings, BuildMessenger buildMessenger)
        {
            _config = config;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;
        }

        public CSharpSolution LoadSolution()
        {
            _buildMessenger.ExecuteOnStepProgress(0);
            _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.LoadingSolution);

            var solution = new CSharpSolution(_sdBuildStrings, _buildMessenger);
            solution.LoadSolution(_config.InputPath);
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
