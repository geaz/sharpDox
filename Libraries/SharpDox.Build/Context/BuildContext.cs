using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.Build.Context
{
    internal class BuildContext
    {
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;
        private readonly ICoreConfigSection _coreConfigSection;
        private readonly Steps _steps;

        public BuildContext(ICoreConfigSection coreConfigSection, SDBuildStrings sdBuildStrings, IConfigController configController, BuildMessenger buildMessenger, IExporter[] allExporters)
        {
            _coreConfigSection = coreConfigSection;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;

            _steps = new Steps(coreConfigSection, sdBuildStrings, configController, buildMessenger, allExporters);
        }

        public virtual void BuildDocumentation()
        {
            try
            {
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.StartingBuild);

                _buildMessenger.ExecuteOnBuildProgress(0);
                _steps.PreBuildStep.CheckConfig(false);

                _buildMessenger.ExecuteOnBuildProgress(10);
                var solution = _steps.LoadStep.LoadSolution();

                _buildMessenger.ExecuteOnBuildProgress(30);
                var repository = _steps.ParseStep.ParseSolution(solution, _coreConfigSection.ExcludedIdentifiers.ToList());

                _buildMessenger.ExecuteOnBuildProgress(60);
                _steps.ExportStep.ExportSolution(repository);

                _buildMessenger.ExecuteOnBuildProgress(90);
                _steps.EndStep.EndProcess();

                _buildMessenger.ExecuteOnBuildProgress(100);
                _buildMessenger.ExecuteOnStepMessage(string.Empty);
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildSuccess);
                _buildMessenger.ExecuteOnBuildCompleted();
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildStopped);
                    _buildMessenger.ExecuteOnBuildStopped();

                    _buildMessenger.ExecuteOnStepProgress(0);
                    _buildMessenger.ExecuteOnBuildProgress(0);
                }
                else
                {
                    Trace.TraceError(ex.ToString());                    

                    if (ex is SDBuildException)
                    {
                        _buildMessenger.ExecuteOnBuildMessage(ex.Message);
                    }

                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndBuild);

                    _buildMessenger.ExecuteOnBuildFailed();
                    _buildMessenger.ExecuteOnStepProgress(100);
                    _buildMessenger.ExecuteOnBuildProgress(100);
                }
            }
        }
    }
}