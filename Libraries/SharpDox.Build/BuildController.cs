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
        private readonly IExporter[] _allExporters;
        private readonly IConfigController _configController;

        public BuildController(SDBuildStrings sdBuildStrings, IConfigController configController, IExporter[] allExporters, IBuildMessenger buildMessenger)
        {
            _configController = configController;
            _sdBuildStrings = sdBuildStrings;
            _allExporters = allExporters;

            BuildMessenger = buildMessenger;
        }

        public void StartParse(ICoreConfigSection coreConfigSection, bool thread)
        {
            var parseContext = new ParseContext(coreConfigSection, _sdBuildStrings, _configController, BuildMessenger as BuildMessenger);

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
            var buildContext = new BuildContext(sharpDoxConfig, _sdBuildStrings, _configController, BuildMessenger as BuildMessenger, _allExporters);

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

        public IBuildMessenger BuildMessenger { get; private set; }
    }
}