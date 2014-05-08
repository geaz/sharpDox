using SharpDox.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDox.Build.Context.Step
{
    internal class ParseCodeStep : StepBase
    {
        public ParseCodeStep(int progressStart, int progressEnd) :
            base(StepInput.SDBuildStrings.StepParseCode, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            StepInput.CodeParser.OnDocLanguageFound += sdProject.AddDocumentationLanguage;
            StepInput.CodeParser.OnStepMessage += ExecuteOnStepMessage;
            StepInput.CodeParser.OnStepProgress += ExecuteOnStepProgress;

            var solutionList = new List<string>(sdProject.Repositories.Keys);
            foreach (var solution in solutionList)
            {
                sdProject.Repositories[solution] = StepInput.CodeParser.GetFullParsedSolution(solution, StepInput.CoreConfigSection.ExcludedIdentifiers.ToList());
            }

            return sdProject;
        }
    }
}
