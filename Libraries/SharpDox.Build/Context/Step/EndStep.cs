using System;
using SharpDox.Model;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class EndStep : StepBase
    {
        public EndStep(int progressStart, int progressEnd) :
            base(StepInput.SDBuildStrings.StepEnd, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            StepInput.ConfigController.GetConfigSection<ICoreConfigSection>().LastBuild = DateTime.Now.ToString("d.M.yyyy - HH:mm");
            StepInput.ConfigController.Save();

            return sdProject;
        }
    }
}
