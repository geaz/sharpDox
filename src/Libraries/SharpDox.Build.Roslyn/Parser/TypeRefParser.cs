using SharpDox.Model.Repository;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class TypeRefParser : BaseParser
    {
        private readonly StrangerTypeParser _strangerTypeParser;

        internal TypeRefParser(StrangerTypeParser strangerTypeParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _strangerTypeParser = strangerTypeParser;
        }

        public SDTypeRef GetParsedTypeReference(ITypeSymbol typeSymbol)
        {
            var typeRef = new SDTypeRef();
            
            var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
            if(namedTypeSymbol != null)
            {
                typeRef.Type = _strangerTypeParser.GetParsedType(this, namedTypeSymbol, namedTypeSymbol.ContainingNamespace);
            }

            var pointerTypeSymbol = typeSymbol as IPointerTypeSymbol;
            if(pointerTypeSymbol != null)
            {
                var pointedSymbol = pointerTypeSymbol.PointedAtType;
                typeRef.Type = _strangerTypeParser.GetParsedType(this, pointedSymbol, pointedSymbol.ContainingNamespace);
                typeRef.IsPointerType = true;
            }

            var arrayTypeSymbol = typeSymbol as IArrayTypeSymbol;
            if (arrayTypeSymbol != null)
            {
                var arrayDimensions = 1;
                var elementTypeSymbol = arrayTypeSymbol.ElementType;
                while (elementTypeSymbol is IArrayTypeSymbol)
                {
                    arrayDimensions++;
                    elementTypeSymbol = ((IArrayTypeSymbol)elementTypeSymbol).ElementType;
                }
                
                typeRef.Type = _strangerTypeParser.GetParsedType(this, elementTypeSymbol, elementTypeSymbol.ContainingNamespace);
                typeRef.IsArrayType = true;
                typeRef.ArrayDimensions = arrayDimensions;
            }
            
            if (typeRef.Type == null)
            {
                typeRef.Type = _strangerTypeParser.GetParsedType(this, typeSymbol, typeSymbol.ContainingNamespace);
            }

            return typeRef;
        }
    }
}
