using System;
using SharpDox.ConsoleHelper;
using SharpDox.Sdk.Local;
using SharpDox.Sdk.UI;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;

namespace SharpDox.Console
{
    public class Main : IShell
    {
        private readonly Func<IBuildController> _builderFactory;
        private readonly IConfigController _configController;

        private IBuildController _buildController;
        private ConsoleArguments _arguments;
        private readonly IBuildMessenger _buildMessenger;
        private readonly SDConsoleStrings _strings;

        public Main(SDConsoleStrings strings, IConfigController configController, IBuildMessenger buildMessenger, Func<IBuildController> builderFactory)
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
