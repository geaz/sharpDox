using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class StrangerTypeParser : BaseParser
    {
        internal StrangerTypeParser(ParserOptions parserOptions) : base(parserOptions) { }

        public SDType GetParsedType(TypeRefParser typeRefParser, ITypeSymbol typeSymbol, INamespaceSymbol namespaceSymbol)
        {
            var sdType = ParserOptions.SDRepository.GetTypeByIdentifier(typeSymbol.GetIdentifier());
            if (sdType == null)
            {
                sdType = CreateSDType(typeSymbol, namespaceSymbol);
                ParserOptions.SDRepository.AddType(sdType);

                var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                    {
                        sdType.TypeArguments.Add(typeRefParser.GetParsedTypeReference(typeArgument));
                    }
                }
            }
            return sdType;
        }

        private SDType CreateSDType(ITypeSymbol typeSymbol, INamespaceSymbol namespaceSymbol)
        {
            var sdNamespace = new SDNamespace(namespaceSymbol.GetIdentifier()) { IsProjectStranger = true };
            var sdType = new SDType(typeSymbol.GetIdentifier(), typeSymbol.Name, sdNamespace)
            {
                Accessibility = typeSymbol.DeclaredAccessibility.ToString().ToLower(),
                IsAbstract = typeSymbol.IsAbstract,
                IsReferenceType = typeSymbol.IsReferenceType,
                IsSealed = typeSymbol.IsSealed,
                IsStatic = typeSymbol.IsStatic,
                IsProjectStranger = true,
                Kind = typeSymbol.TypeKind.ToString().ToLower()
            };
            return sdType;
        }
    }
}
