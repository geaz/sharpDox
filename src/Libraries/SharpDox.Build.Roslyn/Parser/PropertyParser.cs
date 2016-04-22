using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
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
                Documentations = DocumentationParser.ParseDocumentation(property)
            };

            ParserOptions.SDRepository.AddMember(sdProperty);
            return sdProperty;
        }
    }
}
