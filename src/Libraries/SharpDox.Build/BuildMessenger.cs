using SharpDox.Model;
using System;

namespace SharpDox.Build
{
    public class BuildMessenger
    {
        public event Action<string> OnBuildMessage;
        public event Action<string> OnStepMessage;

        public event Action<int> OnBuildProgress;
        public event Action<int> OnStepProgress;

        public event Action<SDProject> OnBuildCompleted;
        public event Action OnBuildFailed;
        public event Action OnBuildStopped;

        internal void ExecuteOnBuildMessage(string message)
        {
            if (OnBuildMessage != null)
            {
                OnBuildMessage(message);
            }
        }

        internal void ExecuteOnStepMessage(string message)
        {
            if (OnStepMessage != null)
            {
                OnStepMessage(message);
            }
        }

        internal void ExecuteOnBuildProgress(int value)
        {
            if (OnBuildProgress != null)
            {
                OnBuildProgress(value);
            }
        }

        internal void ExecuteOnStepProgress(int value)
        {
            if (OnStepProgress != null)
            {
                OnStepProgress(value);
            }
        }

        internal void ExecuteOnBuildStopped()
        {
            if (OnBuildStopped != null)
            {
                OnBuildStopped();
            }
            ExecuteOnStepMessage("");
        }

        internal void ExecuteOnBuildFailed(string errorMessage)
        {
            if (OnBuildFailed != null)
            {
                OnBuildFailed();
            }
            ExecuteOnBuildMessage(errorMessage);
        }

        internal void ExecuteOnBuildCompleted(SDProject sdProject)
        {
            if (OnBuildCompleted != null)
            {
                OnBuildCompleted(sdProject);
            }
            ExecuteOnStepMessage("");
        }
    }
}