using System;
using SharpDox.Model;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class EndStep : StepBase
    {
        public EndStep(StepInput stepInput, int progressStart, int progressEnd) :
            base(stepInput, stepInput.SDBuildStrings.StepEnd, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            _stepInput.ConfigController.GetConfigSection<ICoreConfigSection>().LastBuild = DateTime.Now.ToString("d.M.yyyy - HH:mm");
            _stepInput.ConfigController.Save();

            return sdProject;
        }
    }
}
