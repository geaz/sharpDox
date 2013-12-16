using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class EndStep
    {
        private readonly SharpDoxConfig _sharpDoxConfig;
        private readonly IConfigController _configController;

        public EndStep(IConfigController configController, SharpDoxConfig sharpDoxConfig)
        {
            _configController = configController;
            _sharpDoxConfig = sharpDoxConfig;
        }

        public void EndProcess()
        {
            _sharpDoxConfig.LastBuild = DateTime.Now.ToString("d.M.yyyy - HH:mm");
            _configController.Save();
        }
    }
}
