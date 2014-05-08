using SharpDox.Build.Context.Step;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using System;
using System.Collections.Generic;

namespace SharpDox.Build.Context
{
    internal static class BuildConfig
    {
        public static List<StepBase> FullBuildConfig(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            StepInput.InitStepinput(configController, codeParser, sdBuildStrings, allExporters);

            var config = new List<StepBase>();
            var checkConfig = new CheckConfigStep(0, 15);

            config.Add(new ExtendedCheckConfigStep(checkConfig, 0, 15));
            config.Add(new ParseProjectStep(15, 25));
            config.Add(new ParseCodeStep(25, 90));
            config.Add(new EndStep(90, 100));

            return config;
        }

        public static List<StepBase> StructureParseConfig(IConfigController configController, ICodeParser codeParser, SDBuildStrings sdBuildStrings, IExporter[] allExporters)
        {
            StepInput.InitStepinput(configController, codeParser, sdBuildStrings, allExporters);

            var config = new List<StepBase>();

            config.Add(new CheckConfigStep(0, 15));
            config.Add(new ParseProjectStep(15, 25));
            config.Add(new StructeParseCodeStep(25, 100));

            return config;
        }
    }
}
