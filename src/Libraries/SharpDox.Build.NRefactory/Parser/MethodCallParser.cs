using ICSharpCode.NRefactory;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.Repository;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class MethodCallParser : BaseParser
    {
        private readonly CSharpSolution _solution;

        internal MethodCallParser(SDRepository repository, CSharpSolution solution) : base(repository) 
        {
            _solution = solution;
        }

        internal void ParseMethodCalls()
        {
            var namespaces = _repository.GetAllNamespaces();
            foreach(var sdNamespace in namespaces)
            {
                foreach(var sdType in sdNamespace.Types)
                {
                    foreach(var sdMethod in sdType.Methods)
                    {
                        HandleOnItemParseStart(sdMethod.Name);
                        var file = _solution.GetFile(sdType.Region.Filename);
                        var methodAstNode = file.SyntaxTree.GetNodeContaining(
                                                new TextLocation(sdMethod.Region.BeginLine, sdMethod.Region.BeginColumn),
                                                new TextLocation(sdMethod.Region.EndLine, sdMethod.Region.EndColumn));

                        methodAstNode.AcceptVisitor(new MethodVisitor(_repository, sdMethod, sdType, file));
                    }
                }
            }       
        }
    }
}
