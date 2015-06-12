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

            var solutionList = new List<string>(sdProject.Solutions.Keys);
            foreach (var solution in solutionList)
            {
                sdProject.Solutions[solution] = _stepInput.CodeParser.GetStructureParsedSolution(solution);
            }

            return sdProject;
        }
    }
}
