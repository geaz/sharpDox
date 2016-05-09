using SharpDox.Model.CallTree;
using System.Linq;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class UseParser : BaseParser
    {
        internal UseParser(ParserOptions parserOptions) : base(parserOptions) { }

        internal void ResolveAllUses()
        {
            var methods = ParserOptions.SDRepository.GetAllMethods();
            foreach (var sdMethod in methods)
            {
                HandleOnItemParseStart(sdMethod.Name);
                foreach (var call in sdMethod.Calls)
                {
                    ResolveCall(call);
                }
            }
        }

        private void ResolveCall(SDNode call)
        {
            if (call is SDTargetNode)
            {
                var targetNode = call as SDTargetNode;
                var calledType = ParserOptions.SDRepository.GetTypeByIdentifier(targetNode.CalledType.Identifier);
                var callerType = ParserOptions.SDRepository.GetTypeByIdentifier(targetNode.CallerType.Identifier);

                if (calledType != null && callerType != null && calledType.Identifier != callerType.Identifier)
                {
                    if (!calledType.IsProjectStranger && calledType.UsedBy.SingleOrDefault(u => u.Identifier == callerType.Identifier) == null)
                        calledType.UsedBy.Add(callerType);

                    if (!calledType.IsProjectStranger && callerType.Uses.SingleOrDefault(u => u.Identifier == calledType.Identifier) == null)
                        callerType.Uses.Add(calledType);

                    var calledNamespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(calledType.Namespace.Identifier);
                    var callerNamespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(callerType.Namespace.Identifier);

                    if (calledNamespace != null && callerNamespace != null && calledNamespace.Fullname != callerNamespace.Fullname)
                    {
                        if (calledNamespace.UsedBy.SingleOrDefault(u => u.Fullname == callerNamespace.Fullname) == null)
                            calledNamespace.UsedBy.Add(callerNamespace);

                        if (callerNamespace.Uses.SingleOrDefault(u => u.Fullname == calledNamespace.Fullname) == null)
                            callerNamespace.Uses.Add(calledNamespace);
                    }
                }
            }
            else if (call is SDBlock)
            {
                foreach (var embeddedCall in ((SDBlock)call).Statements)
                {
                    ResolveCall(embeddedCall);
                }
            }
            else if (call is SDConditionalBlock)
            {
                foreach (var embeddedCall in ((SDConditionalBlock)call).TrueStatements)
                {
                    ResolveCall(embeddedCall);
                }
                foreach (var embeddedCall in ((SDConditionalBlock)call).FalseStatements)
                {
                    ResolveCall(embeddedCall);
                }
            }
        }
    }
}
