using System.IO;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Context.Step
{
    internal class ExportStep
    {
        private readonly IExporter[] _allExporters;
        private readonly SharpDoxConfig _sharpDoxConfig;
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;

        public ExportStep(SharpDoxConfig sharpDoxConfig, SDBuildStrings sdBuildStrings, BuildMessenger buildMessenger, IExporter[] allExporters)
        {
            _sharpDoxConfig = sharpDoxConfig;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;
            _allExporters = allExporters;
        }

        public void ExportSolution(SDRepository repository)
        {
            _buildMessenger.ExecuteOnStepProgress(0);
            _buildMessenger.ExecuteOnStepMessage(string.Empty);

            RunAllExporters(repository);

            _buildMessenger.ExecuteOnStepProgress(100);
        }

        private void RunAllExporters(SDRepository repository)
        {
            var i = 0;
            foreach (var exporter in _allExporters)
            {
                if (!_sharpDoxConfig.DeactivatedExporters.Contains(exporter.ExporterName))
                {
                    _buildMessenger.ExecuteOnBuildMessage(string.Format(_sdBuildStrings.StartExporter + ": \"{0}\" ...", exporter.ExporterName));

                    var outputPath = GetOutputPath(_sharpDoxConfig.OutputPath, exporter.ExporterName);

                    exporter.OnStepMessage += (m) => _buildMessenger.ExecuteOnStepMessage(m);
                    exporter.OnStepProgress += (p) => _buildMessenger.ExecuteOnStepProgress(p);
                    exporter.Export(repository, outputPath);
                }
            }
        }

        private string GetOutputPath(string basePath, string exporterName)
        {
            var outputPath = Path.Combine(basePath, exporterName);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            return outputPath;
        }
    }
}
