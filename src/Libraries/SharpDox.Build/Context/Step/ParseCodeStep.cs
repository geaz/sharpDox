using SharpDox.Model;
using System.Collections.Generic;

namespace SharpDox.Build.Context.Step
{
    internal class ParseCodeStep : StepBase
    {
        public ParseCodeStep(StepInput stepInput, int progressStart, int progressEnd) :
            base(stepInput, stepInput.SDBuildStrings.StepParseCode, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            _stepInput.CodeParser.OnDocLanguageFound += sdProject.AddDocumentationLanguage;
            _stepInput.CodeParser.OnStepMessage += ExecuteOnStepMessage;
            _stepInput.CodeParser.OnStepProgress += ExecuteOnStepProgress;

            var solutionList = new List<string>(sdProject.Repositories.Keys);
            foreach (var solution in solutionList)
            {
                var projectRepositories = _stepInput.CodeParser.GetFullParsedSolution(solution, _stepInput.CoreConfigSection, sdProject.Tokens);

                foreach (var projectRepository in projectRepositories)
                {
                    sdProject.Repositories[projectRepository.Location] = projectRepository;
                }
                
                // Because of excluding privates, internals and protected members
                // it is possible, that a namespace has no visible namespaces at all.
                // It is necessary to remove empty namespaces.
                foreach (var sdNamespace in sdProject.Repositories[solution].GetAllNamespaces())
                {
                    if (sdNamespace.Types.Count == 0)
                    {
                        sdProject.Repositories[solution].RemoveNamespace(sdNamespace);
                    }
                }
            }

            return sdProject;
        }
    }
}
