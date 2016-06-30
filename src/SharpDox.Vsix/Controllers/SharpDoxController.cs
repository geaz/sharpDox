using System;
using EnvDTE;
using SharpDox.Build;
using SharpDox.Sdk.Config;

namespace SharpDox.Vsix.Controllers
{
    internal class SharpDoxController
    {
        private readonly OutputController _outputController;
        private readonly IConfigController _configController;
        private readonly BuildMessenger _buildMessenger;
        private readonly Func<BuildController> _builderFactory;

        public SharpDoxController(OutputController outputController, IConfigController configController, BuildMessenger buildMessenger, Func<BuildController> builderFactory)
        {
            _builderFactory = builderFactory;
            _buildMessenger = buildMessenger;
            _configController = configController;
            _outputController = outputController;
        }

        public void BuildDocumentation(Solution solution)
        {
            if (solution == null)
            {
                _outputController.WriteOutputLine("No solution loaded. Please load a solution before building a documentation!");
                return;
            }

            var sdSolutionConfig = new SDSolutionConfig(solution);
        }
    }
}
