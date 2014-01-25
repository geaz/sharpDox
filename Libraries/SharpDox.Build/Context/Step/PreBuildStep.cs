using System.IO;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Build.Context.Step
{
    internal class PreBuildStep
    {
        private readonly SharpDoxConfig _config;
        private readonly IExporter[] _allExporters;
        private readonly BuildMessenger _buildMessenger;
        private readonly SDBuildStrings _sdBuildStrings;

        public PreBuildStep(SharpDoxConfig config, IExporter[] allExporters, BuildMessenger buildMessenger, SDBuildStrings sdBuildStrings)
        {
            _config = config;
            _allExporters = allExporters;
            _buildMessenger = buildMessenger;
            _sdBuildStrings = sdBuildStrings;
        }

        public void CheckConfig(bool justParse = true)
        {
            if (string.IsNullOrEmpty(_config.ProjectName))
                throw new SDBuildException(_sdBuildStrings.NoProjectNameGiven);

            if (string.IsNullOrEmpty(_config.InputPath))
                throw new SDBuildException(_sdBuildStrings.NoProjectGiven);

            if (!File.Exists(_config.InputPath))
                throw new SDBuildException(_sdBuildStrings.ProjectNotFound);

            if (!justParse)
            {
                if (string.IsNullOrEmpty(_config.DocLanguage))
                    throw new SDBuildException(_sdBuildStrings.NoDocLanguage);

                if (string.IsNullOrEmpty(_config.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.NoOutputPathGiven);

                if (!Directory.Exists(_config.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.OutputPathNotFound);

                foreach (var exporter in _allExporters)
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
