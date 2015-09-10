using SharpDox.Model.CallTree;
using SharpDox.Model.Repository;
using System.Linq;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class UseParser : BaseParser
    {
        internal UseParser(SDRepository repository) : base(repository) { }

        internal void ResolveAllUses()
        {
            var methods = _repository.GetAllMethods();
            foreach(var sdMethod in methods)
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
                var calledType = _repository.GetTypeByIdentifier(targetNode.CalledType.Identifier);
                var callerType = _repository.GetTypeByIdentifier(targetNode.CallerType.Identifier);

                if (calledType != null && callerType != null && calledType.Identifier != callerType.Identifier)
                {
                    if (!calledType.IsProjectStranger && calledType.UsedBy.SingleOrDefault(u => u.Identifier == callerType.Identifier) == null)
                        calledType.UsedBy.Add(callerType);

                    if (!calledType.IsProjectStranger && callerType.Uses.SingleOrDefault(u => u.Identifier == calledType.Identifier) == null)
                        callerType.Uses.Add(calledType);

                    var calledNamespace = _repository.GetNamespaceByIdentifier(calledType.Namespace.Identifier);
                    var callerNamespace = _repository.GetNamespaceByIdentifier(callerType.Namespace.Identifier);

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