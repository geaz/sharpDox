using SharpDox.Model;
using System;
using System.IO;

namespace SharpDox.Build.Context.Step
{
    internal class ExtendedCheckConfigStep : StepBase
    {
        private readonly CheckConfigStep _checkConfigStep;

        public ExtendedCheckConfigStep(StepInput stepInput, CheckConfigStep checkConfigStep, int progressStart, int progressEnd) : 
            base(stepInput, stepInput.SDBuildStrings.StepCheckConfig, new StepRange(progressStart, progressEnd)) 
        { 
            _checkConfigStep = checkConfigStep;
        }

        public override SDProject RunStep(SDProject sdProject)
        {
            if (string.IsNullOrEmpty(_stepInput.CoreConfigSection.DocLanguage))
                throw new SDBuildException(_stepInput.SDBuildStrings.NoDocLanguage);

            if (_stepInput.CoreConfigSection.OutputPath == null)
                throw new SDBuildException(_stepInput.SDBuildStrings.NoOutputPathGiven);

            if (!Directory.Exists(_stepInput.CoreConfigSection.OutputPath))
            {
                Directory.CreateDirectory(_stepInput.CoreConfigSection.OutputPath);
            }

            foreach (var exporter in _stepInput.AllExporters)
            {
                if (_stepInput.CoreConfigSection.ActivatedExporters.Contains(exporter.ExporterName))
                {
                    exporter.OnRequirementsWarning += (m) => ExecuteOnBuildMessage(m);
                    if (!exporter.CheckRequirements())
                    {
                        throw new SDBuildException(_stepInput.SDBuildStrings.RequirementError);
                    }
                }
            }

            return _checkConfigStep.RunStep(sdProject);
        }
    }
}
