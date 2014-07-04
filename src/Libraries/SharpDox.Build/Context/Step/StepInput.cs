using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Build.Context.Step
{
    internal class StepInput
    {
        public StepInput(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            ConfigController = configController;            
            CoreConfigSection = configController.GetConfigSection<ICoreConfigSection>();
            CodeParser = codeParser;
            SDBuildStrings = sdBuildStrings;
            AllExporters = allExporters;
        }

        public IConfigController ConfigController { get; set; }
        public ICoreConfigSection CoreConfigSection { get; set; }
        public ICodeParser CodeParser { get; set; }
        public SDBuildStrings SDBuildStrings { get; set; }
        public IExporter[] AllExporters { get; set; }
    }
}
