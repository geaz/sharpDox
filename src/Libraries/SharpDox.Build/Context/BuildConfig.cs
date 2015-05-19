using SharpDox.Build.Context.Step;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using System.Collections.Generic;

namespace SharpDox.Build.Context
{
    internal static class BuildConfig
    {
        public static List<StepBase> FullBuildConfig(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            var stepInput = new StepInput(configController, codeParser, sdBuildStrings, allExporters);

            var config = new List<StepBase>();
            var checkConfig = new CheckConfigStep(stepInput, 0, 15);

            config.Add(new ExtendedCheckConfigStep(stepInput, checkConfig, 0, 15));
            config.Add(new ParseProjectStep(stepInput, 15, 25));
            config.Add(new ParseCodeStep(stepInput, 25, 70));
            config.Add(new ExportStep(stepInput, 70, 100));

            return config;
        }

        public static List<StepBase> StructureParseConfig(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            var stepInput = new StepInput(configController, codeParser, sdBuildStrings, allExporters);

            var config = new List<StepBase>();

            config.Add(new CheckConfigStep(stepInput, 0, 15));
            config.Add(new ParseProjectStep(stepInput, 15, 25));
            config.Add(new StructeParseCodeStep(stepInput, 25, 100));

            return config;
        }
    }
}
