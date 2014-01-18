using System;
using System.IO;
using SharpDox.Plugins.Html.Steps;
using SharpDox.Plugins.Html.Templates.Sites;
using SharpDox.Model.Repository;
using SharpDox.Sdk.Exporter;
using SharpDox.Plugins.Html.Templates.Strings;

namespace SharpDox.Plugins.Html
{
    public class HtmlExporter : IExporter
    {
        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;

        public HtmlExporter(HtmlStrings strings)
	    {
            HtmlStrings = strings;
	    }

        public void Export(SDRepository repository, string outputPath)
        {
            foreach (var documentationLanguage in repository.DocumentationLanguages)
            {
                Repository = repository;
                CurrentLanguage = documentationLanguage;
                OutputPath = Path.Combine(outputPath, CurrentLanguage);
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

        public string ExporterName { get { return "Html"; } }

        public string Author { get { return "Gerrit \"Geaz\" Gazic"; } }

        public string Description { get { return HtmlStrings.Description; } }

        internal string OutputPath { get; private set; }

        internal string CurrentLanguage { get; private set; }

        internal SDRepository Repository { get; private set; }

        internal HtmlStrings HtmlStrings { get; private set; }

        internal IStrings CurrentStrings { get; private set; }

        internal Step CurrentStep { get; set; }
    }
}