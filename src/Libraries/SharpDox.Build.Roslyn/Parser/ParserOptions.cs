using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SharpDox.Model;
using SharpDox.Model.Repository;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class ParserOptions
    {
        public Solution CodeSolution { get; set; }
        public SDSolution SDSolution { get; set; }
        public SDRepository SDRepository { get; set; }
        public ICoreConfigSection SharpDoxConfig { get; set; }
        public Dictionary<string, string> Tokens { get; set; }
    }
}
