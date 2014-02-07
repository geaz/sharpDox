using System.Threading;
using SharpDox.Build.Context;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;

namespace SharpDox.Build
{
    public class BuildController : IBuildController
    {
        private Thread _buildThread;

        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;
        private readonly IExporter[] _allExporters;
        private readonly IConfigController _configController;

        public BuildController(SDBuildStrings sdBuildStrings, IConfigController configController, IBuildMessenger buildMessenger, IExporter[] allExporters)
        {
            _configController = configController;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = (BuildMessenger)buildMessenger;
            _allExporters = allExporters;
        }

        public void StartParse(ICoreConfigSection coreConfigSection, bool thread)
        {
            var parseContext = new ParseContext(coreConfigSection, _sdBuildStrings, _configController, _buildMessenger);

            if (thread)
            {
                Stop();
                _buildThread = new Thread(parseContext.ParseSolution);
                _buildThread.Start();
            }
            else
            {
                parseContext.ParseSolution();
            }
        }

        public void StartBuild(ICoreConfigSection sharpDoxConfig, bool thread)
        {
            var buildContext = new BuildContext(sharpDoxConfig, _sdBuildStrings, _configController, _buildMessenger, _allExporters);

            if (thread)
            {
                Stop();
                _buildThread = new Thread(buildContext.BuildDocumentation);
                _buildThread.Start();
            }
            else
            {
                buildContext.BuildDocumentation();
            }
        }

        public void Stop()
        {
            if (_buildThread != null)
            {
                _buildThread.Abort();
                _buildThread = null;
            }
        }
    }
}