using System.Linq;
using SharpDox.Build.Roslyn.MethodVisitors;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class MethodCallParser : BaseParser
    {
        internal MethodCallParser(ParserOptions parserOptions) : base(parserOptions) { }

        internal void ParseMethodCalls()
        {
            var namespaces = ParserOptions.SDRepository.GetAllNamespaces();
            foreach (var sdNamespace in namespaces)
            {
                foreach (var sdType in sdNamespace.Types)
                {
                    foreach (var sdMethod in sdType.Methods)
                    {
                        HandleOnItemParseStart(sdMethod.Name);
                        var fileId = ParserOptions.CodeSolution.GetDocumentIdsWithFilePath(sdMethod.Region.Filename).Single();
                        var file = ParserOptions.CodeSolution.GetDocument(fileId);
                        var syntaxTree = file.GetSyntaxTreeAsync().Result;
                        
                        if (file.Project.Language == "C#")
                        {
                            var methodVisitor = new CSharpMethodVisitor(ParserOptions.SDRepository, sdMethod, sdType, file);
                            var methodSyntaxNode = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                                                    .Single(m => m.Span.Start == sdMethod.Region.Start &&
                                                    m.Span.End == sdMethod.Region.End);
                            methodVisitor.Visit(methodSyntaxNode);
                        }
                        else if (file.Project.Language == "VBNET")
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
