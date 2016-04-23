using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class TypeParameterParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;

        internal TypeParameterParser(TypeRefParser typeRefParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = typeRefParser;
        }

        internal SortedList<SDTypeParameter> ParseTypeParameters(IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            var sdTypeParameters = new SortedList<SDTypeParameter>();
            foreach (var typeParameter in typeParameters)
            {
                var parsedTypeParameter = GetTypeParameter(typeParameter);
                if (sdTypeParameters.SingleOrDefault((i => i.Name == parsedTypeParameter.Name)) == null)
                {
                    sdTypeParameters.Add(parsedTypeParameter);
                }
            }
            return sdTypeParameters;
        }

        private SDTypeParameter GetTypeParameter(ITypeParameterSymbol typeParameter)
        {
            var sdTypeParameter = new SDTypeParameter
            {
                Name = typeParameter.Name,
                HasDefaultConstructorConstraint = typeParameter.HasConstructorConstraint,
                HasReferenceTypeConstraint = typeParameter.HasReferenceTypeConstraint,
                HasValueTypeConstraint = typeParameter.HasValueTypeConstraint
            };
            foreach (var constraintType in typeParameter.ConstraintTypes)
            {
                sdTypeParameter.ConstraintTypes.Add(_typeRefParser.GetParsedTypeReference(constraintType));
            }

            return sdTypeParameter;
        }
    }
}
