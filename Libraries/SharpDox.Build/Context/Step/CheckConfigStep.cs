using SharpDox.Model;
using SharpDox.Sdk.Config;
using System;
using System.IO;

namespace SharpDox.Build.Context.Step
{
    internal class CheckConfigStep : StepBase
    {
        public CheckConfigStep(int progressStart, int progressEnd) : 
            base(StepInput.SDBuildStrings.StepCheckConfig, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            if (string.IsNullOrEmpty(StepInput.CoreConfigSection.ProjectName))
                throw new SDBuildException(StepInput.SDBuildStrings.NoProjectNameGiven);

            if (string.IsNullOrEmpty(StepInput.CoreConfigSection.InputFile))
                throw new SDBuildException(StepInput.SDBuildStrings.NoProjectGiven);

            if (!File.Exists(StepInput.CoreConfigSection.InputFile))
                throw new SDBuildException(StepInput.SDBuildStrings.ProjectNotFound);

            return sdProject;
        }
    }
}
