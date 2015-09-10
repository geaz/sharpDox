using System;
using SharpDox.Sdk.Config;
using System.Collections.Generic;
using SharpDox.Model;

namespace SharpDox.Build
{
    public interface ICodeParser
    {
        event Action<string> OnDocLanguageFound;
        event Action<string> OnStepMessage;
        event Action<int> OnStepProgress;

        SDSolution GetStructureParsedSolution(string solutionFile);
        SDSolution GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens);
    }
}
