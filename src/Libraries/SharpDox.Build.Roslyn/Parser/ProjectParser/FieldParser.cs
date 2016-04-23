using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class FieldParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;

        internal FieldParser(TypeRefParser typeRefParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = typeRefParser;
        }

        internal void ParseFields(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var fields = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Field && !m.IsImplicitlyDeclared).Select(f => f as IFieldSymbol);
            foreach (var field in fields)
            {
                if (!IsMemberExcluded(field.GetIdentifier(), field.DeclaredAccessibility))
                {
                    var parsedField = GetParsedField(field);
                    if (sdType.Fields.SingleOrDefault(f => f.Name == parsedField.Name) == null)
                    {
                        sdType.Fields.Add(parsedField);
                    }
                }
            }
        }

        private SDField GetParsedField(IFieldSymbol field)
        {
            var sdField = new SDField(field.GetIdentifier())
            {
                Name = field.Name,
                DeclaringType = _typeRefParser.GetParsedTypeReference(field.ContainingType),
                Accessibility = field.DeclaredAccessibility.ToString().ToLower(),
                ReturnType = _typeRefParser.GetParsedTypeReference(field.Type),
                ConstantValue = field.ConstantValue != null ? field.ConstantValue.ToString() : string.Empty,
                IsConst = field.IsConst,
                IsReadonly = field.IsReadOnly,
                Documentations = DocumentationParser.ParseDocumentation(field)
            };

            ParserOptions.SDRepository.AddMember(sdField);
            return sdField;
        }
    }
}
