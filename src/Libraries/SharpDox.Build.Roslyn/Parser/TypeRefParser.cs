using SharpDox.Model.Repository;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class TypeRefParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal TypeRefParser(TypeParser typeParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeParser = typeParser;
        }

        public SDTypeRef GetParsedTypeReference(ITypeSymbol typeSymbol)
        {
            var typeRef = new SDTypeRef();
            
            var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
            if(namedTypeSymbol != null)
            {
                typeRef.Type = _typeParser.GetParsedType(namedTypeSymbol);
            }

            var pointerTypeSymbol = typeSymbol as IPointerTypeSymbol;
            if(pointerTypeSymbol != null)
            {
                typeRef.Type = _typeParser.GetParsedType(pointerTypeSymbol.PointedAtType);
                typeRef.IsPointerType = true;
            }

            var arrayTypeSymbol = typeSymbol as IArrayTypeSymbol;
            if (arrayTypeSymbol != null)
            {
                typeRef.Type = _typeParser.GetParsedType(arrayTypeSymbol.ElementType);
                typeRef.IsArrayType = true;
            }

            return typeRef;
        }
    }
}
