using System.IO;
using SharpDox.Model.Repository;

namespace SharpDox.Plugins.Chm.Steps
{
    internal class SaveAndCleanStep : Step
    {
        private SDRepository _repository;
        private string _currentLanguage;
        private string _tmpPath;

        public override void ProcessStep(ChmExporter chmExporter)
        {
            _repository = chmExporter.Repository;
            _currentLanguage = chmExporter.CurrentLanguage;
            _tmpPath = chmExporter.TmpPath;

            chmExporter.ExecuteOnStepProgress(90);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.Saving);
            SaveTo(chmExporter.OutputPath, 0);

            chmExporter.ExecuteOnStepProgress(95);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.Cleaning);
            CleanUp();

            chmExporter.ExecuteOnStepProgress(100);
            chmExporter.CurrentStep = null;
        }

        private void SaveTo(string path, int appenderNumber)
        {
            try
            {
                var copyTo = Path.Combine(path, _repository.ProjectInfo.ProjectName + "-" + _currentLanguage + ".chm");
                if (appenderNumber > 0)
                {
                    copyTo = Path.Combine(path, _repository.ProjectInfo.ProjectName + appenderNumber + "-" + _currentLanguage + ".chm");
                }

                File.Copy(Path.Combine(_tmpPath, _repository.ProjectInfo.ProjectName.Replace(" ", "") + ".chm"), copyTo, true);
            }
            catch (IOException ex)
            {
                SaveTo(path, appenderNumber + 1);
            }
        }

        private void CleanUp()
        {
            Directory.Delete(_tmpPath, true);
        }
    }
}
