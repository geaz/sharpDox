using SharpDox.Model.Repository;
using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Build
{
    public interface ICodeParser
    {
        event Action<string> OnDocLanguageFound;
        event Action<string> OnStepMessage;
        event Action<int> OnStepProgress;

        SDRepository GetStructureParsedSolution(string solutionFile);
        SDRepository GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig);
    }
}
