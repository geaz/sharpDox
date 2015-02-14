using System;
using SharpDox.Build;
using SharpDox.Sdk.Config;

namespace SharpDox.Console
{
    public class SDConsole
    {
        private readonly Func<BuildController> _builderFactory;
        private readonly IConfigController _configController;

        private BuildController _buildController;
        private ConsoleArguments _arguments;
        private readonly BuildMessenger _buildMessenger;
        private readonly SDConsoleStrings _strings;

        public SDConsole(SDConsoleStrings strings, IConfigController configController, BuildMessenger buildMessenger, Func<BuildController> builderFactory)
        {
            _strings = strings;
            _buildMessenger = buildMessenger;
            _configController = configController;
            _builderFactory = builderFactory;
        }

        public void Start(string[] args)
        {
            _arguments = new ConsoleArguments(args);

            if (_arguments["config"] != null)
            {
                _configController.Load(_arguments["config"]);
                _buildController = _builderFactory();

                _buildMessenger.OnBuildMessage += System.Console.WriteLine;

                _buildController.StartBuild(_configController.GetConfigSection<ICoreConfigSection>(), false);
            }
            else
            {
                System.Console.WriteLine(_strings.ConfigMissing + " -config " + _strings.Path);
            }

            System.Console.WriteLine(_strings.PressToEnd);
            System.Console.ReadLine();
        }
        
        public bool IsGui { get { return false; } }
    }
}
