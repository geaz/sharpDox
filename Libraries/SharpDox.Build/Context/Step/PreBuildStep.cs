using System.IO;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Context.Step
{
    internal class PreBuildStep
    {
        private readonly SharpDoxConfig _config;
        private readonly SDBuildStrings _sdBuildStrings;

        public PreBuildStep(SharpDoxConfig config, SDBuildStrings sdBuildStrings)
        {
            _config = config;
            _sdBuildStrings = sdBuildStrings;
        }

        public void CheckConfig(bool checkOutputPath)
        {
            if (string.IsNullOrEmpty(_config.InputPath))
                throw new SDBuildException(_sdBuildStrings.NoProjectGiven);

            if (!File.Exists(_config.InputPath))
                throw new SDBuildException(_sdBuildStrings.ProjectNotFound);

            if (checkOutputPath)
            {
                if (string.IsNullOrEmpty(_config.DocLanguage))
                    throw new SDBuildException(_sdBuildStrings.NoDocLanguage);

                if (string.IsNullOrEmpty(_config.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.NoOutputPathGiven);

                if (!Directory.Exists(_config.OutputPath))
                    throw new SDBuildException(_sdBuildStrings.OutputPathNotFound);
            }
        }
    }
}
