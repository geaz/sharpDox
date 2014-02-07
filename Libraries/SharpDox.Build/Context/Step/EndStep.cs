using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class EndStep
    {
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly IConfigController _configController;

        public EndStep(IConfigController configController, ICoreConfigSection coreConfigSection)
        {
            _configController = configController;
            _coreConfigSection = coreConfigSection;
        }

        public void EndProcess()
        {
            _coreConfigSection.LastBuild = DateTime.Now.ToString("d.M.yyyy - HH:mm");
            _configController.Save();
        }
    }
}
