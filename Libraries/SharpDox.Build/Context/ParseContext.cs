using System;
using System.Diagnostics;
using System.Threading;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context
{
    internal class ParseContext
    {
        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;
        private readonly Steps _steps;

        public ParseContext(ICoreConfigSection coreConfigSection, SDBuildStrings sdBuildStrings, IConfigController configController, BuildMessenger buildMessenger)
        {
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;

            _steps = new Steps(coreConfigSection, sdBuildStrings, configController, buildMessenger);
        }

        public virtual void ParseSolution()
        {
            try
            {
                _buildMessenger.ExecuteOnBuildProgress(0);
                _steps.PreBuildStep.CheckConfig();

                _buildMessenger.ExecuteOnBuildProgress(10);
                var solution = _steps.LoadStep.LoadSolution();

                _buildMessenger.ExecuteOnBuildProgress(60);
                var repository = _steps.StructureParseStep.ParseStructure(solution);

                _buildMessenger.ExecuteOnBuildProgress(100);
                _buildMessenger.ExecuteOnStepMessage(string.Empty);
                _buildMessenger.ExecuteOnParseCompleted(repository);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.ParseStopped);
                }
                else if (ex is SDBuildException)
                {
                    Trace.TraceError(ex.ToString());
                    _buildMessenger.ExecuteOnBuildMessage(ex.Message);
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndParse);
                }
                else
                {
                    Trace.TraceError(ex.ToString());
                    _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.CouldNotEndParse);
                }

                _buildMessenger.ExecuteOnStepProgress(0);
                _buildMessenger.ExecuteOnBuildProgress(0);
                _buildMessenger.ExecuteOnParseCompleted(null);
            }
        }
    }
}