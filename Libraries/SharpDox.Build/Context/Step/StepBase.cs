using System;
using SharpDox.Model;

namespace SharpDox.Build.Context.Step
{
    internal abstract class StepBase
    {
        public event Action<string> OnBuildMessage;
        public event Action<string> OnStepMessage;
        public event Action<int> OnBuildProgress;
        public event Action<int> OnStepProgress;

        protected StepBase(string stepName, StepRange stepRange)
        {
            StepName = stepName;
            StepRange = stepRange;
        }

        public abstract SDProject RunStep(SDProject sdProject);

        protected void ExecuteOnBuildMessage(string message)
        {
            if (OnBuildMessage != null)
            {
                OnBuildMessage(message);
            }
        }

        protected void ExecuteOnStepMessage(string message)
        {
            if (OnStepMessage != null)
            {
                OnStepMessage(message);
            }
        }

        protected void ExecuteOnStepProgress(int value)
        {
            if (OnStepProgress != null)
            {
                ExecuteOnBuildProgress(StepRange.GetProgressByStepProgress(value));
                OnStepProgress(value);
            }
        }

        private void ExecuteOnBuildProgress(int value)
        {
            if (OnBuildProgress != null)
            {
                OnBuildProgress(value);
            }
        }

        public string StepName { private set; get; }
        public StepRange StepRange { private set; get; }
    }
}
