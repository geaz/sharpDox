using SharpDox.Build.Context.Step;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context
{
    internal class Steps
    {
        public Steps(ICoreConfigSection coreConfigSection, SDBuildStrings sdBuildStrings, IConfigController configController, BuildMessenger buildMessenger, IExporter[] allExporters = null)
        {
            PreBuildStep = new PreBuildStep(coreConfigSection, allExporters, buildMessenger, sdBuildStrings);
            LoadStep = new LoadStep(coreConfigSection, sdBuildStrings, buildMessenger);
            ParseStep = new ParseStep(sdBuildStrings, coreConfigSection, buildMessenger);
            StructureParseStep = new StructureParseStep(sdBuildStrings, buildMessenger);
            ExportStep = new ExportStep(coreConfigSection, sdBuildStrings, buildMessenger, allExporters);
            EndStep = new EndStep(configController, coreConfigSection);
        }

        public PreBuildStep PreBuildStep { get; set; }
        public LoadStep LoadStep { get; set; }
        public ParseStep ParseStep { get; set; }
        public StructureParseStep StructureParseStep { get; set; }
        public ExportStep ExportStep { get; set; }
        public EndStep EndStep { get; set; }
    }
}
