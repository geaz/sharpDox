using SharpDox.Build.Context;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using System;
using System.Threading;

namespace SharpDox.Build
{
    public class BuildController : IBuildController
    {
        private Thread _buildThread;

        private readonly SDBuildStrings _sdBuildStrings;
        private readonly IConfigController _configController;
        private readonly Func<ICodeParser> _codeParser;
        private readonly IExporter[] _allExporters;

        public BuildController(IBuildMessenger buildMessenger, IConfigController configController, Func<ICodeParser> codeParser, IExporter[] allExporters, SDBuildStrings sdBuildStrings)
        {
            BuildMessenger = buildMessenger;
            _configController = configController;
            _codeParser = codeParser;
            _allExporters = allExporters;
            _sdBuildStrings = sdBuildStrings;
        }

        public void StartParse(ICoreConfigSection coreConfigSection, bool thread)
        {
            var config = BuildConfig.StructureParseConfig(_configController, _codeParser(), _sdBuildStrings, _allExporters);
            var context = new BuildContext(BuildMessenger as BuildMessenger, _sdBuildStrings, config);

            if (thread)
            {
                Stop();
                _buildThread = new Thread(context.StartBuild);
                _buildThread.Start();
            }
            else
            {
                context.StartBuild();
            }
        }

        public void StartBuild(ICoreConfigSection coreConfigSection, bool thread)
        {
            var config = BuildConfig.FullBuildConfig(_configController, _codeParser(), _sdBuildStrings, _allExporters);
            var context = new BuildContext(BuildMessenger as BuildMessenger, _sdBuildStrings, config);

            if (thread)
            {
                Stop();
                _buildThread = new Thread(context.StartBuild);
                _buildThread.Start();
            }
            else
            {
                context.StartBuild();
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