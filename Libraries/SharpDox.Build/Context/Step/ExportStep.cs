using System.IO;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;
using SharpDox.Model;
using System;

namespace SharpDox.Build.Context.Step
{
    internal class ExportStep : StepBase
    {
        public ExportStep(StepInput stepInput, int progressStart, int progressEnd) :
            base(stepInput, stepInput.SDBuildStrings.StepExport, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            RunAllExporters(sdProject);

            return sdProject;
        }

        private void RunAllExporters(SDProject sdProject)
        {
            var i = 0;
            foreach (var exporter in _stepInput.AllExporters)
            {
                if (_stepInput.CoreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                {
                    var outputPath = GetOutputPath(_stepInput.CoreConfigSection.OutputPath, exporter.ExporterName);

                    exporter.OnStepMessage += (m) => ExecuteOnStepMessage(string.Format("[{0}] {1}", exporter.ExporterName, m));
                    exporter.OnStepProgress += (p) => ExecuteOnStepProgress((int)(((double)p / _stepInput.AllExporters.Length) + (i / _stepInput.AllExporters.Length * 100)));
                    exporter.Export(sdProject, outputPath);
                }
                i++;
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
