using System;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Model;
using SharpDox.Model.CallTree;
using SharpDox.Model.Repository.Members;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.Sequence.Model;

namespace SharpDox.UML.Sequence
{
    internal class SequenceDiagramParser
    {
        private SequenceDiagram _sequenceDiagram;

        private readonly SDMethod _method;
        private readonly SDProject _sdProject;

        public SequenceDiagramParser(SDMethod method, SDProject sdProject = null)
        {
            _method = method;
            _sdProject = sdProject;
        }

        public SequenceDiagram CreateSequenceDiagram()
        {
            if(!IsSequenceDiagramEmpty())
            {
                _sequenceDiagram = new SequenceDiagram(_sdProject);

                CreateStartNode(_method.DeclaringType.Identifier, _method.Name, _method.Identifier);
                ParseAllCalls(_method.Calls);
                CreateReturn(_method);
            }

            return _sequenceDiagram;
        }

        public bool IsSequenceDiagramEmpty()
        {
            return !NodeNotEmpty(null, _method.Calls.Skip(1).ToList());
        }

        private void CreateStartNode(string identifier, string startMethodName, string startMethodIdentifier)
        {
            var node = _sequenceDiagram.AddNode(identifier);
            _sequenceDiagram.AddConnection(Guid.Empty, node.ID, startMethodName, startMethodIdentifier);
            _sequenceDiagram.StartNodeID = node.ID;
        }

        private void ParseAllCalls(List<SDNode> calls)
        {
            foreach (var call in calls)
            {
                ParseCall(call, _sequenceDiagram);
            }
        }

        private void ParseCall(SDNode call, SequenceDiagramComposite composite)
        {
            var block = call as SDBlock;
            if (block != null && NodeNotEmpty(block, block.Statements))
            {
                ParseMethodBlock(composite, block);
            }

            var conditionalBlock = call as SDConditionalBlock;
            if (conditionalBlock != null && NodeNotEmpty(conditionalBlock, conditionalBlock.TrueStatements))
            {
                ParseConditionalMethodBlock(composite, conditionalBlock);
            }

            var targetNode = call as SDTargetNode;
            if (targetNode != null && NodeNotEmpty(targetNode, null))
            {
                ParseTargetNode(composite, targetNode);
            }
        }

        private void ParseMethodBlock(SequenceDiagramComposite composite, SDBlock block)
        {
            var sequenceDiagramBlock = composite.AddBlock(block.Expression);
            foreach (var statement in block.Statements)
            {
                ParseCall(statement, sequenceDiagramBlock);
            }
        }

        private void ParseConditionalMethodBlock(SequenceDiagramComposite composite, SDConditionalBlock conditionalBlock)
        {
            var sequenceDiagramBlock = composite.AddBlock(string.Format("if ({0})", conditionalBlock.Expression));
            foreach (var statement in conditionalBlock.TrueStatements)
            {
                ParseCall(statement, sequenceDiagramBlock);
            }

            if (NodeNotEmpty(conditionalBlock, conditionalBlock.FalseStatements))
            {
                sequenceDiagramBlock = composite.AddBlock("else");
                foreach (var statement in conditionalBlock.FalseStatements)
                {
                    ParseCall(statement, sequenceDiagramBlock);
                }
            }
        }

        private void ParseTargetNode(SequenceDiagramComposite composite, SDTargetNode targetNode)
        {
            if (!targetNode.CalledType.IsProjectStranger)
            {
                var caller = _sequenceDiagram.Nodes.SingleOrDefault(o => o.TypeIdentifier == targetNode.CallerType.Identifier);
                caller = caller == null ? _sequenceDiagram.Nodes.Single(o => o.ID == _sequenceDiagram.StartNodeID) : caller;

                var called = _sequenceDiagram.Nodes.SingleOrDefault(o => o.TypeIdentifier == targetNode.CalledType.Identifier) ?? _sequenceDiagram.AddNode(targetNode.CalledType.Identifier);

                composite.AddConnection(caller.ID, called.ID, targetNode.CalledMethod.Name, targetNode.CalledMethod.Identifier);

                var sdType = _sdProject.GetTypeByIdentifier(targetNode.CalledType.Identifier);
                var targetMethod = sdType.Methods.SingleOrDefault(o => o.Identifier == targetNode.CalledMethod.Identifier);
                if (caller.ID != called.ID && targetMethod.ReturnType != null && targetMethod.ReturnType.Name.ToUpper() != "VOID")
                {
                    composite.AddConnection(called.ID, caller.ID, targetMethod.ReturnType.Name, targetNode.CalledMethod.Identifier, true);
                }
            }
        }

        private bool NodeNotEmpty(SDNode node, List<SDNode> calls)
        {
            var targetNode = node as SDTargetNode;
            if (targetNode != null)
            {
                if (!targetNode.CalledType.IsProjectStranger)
                {
                    return true;
                }
            }

            if (calls != null)
            {
                if (calls.Count == 0)
                    return false;

                var empty = false;

                foreach (var call in calls)
                {
                    var block = call as SDBlock;
                    if (block != null && NodeNotEmpty(block, block.Statements))
                    {
                        empty = true;
                        break;
                    }

                    var conditionalBlock = call as SDConditionalBlock;
                    if (conditionalBlock != null && NodeNotEmpty(conditionalBlock, conditionalBlock.TrueStatements))
                    {
                        empty = true;
                        break;
                    }

                    var target = call as SDTargetNode;
                    if (target != null && NodeNotEmpty(target, null))
                    {
                        empty = true;
                        break;
                    }
                }

                return empty;
            }

            return false;
        }

        private void CreateReturn(SDMethod method)
        {
            if (!string.IsNullOrEmpty(method.ReturnType.Name) && method.ReturnType.Name.ToUpper() != "VOID")
            {
                _sequenceDiagram.AddConnection(_sequenceDiagram.StartNodeID, Guid.Empty, method.ReturnType.Name, string.Empty, true);
            }
        }
    }
}
