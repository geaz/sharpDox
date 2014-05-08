using SharpDox.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDox.Build.Context.Step
{
    internal class StructeParseCodeStep : StepBase
    {
        public StructeParseCodeStep(int progressStart, int progressEnd) :
            base(StepInput.SDBuildStrings.StepParseCode, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            StepInput.CodeParser.OnStepMessage += ExecuteOnStepMessage;
            StepInput.CodeParser.OnStepProgress += ExecuteOnStepProgress;

            var solutionList = new List<string>(sdProject.Repositories.Keys);
            foreach (var solution in solutionList)
            {
                sdProject.Repositories[solution] = StepInput.CodeParser.GetStructureParsedSolution(solution);
            }

            return sdProject;
        }
    }
}
