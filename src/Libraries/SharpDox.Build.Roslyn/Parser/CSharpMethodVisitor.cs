using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SharpDox.Model.CallTree;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class CSharpMethodVisitor : CSharpSyntaxWalker
    {
        private List<SDNode> _tokenList;

        private readonly Document _file;
        private readonly SDRepository _repository;
        private readonly SDMethod _method;
        private readonly SDType _type;

        public CSharpMethodVisitor(SDRepository repository, SDMethod method, SDType type, Document file) : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _repository = repository;
            _method = method;
            _type = type;
            _file = file;
            _tokenList = method.Calls;
        }

        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            var token = CreateConditionalBlock(node.Condition.ToString());
            _tokenList.Add(token);

            VisitChildren(token.FalseStatements, node.WhenFalse);
            VisitChildren(token.TrueStatements, node.WhenTrue);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            var token = CreateBlock($"while ({node.Condition})", SDNodeRole.WhileLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, node.Statement);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            var token = CreateBlock($"while ({node.Condition})", SDNodeRole.DoWhileLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, node.Statement);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax switchStatement)
        {
            var token = CreateBlock($"switch ({switchStatement.Expression})", SDNodeRole.Switch);
            _tokenList.Add(token);
            _tokenList = token.Statements;

            foreach (var section in switchStatement.Sections)
            {
                var caseToken = CreateBlock(string.Join(" ", section.Labels.Select(o => o.ToString())), SDNodeRole.Case);
                _tokenList.Add(caseToken);

                VisitChildren(caseToken.Statements, section);
            }
        }

        public override void VisitForEachStatement(ForEachStatementSyntax foreachStatement)
        {
            var expression = $"foreach ({foreachStatement.Type} {foreachStatement.Identifier} in {foreachStatement.InKeyword})";
            var token = CreateBlock(expression, SDNodeRole.ForEach);
            _tokenList.Add(token);

            VisitChildren(token.Statements, foreachStatement.Statement);
        }

        public override void VisitForStatement(ForStatementSyntax forStatement)
        {
            var initializers = string.Join(" ", forStatement.Initializers.Select(o => o.ToString()));
            var condition = forStatement.Condition.ToString();
            var interator = string.Join(" ", forStatement.Incrementors.Select(o => o.ToString()));
            var expression = $"for ({initializers}; {condition}; {interator})";

            var token = CreateBlock(expression, SDNodeRole.ForLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, forStatement.Statement);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ParseTargetExpression(node);
            base.VisitInvocationExpression(node);
        }

        private void VisitChildren(List<SDNode> statemenList, CSharpSyntaxNode children)
        {
            var tmp = _tokenList;

            _tokenList = statemenList;
            Visit(children);

            _tokenList = tmp;
        }

        private void ParseTargetExpression(InvocationExpressionSyntax expression)
        {
            var compilation = _file.Project.GetCompilationAsync().Result;
            var symbolInfo = compilation.GetSemanticModel(_file.GetSyntaxTreeAsync().Result).GetSymbolInfo(expression);

            if (symbolInfo.Symbol != null)
            {
                var member = symbolInfo.Symbol;
                var method = member as IMethodSymbol;

                if (method != null)
                {
                    var callerType = _type;
                    var callerMethod = _method;

                    SDType calledType = null;
                    var sdType = _repository.GetTypeByIdentifier(member.ContainingType.GetIdentifier());
                    if (sdType == null)
                    {
                        var sdNamespace = _repository.GetNamespaceByIdentifier(member.ContainingNamespace.ToDisplayString());
                        sdNamespace = sdNamespace ?? new SDNamespace(member.ContainingNamespace.ToDisplayString()) { IsProjectStranger = true };
                        calledType = new SDType(member.ContainingType.GetIdentifier(), member.ContainingType.Name, sdNamespace) { IsProjectStranger = true };
                    }
                    else
                    {
                        calledType = sdType;
                    }

                    SDMethod calledMethod = null;
                    if (calledType.IsProjectStranger)
                    {
                        calledMethod = new SDMethod(method.GetIdentifier(), method.Name);
                    }
                    else
                    {
                        SDMethod sdMethod = sdType.Methods.Concat(sdType.Constructors).SingleOrDefault(m => m.Identifier == method.GetIdentifier());
                        if (sdMethod != null)
                        {
                            calledMethod = sdMethod;
                        }
                    }

                    // Only add method, if it is project stranger, public or private (if not only public members option on)
                    if (calledMethod != null)
                    {
                        var token = new SDTargetNode
                        {
                            CalledType = calledType,
                            CallerType = callerType,
                            CalledMethod = calledMethod,
                            CallerMethod = callerMethod
                        };

                        _tokenList.Add(token);
                    }
                }
            }
        }

        private SDBlock CreateBlock(string expression, string role)
        {
            var token = new SDBlock
            {
                Expression = expression,
                Role = role
            };

            return token;
        }

        private SDConditionalBlock CreateConditionalBlock(string expression)
        {
            var token = new SDConditionalBlock
            {
                Expression = expression,
                Role = SDNodeRole.Conditional
            };

            return token;
        }
    }
}
