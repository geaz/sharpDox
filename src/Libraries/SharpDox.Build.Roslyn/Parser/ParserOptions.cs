using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class ParserOptions
    {
        public Solution LoadedSolution { get; set; }
        public SDRepository SDRepository { get; set; }
        public ICoreConfigSection SharpDoxConfig { get; set; }
    }
}
