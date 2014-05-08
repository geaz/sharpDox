using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Build.Context.Step
{
    internal static class StepInput
    {
        public static void InitStepinput(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            ConfigController = configController;            
            CoreConfigSection = configController.GetConfigSection<ICoreConfigSection>();
            CodeParser = codeParser;
            SDBuildStrings = sdBuildStrings;
            AllExporters = allExporters;
        }

        public static IConfigController ConfigController { get; set; }
        public static ICoreConfigSection CoreConfigSection { get; set; }
        public static ICodeParser CodeParser { get; set; }
        public static SDBuildStrings SDBuildStrings { get; set; }
        public static IExporter[] AllExporters { get; set; }
    }
}
