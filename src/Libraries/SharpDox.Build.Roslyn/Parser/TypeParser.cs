using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Sdk.Config;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class TypeParser : BaseParser
    {
        private readonly EventParser _eventParser;
        private readonly FieldParser _fieldParser;
        private readonly MethodParser _methodParser;
        private readonly PropertyParser _propertyParser;

        internal TypeParser(ParserOptions parserOptions) : base(parserOptions)
        {
            _eventParser = new EventParser(this, parserOptions);
            _fieldParser = new FieldParser(this, parserOptions);
            _methodParser = new MethodParser(this, parserOptions);
            _propertyParser = new PropertyParser(this, parserOptions);
        }

        internal void ParseProjectTypes(List<INamedTypeSymbol> typeSymbols)
        {
            for (int i = 0; i < typeSymbols.Count; i++ )
            {
                HandleOnItemParseStart(typeSymbols[i].GetIdentifier());
                if (!IsMemberExcluded(typeSymbols[i].GetIdentifier(), typeSymbols[i].DeclaredAccessibility))
                {
                    GetParsedType(typeSymbols[i], false);
                }
            }
        }

        internal SDType GetParsedType(ITypeSymbol typeSymbol, bool isProjectStranger = true)
        {
            var parsedType = ParserOptions.SDRepository.GetTypeByIdentifier(typeSymbol.GetIdentifier());
            if (parsedType == null)
            {
                parsedType = CreateSDType(typeSymbol, isProjectStranger);
                ParseForeignTypeToModel(parsedType, typeSymbol);
            }

            if (!isProjectStranger)
            {
                ParseTypeToModel(parsedType, typeSymbol);
            }

            return parsedType;
        }               

        private void ParseTypeToModel(SDType sdType, ITypeSymbol typeSymbol)
        {
            var type = typeSymbol as INamedTypeSymbol;
            if (type != null)
            {
                sdType.IsProjectStranger = false;
                sdType.Documentations = DocumentationParser.ParseDocumentation(typeSymbol);
                AddParsedNestedTypes(sdType, type.GetTypeMembers());
                AddParsedTypeArguments(sdType, type.TypeArguments);
                AddParsedTypeParameters(sdType, type.TypeParameters);
                AddParsedProperties(sdType, type);
                AddParsedFields(sdType, type);
                AddParsedConstructorsAndMethods(sdType, type);
                AddParsedEvents(sdType, type);
            }
        }

        private void ParseForeignTypeToModel(SDType sdType, ITypeSymbol typeSymbol)
        {
            var arrayType = typeSymbol as IArrayTypeSymbol;
            if (arrayType != null)
            {
                AddParsedArrayTypeElement(sdType, typeSymbol);
            }

            var type = typeSymbol as INamedTypeSymbol;
            if (type != null)
            {
                AddParsedTypeArguments(sdType, type.TypeArguments);
            }

            AddParsedBaseType(sdType, typeSymbol.BaseType);
            AddParsedInterfaces(sdType, typeSymbol.Interfaces);            
        }

        private void AddParsedArrayTypeElement(SDType sdType, ITypeSymbol typeSymbol)
        {
            var arrayType = typeSymbol as IArrayTypeSymbol;
            if (arrayType != null)
            {
                sdType.ArrayElementType = GetParsedType(arrayType.ElementType);
            }
        }

        private void AddParsedBaseType(SDType sdType, INamedTypeSymbol baseType)
        {
            if (baseType != null && baseType.TypeKind != TypeKind.Interface)
            {
                var type = GetParsedType(baseType);
                if (sdType.BaseTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                {
                    sdType.BaseTypes.Add(type);
                }
            }
        }

        private void AddParsedNestedTypes(SDType sdType, IEnumerable<INamedTypeSymbol> nestedTypes)
        {
            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.TypeKind != TypeKind.Interface)
                {
                    var type = GetParsedType(nestedType);
                    if (sdType.NestedTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.NestedTypes.Add(type);
                    }
                }
            }
        }

        private void AddParsedInterfaces(SDType sdType, IEnumerable<INamedTypeSymbol> implementedInterfaces)
        {
            foreach (var implementedInterface in implementedInterfaces)
            {
                if (implementedInterface.TypeKind == TypeKind.Interface)
                {
                    var type = GetParsedType(implementedInterface);
                    if (sdType.ImplementedInterfaces.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.ImplementedInterfaces.Add(type);
                    }
                }
            }
        }

        private void AddParsedTypeArguments(SDType sdType, IEnumerable<ITypeSymbol> typeArguments)
        {
            foreach (var typeArgument in typeArguments)
            {
                var type = GetParsedType(typeArgument);
                if (sdType.TypeArguments.SingleOrDefault((i => i.Identifier == type.Identifier)) == null)
                {
                    sdType.TypeArguments.Add(GetParsedType(typeArgument));
                }
            }
        }

        private void AddParsedTypeParameters(SDType sdType, IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            foreach (var typeParameter in typeParameters)
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
                    sdTypeParameter.ConstraintTypes.Add(GetParsedType(constraintType));
                }

                if (sdType.TypeParameters.SingleOrDefault((i => i.Name == sdTypeParameter.Name)) == null)
                {
                    sdType.TypeParameters.Add(sdTypeParameter);
                }
            }
        }

        private void AddParsedProperties(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            _propertyParser.ParseProperties(sdType, typeSymbol);
        }

        private void AddParsedFields(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            _fieldParser.ParseFields(sdType, typeSymbol);
        }

        private void AddParsedConstructorsAndMethods(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            _methodParser.ParseConstructors(sdType, typeSymbol);
            _methodParser.ParseMethods(sdType, typeSymbol);
        }

        private void AddParsedEvents(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            _eventParser.ParseEvents(sdType, typeSymbol);
        }

        private SDType CreateSDType(ITypeSymbol typeSymbol, bool isProjectStranger)
        {
            SDNamespace sdNamespace = null;

            if (typeSymbol is IArrayTypeSymbol)
            {
                sdNamespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(((IArrayTypeSymbol)typeSymbol).ElementType.ContainingNamespace.GetIdentifier());
                sdNamespace = sdNamespace ?? new SDNamespace(((IArrayTypeSymbol)typeSymbol).ElementType.ContainingNamespace.GetIdentifier()) { IsProjectStranger = true };
            }
            else
            {
                sdNamespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(typeSymbol.ContainingNamespace.GetIdentifier());
                sdNamespace = sdNamespace ?? new SDNamespace(typeSymbol.ContainingNamespace.GetIdentifier()) { IsProjectStranger = true };
            }

            var sdType = new SDType(typeSymbol.GetIdentifier(), typeSymbol.Name, sdNamespace)
            {
                Accessibility = typeSymbol.DeclaredAccessibility.ToString().ToLower(),
                IsAbstract = typeSymbol.IsAbstract,
                IsReferenceType = typeSymbol.IsReferenceType,
                IsSealed = typeSymbol.IsSealed,
                IsStatic = typeSymbol.IsStatic,
                IsProjectStranger = isProjectStranger,
                Kind = typeSymbol.TypeKind.ToString().ToLower()
            };

            var declaringReferences = !isProjectStranger ? typeSymbol.DeclaringSyntaxReferences.ToList() : new List<SyntaxReference>();
            foreach (var reference in declaringReferences)
            {
                var region = new SDRegion
                {
                    Start = reference.Span.Start,
                    End = reference.Span.End,
                    Filename = reference.SyntaxTree.FilePath
                };
                sdType.Regions.Add(region);
            }

            ParserOptions.SDRepository.AddType(sdType);

            return sdType;
        }
    }
}
