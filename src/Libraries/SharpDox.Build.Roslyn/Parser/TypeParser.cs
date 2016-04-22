using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Sdk.Config;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class TypeParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;
        private readonly EventParser _eventParser;
        private readonly FieldParser _fieldParser;
        private readonly MethodParser _methodParser;
        private readonly PropertyParser _propertyParser;

        internal TypeParser(ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = new TypeRefParser(this, parserOptions);
            _eventParser = new EventParser(_typeRefParser, parserOptions);
            _fieldParser = new FieldParser(_typeRefParser, parserOptions);
            _methodParser = new MethodParser(_typeRefParser, parserOptions);
            _propertyParser = new PropertyParser(_typeRefParser, parserOptions);
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
            var type = typeSymbol as INamedTypeSymbol;
            if (type != null)
            {
                AddParsedTypeArguments(sdType, type.TypeArguments);
            }

            AddParsedBaseType(sdType, typeSymbol.BaseType);
            AddParsedInterfaces(sdType, typeSymbol.Interfaces);            
        }

        private void AddParsedBaseType(SDType sdType, INamedTypeSymbol baseType)
        {
            if (baseType != null && baseType.TypeKind != TypeKind.Interface)
            {
                var typeRef = _typeRefParser.GetParsedTypeReference(baseType);
                if (sdType.BaseTypes.SingleOrDefault((i => i.Type.Identifier == typeRef.Type.Identifier)) == null && typeRef.Type.Fullname != "System.Object")
                {
                    sdType.BaseTypes.Add(typeRef);
                }
            }
        }

        private void AddParsedNestedTypes(SDType sdType, IEnumerable<INamedTypeSymbol> nestedTypes)
        {
            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.TypeKind != TypeKind.Interface)
                {
                    var typeRef = _typeRefParser.GetParsedTypeReference(nestedType);
                    if (sdType.NestedTypes.SingleOrDefault((i => i.Type.Identifier == typeRef.Type.Identifier)) == null && typeRef.Type.Fullname != "System.Object")
                    {
                        sdType.NestedTypes.Add(typeRef);
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
                    var typeRef = _typeRefParser.GetParsedTypeReference(implementedInterface);
                    if (sdType.ImplementedInterfaces.SingleOrDefault((i => i.Type.Identifier == typeRef.Type.Identifier)) == null && typeRef.Type.Fullname != "System.Object")
                    {
                        sdType.ImplementedInterfaces.Add(typeRef);
                    }
                }
            }
        }

        private void AddParsedTypeArguments(SDType sdType, IEnumerable<ITypeSymbol> typeArguments)
        {
            foreach (var typeArgument in typeArguments)
            {
                var typeRef = _typeRefParser.GetParsedTypeReference(typeArgument);
                if (sdType.TypeArguments.SingleOrDefault((i => i.Type.Identifier == typeRef.Type.Identifier)) == null)
                {
                    sdType.TypeArguments.Add(typeRef);
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
                    sdTypeParameter.ConstraintTypes.Add(_typeRefParser.GetParsedTypeReference(constraintType));
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
