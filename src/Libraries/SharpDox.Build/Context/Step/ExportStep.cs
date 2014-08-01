using SharpDox.Model;
using System.IO;

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
            var i = 0d;
            foreach (var exporter in _stepInput.AllExporters)
            {
                if (_stepInput.CoreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                {
                    var outputPath = GetOutputPath(_stepInput.CoreConfigSection.OutputPath, exporter.ExporterName);

                    ExecuteOnStepMessage(string.Format(_stepInput.SDBuildStrings.RunningExporter, exporter.ExporterName));
                    exporter.OnStepMessage += (m) => ExecuteOnStepMessage(string.Format("[{0}] {1}", exporter.ExporterName, m));
                    exporter.OnStepProgress += (p) => ExecuteOnStepProgress((int)(((double)p / _stepInput.CoreConfigSection.ActivatedExporters.Count) + (i / _stepInput.CoreConfigSection.ActivatedExporters.Count * 100)));
                    exporter.Export(sdProject, outputPath);
                    i++;
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
