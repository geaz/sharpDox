using ICSharpCode.NRefactory;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.Repository;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class MethodCallParser : BaseParser
    {
        private readonly CSharpProject _project;

        internal MethodCallParser(SDRepository repository, CSharpProject project) 
            : base(repository) 
        {
            _project = project;
        }

        internal void ParseMethodCalls(SDNamespace sdNamespace)
        {
            for (int i = 0; i < sdNamespace.Types.Count; i++)
            {
                foreach (var sdMethod in sdNamespace.Types[i].Methods)
                {
                    HandleOnItemParseStart(sdMethod.Name, i, sdNamespace.Types.Count);
                    var file = _project.GetFile(sdNamespace.Types[i].Region.Filename);
                    var methodAstNode = file.SyntaxTree.GetNodeContaining(
                                            new TextLocation(sdMethod.Region.BeginLine, sdMethod.Region.BeginColumn), 
                                            new TextLocation(sdMethod.Region.EndLine, sdMethod.Region.EndColumn));

                    methodAstNode.AcceptVisitor(new MethodVisitor(_repository, sdMethod, sdNamespace.Types[i], file));
                }
            }
        }
    }
}
