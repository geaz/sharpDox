using SharpDox.Model.Repository;
using System;
using SharpDox.Sdk.Config;
using System.Collections.Generic;

namespace SharpDox.Build
{
    public interface ICodeParser
    {
        event Action<string> OnDocLanguageFound;
        event Action<string> OnStepMessage;
        event Action<int> OnStepProgress;

        IEnumerable<SDRepository> GetStructureParsedSolution(string solutionFile);
        IEnumerable<SDRepository> GetFullParsedSolution(string solutionFile, ICoreConfigSection sharpDoxConfig, Dictionary<string, string> tokens);
    }
}
