using SharpDox.Build.Context.Step;
using SharpDox.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SharpDox.Build.Context
{
    internal class BuildContext
    {
        private readonly List<StepBase> _buildSteps;
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;

        public BuildContext(BuildMessenger buildMessenger, SDBuildStrings sdBuildStrings, List<StepBase> buildSteps)
        {
            _buildSteps = buildSteps;
            _buildMessenger = buildMessenger;
            _sdBuildStrings = sdBuildStrings;
        }

        public virtual void StartBuild()
        {
            try
            {
                _buildMessenger.ExecuteOnBuildProgress(0);
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.StartingBuild);

                var sdProject = new SDProject();
                foreach (var step in _buildSteps)
                {
                    _buildMessenger.ExecuteOnBuildMessage(string.Format(_sdBuildStrings.StartingStep, step.StepName));
                    _buildMessenger.ExecuteOnBuildProgress(step.StepRange.ProgressStart);
                    _buildMessenger.ExecuteOnStepProgress(0);

                    step.OnBuildProgress += _buildMessenger.ExecuteOnBuildProgress;
                    step.OnBuildMessage += _buildMessenger.ExecuteOnBuildMessage;
                    step.OnStepMessage += _buildMessenger.ExecuteOnStepMessage;
                    step.OnStepProgress += _buildMessenger.ExecuteOnStepProgress;

                    sdProject = step.RunStep(sdProject);

                    _buildMessenger.ExecuteOnStepProgress(100);
                }

                _buildMessenger.ExecuteOnBuildProgress(100);
                _buildMessenger.ExecuteOnStepMessage(string.Empty);
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildSuccess);
                _buildMessenger.ExecuteOnBuildCompleted(sdProject);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildStopped);
                    _buildMessenger.ExecuteOnStepProgress(0);
                    _buildMessenger.ExecuteOnBuildProgress(0);

                    _buildMessenger.ExecuteOnBuildStopped();
                }
                else
                {
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndBuild);
                    _buildMessenger.ExecuteOnStepProgress(100);
                    _buildMessenger.ExecuteOnBuildProgress(100);

                    _buildMessenger.ExecuteOnBuildFailed(ex.ToString());
                }
            }
        }
    }
}