using System.IO;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Build.Context.Step
{
    internal class PreBuildStep
    {
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly IExporter[] _allExporters;
        private readonly BuildMessenger _buildMessenger;
        private readonly SDBuildStrings _sdBuildStrings;

        public PreBuildStep(ICoreConfigSection coreConfigSection, IExporter[] allExporters, BuildMessenger buildMessenger, SDBuildStrings sdBuildStrings)
        {
            _coreConfigSection = coreConfigSection;
            _allExporters = allExporters;
            _buildMessenger = buildMessenger;
            _sdBuildStrings = sdBuildStrings;
        }

        public void CheckConfig(bool justParse = true)
        {
            if (string.IsNullOrEmpty(_coreConfigSection.ProjectName))
                throw new SDBuildException(_sdBuildStrings.NoProjectNameGiven);

            if (string.IsNullOrEmpty(_coreConfigSection.InputPath))
                throw new SDBuildException(_sdBuildStrings.NoProjectGiven);

            if (!File.Exists(_coreConfigSection.InputPath))
                throw new SDBuildException(_sdBuildStrings.ProjectNotFound);

            if (!justParse)
            {
                if (string.IsNullOrEmpty(_coreConfigSection.DocLanguage))
                    throw new SDBuildException(_sdBuildStrings.NoDocLanguage);

                if (string.IsNullOrEmpty(_coreConfigSection.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.NoOutputPathGiven);

                if (!Directory.Exists(_coreConfigSection.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.OutputPathNotFound);
                
                foreach (var exporter in _allExporters)
                {
                    if (_coreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                    {
                        exporter.OnRequirementsWarning += (m) => _buildMessenger.ExecuteOnBuildMessage(m);
                        if (!exporter.CheckRequirements())
                        {
                            throw new SDBuildException(_sdBuildStrings.RequirementError);
                        }
                    }
                }
            }
        }
    }
}
