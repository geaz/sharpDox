using SharpDox.Model;
using System;
using System.IO;

namespace SharpDox.Build.Context.Step
{
    internal class ExtendedCheckConfigStep : StepBase
    {
        private readonly CheckConfigStep _checkConfigStep;

        public ExtendedCheckConfigStep(CheckConfigStep checkConfigStep, int progressStart, int progressEnd) : 
            base(StepInput.SDBuildStrings.StepCheckConfig, new StepRange(progressStart, progressEnd)) 
        { 
            _checkConfigStep = checkConfigStep;
        }

        public override SDProject RunStep(SDProject sdProject)
        {
            if (string.IsNullOrEmpty(StepInput.CoreConfigSection.DocLanguage))
                throw new SDBuildException(StepInput.SDBuildStrings.NoDocLanguage);

            if (string.IsNullOrEmpty(StepInput.CoreConfigSection.OutputPath))
                throw new SDBuildException(StepInput.SDBuildStrings.NoOutputPathGiven);

            if (!Directory.Exists(StepInput.CoreConfigSection.OutputPath))
                throw new SDBuildException(StepInput.SDBuildStrings.OutputPathNotFound);

            foreach (var exporter in StepInput.AllExporters)
            {
                if (StepInput.CoreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                {
                    exporter.OnRequirementsWarning += (m) => ExecuteOnBuildMessage(m);
                    if (!exporter.CheckRequirements())
                    {
                        throw new SDBuildException(StepInput.SDBuildStrings.RequirementError);
                    }
                }
            }

            return _checkConfigStep.RunStep(sdProject);
        }
    }
}
