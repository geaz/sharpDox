using System.Collections.Generic;
using SharpDox.Model;

namespace SharpDox.Build.Context.Step
{
    internal class StructeParseCodeStep : StepBase
    {
        public StructeParseCodeStep(StepInput stepInput, int progressStart, int progressEnd) :
            base(stepInput, stepInput.SDBuildStrings.StepParseCode, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            _stepInput.CodeParser.OnStepMessage += ExecuteOnStepMessage;
            _stepInput.CodeParser.OnStepProgress += ExecuteOnStepProgress;

            var solutionList = new List<string>(sdProject.Repositories.Keys);
            foreach (var solution in solutionList)
            {
                var projectRepositories = _stepInput.CodeParser.GetStructureParsedSolution(solution);

                foreach (var projectRepository in projectRepositories)
                {
                    sdProject.Repositories[projectRepository.Location] = projectRepository;
                }
            }

            return sdProject;
        }
    }
}
