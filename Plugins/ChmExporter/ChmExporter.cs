using System;
using SharpDox.Model.Repository;
using SharpDox.Sdk.Exporter;
using SharpDox.Plugins.Chm.Templates.Strings;
using SharpDox.Plugins.Chm.Steps;
using System.IO;

namespace SharpDox.Plugins.Chm
{
    public class ChmExporter : IExporter
    {
        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;
        
        public ChmExporter(ChmConfig chmConfig, ChmStrings chmStrings)
        {
            ChmConfig = chmConfig;
            ChmStrings = chmStrings;
        }

        public void Export(SDRepository repository, string outputPath)
        {
            foreach (var language in repository.DocumentationLanguages)
            {
                Repository = repository;
                CurrentLanguage = language;
                OutputPath = outputPath;
                TmpPath = Path.Combine(outputPath, "tmp-" + CurrentLanguage);
                CurrentStrings = GetCurrentStrings();
                CurrentStep = new PreBuildStep();

                while (CurrentStep != null)
                {
                    CurrentStep.ProcessStep(this);
                }

                ExecuteOnStepProgress(100);
            }
        }

        private IStrings GetCurrentStrings()
        {
            IStrings strings = new EnStrings();
            if (CurrentLanguage == "de" || (CurrentLanguage == "default" && Repository.ProjectInfo.DocLanguage == "de"))
            {
                strings = new DeStrings();
            }
            return strings;
        }

        internal void ExecuteOnStepMessage(string message)
        {
            var handler = OnStepMessage;
            if (handler != null)
            {
                handler(string.Format("({0}) - {1}", CurrentLanguage, message));
            }
        }

        internal void ExecuteOnStepProgress(int progress)
        {
            var handler = OnStepProgress;
            if (handler != null)
            {
                handler(progress);
            }
        }

        public string ExporterName { get { return "Chm"; } }

        public string Author { get { return "Gerrit \"Geaz\" Gazic"; } }

        public string Description { get { return ChmStrings.Description; } }

        internal string OutputPath { get; private set; }

        internal string TmpPath { get; private set; }

        internal string CurrentLanguage { get; private set; }

        internal SDRepository Repository { get; private set; }

        internal ChmStrings ChmStrings { get; private set; }

        internal ChmConfig ChmConfig { get; private set; }

        internal IStrings CurrentStrings { get; private set; }

        internal Step CurrentStep { get; set; }
    }
}
