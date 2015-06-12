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

            var solutionList = new List<string>(sdProject.Solutions.Keys);
            foreach (var solution in solutionList)
            {
                sdProject.Solutions[solution] = _stepInput.CodeParser.GetFullParsedSolution(solution, _stepInput.CoreConfigSection, sdProject.Tokens);
            }

            return sdProject;
        }
    }
}
