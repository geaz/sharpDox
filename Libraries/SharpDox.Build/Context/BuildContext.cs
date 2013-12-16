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
        private readonly SharpDoxConfig _sharpDoxConfig;
        private readonly Steps _steps;

        public BuildContext(SharpDoxConfig sharpDoxConfig, SDBuildStrings sdBuildStrings, IConfigController configController, BuildMessenger buildMessenger, IExporter[] allExporters)
        {
            _sharpDoxConfig = sharpDoxConfig;
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;

            _steps = new Steps(sharpDoxConfig, sdBuildStrings, configController, buildMessenger, allExporters);
        }

        public virtual void BuildDocumentation()
        {
            try
            {
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.StartingBuild);

                _buildMessenger.ExecuteOnBuildProgress(0);
                _steps.PreBuildStep.CheckConfig(true);

                _buildMessenger.ExecuteOnBuildProgress(10);
                var solution = _steps.LoadStep.LoadSolution();

                _buildMessenger.ExecuteOnBuildProgress(30);
                var repository = _steps.ParseStep.ParseSolution(solution, _sharpDoxConfig.ExcludedIdentifiers.ToList());

                _buildMessenger.ExecuteOnBuildProgress(60);
                _steps.ExportStep.ExportSolution(repository);

                _buildMessenger.ExecuteOnBuildProgress(90);
                _steps.EndStep.EndProcess();

                _buildMessenger.ExecuteOnBuildProgress(100);
                _buildMessenger.ExecuteOnStepMessage(string.Empty);
                _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildSuccess);
                _buildMessenger.ExecuteOnBuildStopped();
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.BuildStopped);
                    _buildMessenger.ExecuteOnBuildStopped();
                }
                else if (ex is SDBuildException)
                {
                    Trace.TraceError(ex.ToString());
                    _buildMessenger.ExecuteOnBuildMessage(ex.Message);
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndBuild);
                    _buildMessenger.ExecuteOnBuildStopped();
                }
                else
                {
                    Trace.TraceError(ex.ToString());
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndBuild);
                    _buildMessenger.ExecuteOnBuildStopped();
                }

                _buildMessenger.ExecuteOnStepProgress(0);
                _buildMessenger.ExecuteOnBuildProgress(0);
            }
        }
    }
}