using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class PropertyParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;

        internal PropertyParser(TypeRefParser typeRefParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = typeRefParser;
        }

        internal void ParseProperties(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var properties = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Property).Select(f => f as IPropertySymbol);
            foreach (var property in properties)
            {
                if (!IsMemberExcluded(property.GetIdentifier(), property.DeclaredAccessibility))
                {
                    var parsedProperty = GetParsedProperty(property);
                    if (sdType.Properties.SingleOrDefault(p => p.Name == parsedProperty.Name) == null)
                    {
                        sdType.Properties.Add(parsedProperty);
                    }
                }
            }
        }

        private SDProperty GetParsedProperty(IPropertySymbol property)
        {
            var syntaxReference = property.DeclaringSyntaxReferences.Any() ? property.DeclaringSyntaxReferences.Single() : null;
            var sdProperty = new SDProperty(property.GetIdentifier())
            {
                Name = property.Name,
                DeclaringType = _typeRefParser.GetParsedTypeReference(property.ContainingType),
                Accessibility = property.DeclaredAccessibility.ToString().ToLower(),
                ReturnType = _typeRefParser.GetParsedTypeReference(property.Type),
                CanGet = property.GetMethod != null,
                CanSet = property.SetMethod != null,
                IsAbstract = property.IsAbstract,
                IsVirtual = property.IsVirtual,
                IsOverride = property.IsOverride,
                Documentations = DocumentationParser.ParseDocumentation(property),
                Region = syntaxReference != null ? new SDRegion
                {
                    Start = syntaxReference.Span.Start,
                    End = syntaxReference.Span.End,
                    StartLine = syntaxReference.SyntaxTree.GetLineSpan(syntaxReference.Span).StartLinePosition.Line + 1,
                    EndLine = syntaxReference.SyntaxTree.GetLineSpan(syntaxReference.Span).EndLinePosition.Line + 1,
                    FilePath = syntaxReference.SyntaxTree.FilePath,
                    Filename = Path.GetFileName(syntaxReference.SyntaxTree.FilePath)
                } : null
            };

            ParserOptions.SDRepository.AddMember(sdProperty);
            return sdProperty;
        }
    }
}
