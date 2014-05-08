using System.IO;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Config;
using SharpDox.Model;
using System;

namespace SharpDox.Build.Context.Step
{
    internal class ExportStep : StepBase
    {
        public ExportStep(int progressStart, int progressEnd) :
            base(StepInput.SDBuildStrings.StepExport, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            RunAllExporters(sdProject);

            return sdProject;
        }

        private void RunAllExporters(SDProject sdProject)
        {
            var i = 0;
            foreach (var exporter in StepInput.AllExporters)
            {
                if (StepInput.CoreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                {
                    var outputPath = GetOutputPath(StepInput.CoreConfigSection.OutputPath, exporter.ExporterName);

                    exporter.OnStepMessage += ExecuteOnStepMessage;
                    exporter.OnStepProgress += (p) => ExecuteOnStepProgress((int)(((double)p / StepInput.AllExporters.Length) + (i / StepInput.AllExporters.Length * 100)));
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
