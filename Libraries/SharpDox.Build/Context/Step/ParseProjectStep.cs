using SharpDox.Model;
using SharpDox.Model.Repository;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SharpDox.Build.Context.Step
{
    internal class ParseProjectStep : StepBase
    {
        private SDProject _sdProject;

        public ParseProjectStep(int progressStart, int progressEnd) :
            base(StepInput.SDBuildStrings.StepParseProject, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            _sdProject = sdProject;
            SetProjectInfos();
            GetImages();
            ParseDescriptions();
            
            if (Path.GetExtension(StepInput.CoreConfigSection.InputFile) == ".sdnav")
            {
                ParseNavigationFiles();
            }
            else
            {
                _sdProject.Repositories.Add(StepInput.CoreConfigSection.InputFile, new SDRepository());
            }

            return _sdProject;
        }

        private void SetProjectInfos()
        {
            ExecuteOnStepMessage(StepInput.SDBuildStrings.ParsingProject);
            ExecuteOnStepProgress(25);

            _sdProject.DocLanguage = StepInput.CoreConfigSection.DocLanguage;
            _sdProject.LogoPath = StepInput.CoreConfigSection.LogoPath;
            _sdProject.Author = StepInput.CoreConfigSection.Author;
            _sdProject.ProjectName = StepInput.CoreConfigSection.ProjectName;
            _sdProject.VersionNumber = StepInput.CoreConfigSection.VersionNumber;
        }

        private void GetImages()
        {
            var images = Directory.EnumerateFiles(Path.GetDirectoryName(StepInput.CoreConfigSection.InputFile), "sdi.*", SearchOption.AllDirectories);
            foreach (var image in images)
            {
                _sdProject.Images.Add(image);
            }
        }

        private void ParseDescriptions()
        {
            ExecuteOnStepMessage(StepInput.SDBuildStrings.ParsingDescriptions);
            ExecuteOnStepProgress(50);

            var potentialReadMes = Directory.EnumerateFiles(Path.GetDirectoryName(StepInput.CoreConfigSection.InputFile), "*readme*");
            if (potentialReadMes.Any())
            {
                foreach (var readme in potentialReadMes)
                {
                    var splitted = Path.GetFileName(readme).Split('.');
                    if (splitted.Length > 0 && CultureInfo.GetCultures(CultureTypes.NeutralCultures).Any(c => c.TwoLetterISOLanguageName == splitted[0].ToLower()))
                    {
                        if (!_sdProject.Description.ContainsKey(splitted[0].ToLower()))
                        {
                            _sdProject.Description.Add(splitted[0].ToLower(), File.ReadAllText(readme));
                            _sdProject.AddDocumentationLanguage(splitted[0].ToLower());
                        }
                    }
                    else if (splitted.Length > 0 && splitted[0].ToLower() == "readme" && !_sdProject.Description.ContainsKey("default"))
                    {
                        _sdProject.Description.Add("default", File.ReadAllText(readme));
                    }
                }
            }
        }

        private void ParseNavigationFiles()
        {
            ExecuteOnStepMessage(StepInput.SDBuildStrings.ParsingNav);
            ExecuteOnStepProgress(50);

            var navFileParser = new SDNavParser(StepInput.CoreConfigSection.InputFile);
            var navFiles = Directory.EnumerateFiles(Path.GetDirectoryName(StepInput.CoreConfigSection.InputFile), "*.sdnav", SearchOption.AllDirectories);
            foreach(var navFile in navFiles)
            {
                _sdProject = navFileParser.ParseNavFile(navFile, _sdProject);
            }
        }
    }
}
