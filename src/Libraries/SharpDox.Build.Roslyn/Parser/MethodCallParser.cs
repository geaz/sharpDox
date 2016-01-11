using System.Linq;

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
                        var fileId = ParserOptions.LoadedSolution.GetDocumentIdsWithFilePath(sdMethod.Region.Filename).Single();
                        var file = ParserOptions.LoadedSolution.GetDocument(fileId);
                        var syntaxTree = file.GetSyntaxTreeAsync().Result;

                        if (file.Project.Language == "C#")
                        {
                            var methodVisitor = new CSharpMethodVisitor(ParserOptions.SDRepository, sdMethod, sdType, file);
                            methodVisitor.Visit(syntaxTree.GetRoot());
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
