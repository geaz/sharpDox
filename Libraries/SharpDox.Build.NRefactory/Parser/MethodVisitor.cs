using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.CallTree;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Collections.Generic;
using System.Linq;

namespace SharpDox.Build.NRefactory.Parser
{
	internal class MethodVisitor : DepthFirstAstVisitor
	{
        private List<SDNode> _tokenList;

		private readonly CSharpFile _file;
        private readonly SDRepository _repository;
        private readonly SDMethod _method;
        private readonly SDType _type;

		public MethodVisitor(SDRepository repository, SDMethod method, SDType type, CSharpFile file)
		{
            _repository = repository;
		    _method = method;
            _type = type;
			_file = file;
			_tokenList = method.Calls;
		}

		public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
		{
            var token = CreateConditionalBlock(conditionalExpression.Condition.GetText());
            _tokenList.Add(token);

            VisitChildren(token.FalseStatements, conditionalExpression.FalseExpression);
            VisitChildren(token.TrueStatements, conditionalExpression.TrueExpression);		
		}        

		public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
		{
            var token = CreateConditionalBlock(ifElseStatement.Condition.GetText());
            _tokenList.Add(token);

            VisitChildren(token.FalseStatements, ifElseStatement.FalseStatement);
            VisitChildren(token.TrueStatements, ifElseStatement.TrueStatement);            
		}        

		public override void VisitWhileStatement(WhileStatement whileStatement)
		{
            var token = CreateBlock(string.Format("while ({0})", whileStatement.Condition.GetText()), SDNodeRole.WhileLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, whileStatement.EmbeddedStatement);
		}

		public override void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
		{
            var token = CreateBlock(string.Format("while ({0})", doWhileStatement.Condition.GetText()), SDNodeRole.DoWhileLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, doWhileStatement.EmbeddedStatement);
		}

        public override void VisitSwitchStatement(SwitchStatement switchStatement)
        {
            var token = CreateBlock(string.Format("switch ({0})", switchStatement.Expression.GetText()), SDNodeRole.Switch);
            _tokenList.Add(token);

            var tmp = _tokenList;
            _tokenList = token.Statements;

            foreach (var section in switchStatement.SwitchSections)
            {
                var caseToken = CreateBlock(string.Join(" ", section.CaseLabels.Select(o => o.GetText())), SDNodeRole.Case);
                _tokenList.Add(caseToken);

                VisitChildren(caseToken.Statements, section);
            }            
        }

        public override void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            var expression = string.Format("foreach ({0} {1} in {2})", foreachStatement.VariableType.GetText(), foreachStatement.VariableName, foreachStatement.InExpression.GetText());
            var token = CreateBlock(expression, SDNodeRole.ForEach);
            _tokenList.Add(token);

            VisitChildren(token.Statements, foreachStatement.EmbeddedStatement);
        }

        public override void VisitForStatement(ForStatement forStatement)
        {
            var initializers = string.Join(" ", forStatement.Initializers.Select(o => o.GetText()));
            var condition = forStatement.Condition.GetText();
            var interator = string.Join(" ", forStatement.Iterators.Select(o => o.GetText()));
            var expression = string.Format("for ({0}; {1}; {2})", initializers, condition, interator);

            var token = CreateBlock(expression, SDNodeRole.ForLoop);
            _tokenList.Add(token);

            VisitChildren(token.Statements, forStatement.EmbeddedStatement);
        }        

        public override void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            ParseTargetExpression(invocationExpression);
            base.VisitInvocationExpression(invocationExpression);
        }

        private void VisitChildren(List<SDNode> statemenList, AstNode children)
        {
            var tmp = _tokenList;

            _tokenList = statemenList;
            VisitChildren(children);

            _tokenList = tmp;
        }

        private void ParseTargetExpression(Expression expression)
        {
            if (_file != null)
            {
                var resolver = new CSharpAstResolver(_file.Project.Compilation, _file.SyntaxTree, _file.UnresolvedTypeSystemForFile);
                var resolveResult = resolver.Resolve(expression);

                if (resolveResult is CSharpInvocationResolveResult)
                {
                    var member = ((CSharpInvocationResolveResult)resolveResult).Member;
                    var method = member as IMethod;

                    if (method != null)
                    {
                        var callerType = _type;
                        var callerMethod = _method;

                        SDType calledType = null;
                        var sdType = _repository.GetTypeByIdentifier(member.DeclaringType.GetIdentifier());
                        if (sdType == null)
                        {
                            var sdNamespace = _repository.GetNamespaceByIdentifier(member.DeclaringType.Namespace);
                            sdNamespace = sdNamespace == null ? new SDNamespace(member.DeclaringType.Namespace) { IsProjectStranger = true } : sdNamespace;
                            calledType = new SDType(member.DeclaringType.GetIdentifier(), member.DeclaringType.Name, sdNamespace) { IsProjectStranger = true };
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
                            if(sdMethod != null)
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
