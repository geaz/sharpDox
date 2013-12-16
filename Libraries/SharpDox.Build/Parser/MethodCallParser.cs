using ICSharpCode.NRefactory;
using SharpDox.Build.Loader;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Parser
{
    internal class MethodCallParser : BaseParser
    {
        private readonly CSharpSolution _solution;

        internal MethodCallParser(SDRepository repository, CSharpSolution solution)
            : base(repository) 
        {
            _solution = solution;
        }

        internal void ParseMethodCalls(SDNamespace sdNamespace)
        {
            for (int i = 0; i < sdNamespace.Types.Count; i++)
            {
                foreach (var sdMethod in sdNamespace.Types[i].Methods)
                {
                    HandleOnItemParseStart(sdMethod.Name, i, sdNamespace.Types.Count);
                    var file = _solution.GetFile(sdNamespace.Types[i].Region.Filename);
                    var methodAstNode = file.SyntaxTree.GetNodeContaining(
                                            new TextLocation(sdMethod.Region.BeginLine, sdMethod.Region.BeginColumn), 
                                            new TextLocation(sdMethod.Region.EndLine, sdMethod.Region.EndColumn));

                    methodAstNode.AcceptVisitor(new MethodVisitor(_repository, sdMethod, sdNamespace.Types[i], file));
                }
            }
        }
    }
}
