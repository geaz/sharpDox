using SharpDox.Build.Context.Step;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context
{
    internal class Steps
    {
        public Steps(SharpDoxConfig sharpDoxConfig, SDBuildStrings sdBuildStrings, IConfigController configController, BuildMessenger buildMessenger, IExporter[] allExporters = null)
        {
            PreBuildStep = new PreBuildStep(sharpDoxConfig, allExporters, buildMessenger, sdBuildStrings);
            LoadStep = new LoadStep(sharpDoxConfig, sdBuildStrings, buildMessenger);
            ParseStep = new ParseStep(sdBuildStrings, sharpDoxConfig, buildMessenger);
            StructureParseStep = new StructureParseStep(sdBuildStrings, buildMessenger);
            ExportStep = new ExportStep(sharpDoxConfig, sdBuildStrings, buildMessenger, allExporters);
            EndStep = new EndStep(configController, sharpDoxConfig);
        }

        public PreBuildStep PreBuildStep { get; set; }
        public LoadStep LoadStep { get; set; }
        public ParseStep ParseStep { get; set; }
        public StructureParseStep StructureParseStep { get; set; }
        public ExportStep ExportStep { get; set; }
        public EndStep EndStep { get; set; }
    }
}
