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

        internal TypeParser(SDRepository sdRepository, ICoreConfigSection sharpDoxConfig) : base(sdRepository, sharpDoxConfig)
        {
            _eventParser = new EventParser(this, sdRepository, sharpDoxConfig);
        }

        internal void ParseProjectTypes(List<INamedTypeSymbol> typeSymbols)
        {
            for (int i = 0; i < typeSymbols.Count; i++ )
            {
                HandleOnItemParseStart(typeSymbols[i].ToDisplayString());
                if (!IsMemberExcluded(typeSymbols[i].GetIdentifier(), typeSymbols[i].DeclaredAccessibility))
                {
                    var sdType = GetParsedType(typeSymbols[i], false);
                    SDRepository.AddNamespaceTypeRelation(typeSymbols[i].ContainingNamespace.ToDisplayString(), sdType.Identifier);
                }
            }
        }

        internal SDType GetParsedType(ITypeSymbol typeSymbol, bool isProjectStranger = true)
        {
            var parsedType = SDRepository.GetTypeByIdentifier(typeSymbol.GetIdentifier());
            if (parsedType == null)
            {
                parsedType = CreateSDType(typeSymbol, isProjectStranger);
               // ParseForeignTypeToModel(parsedType, typeSymbol);
            }

            if (!isProjectStranger)
            {
                //ParseTypeToModel(parsedType, typeSymbol);
            }

            return parsedType;
        }

        

        /*private void ParseTypeToModel(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            sdType.IsProjectStranger = false;
            AddParsedTypeArguments(sdType, typeSymbol.TypeArguments);
            AddParsedTypeParameters(sdType, typeSymbol.GetDefinition().TypeParameters);
            AddParsedNestedTypes(sdType, typeSymbol.GetNestedTypes());
            AddParsedBaseTypes(sdType, typeSymbol.DirectBaseTypes);
            AddParsedInterfaces(sdType, typeSymbol.DirectBaseTypes);            
            AddParsedProperties(sdType, typeSymbol);
            AddParsedFields(sdType, typeSymbol);
            AddParsedConstructorsAndMethods(sdType, typeSymbol);
            AddParsedEvents(sdType, typeSymbol);
        }

        private void ParseForeignTypeToModel(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            AddParsedArrayTypeElement(sdType, typeSymbol);
            AddParsedTypeArguments(sdType, typeSymbol.TypeArguments);
            AddParsedBaseTypes(sdType, typeSymbol.DirectBaseTypes);
            AddParsedInterfaces(sdType, typeSymbol.DirectBaseTypes);            
        }

        private void AddParsedArrayTypeElement(SDType sdType, ITypeSymbol typeSymbol)
        {
            var arrayType = typeSymbol as IArrayTypeSymbol;
            if (arrayType != null)
            {
                sdType.ArrayElementType = GetParsedType(arrayType.ElementType);
            }
        }

        private void AddParsedNestedTypes(SDType sdType, IEnumerable<INamedTypeSymbol> nestedTypes)
        {
            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.Kind != TypeKind.Interface)
                {
                    var type = GetParsedType(nestedType);
                    if (sdType.NestedTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.NestedTypes.Add(type);
                    }
                }
            }
        }

        private void AddParsedBaseTypes(SDType sdType, IEnumerable<IType> directBaseTypes)
        {
            foreach (var baseType in directBaseTypes)
            {
                if (baseType.Kind != TypeKind.Interface)
                {
                    var type = GetParsedType(baseType);
                    if (sdType.BaseTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.BaseTypes.Add(type);
                    }
                }
            }
        }

        private void AddParsedInterfaces(SDType sdType, IEnumerable<IType> implementedInterfaces)
        {
            foreach (var implementedInterface in implementedInterfaces)
            {
                if (implementedInterface.Kind == TypeKind.Interface)
                {
                    var type = GetParsedType(implementedInterface);
                    if (sdType.ImplementedInterfaces.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.ImplementedInterfaces.Add(type);
                    }
                }
            }
        }

        private void AddParsedTypeArguments(SDType sdType, IEnumerable<IType> typeArguments)
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

        private void AddParsedTypeParameters(SDType sdType, IEnumerable<ITypeParameter> typeParameters)
        {
            foreach (var typeParameter in typeParameters)
            {
                var sdTypeParameter = new SDTypeParameter
                {
                    Name = typeParameter.Name,
                    HasDefaultConstructorConstraint = typeParameter.HasDefaultConstructorConstraint,
                    HasReferenceTypeConstraint = typeParameter.HasReferenceTypeConstraint,
                    HasValueTypeConstraint = typeParameter.HasValueTypeConstraint,
                    BaseClass = GetParsedType(typeParameter.EffectiveBaseClass)
                };
                foreach (var interfaceConstraint in typeParameter.EffectiveInterfaceSet)
                {
                    sdTypeParameter.Interfaces.Add(GetParsedType(interfaceConstraint));
                }

                if (sdType.TypeParameters.SingleOrDefault((i => i.Name == sdTypeParameter.Name)) == null)
                {
                    sdType.TypeParameters.Add(sdTypeParameter);
                }
            }
        }

        private void AddParsedProperties(SDType sdType, IType type)
        {
            var propertyParser = new PropertyParser(_repository, this, _sharpDoxConfig);
            propertyParser.ParseProperties(sdType, type);
        }

        private void AddParsedFields(SDType sdType, IType type)
        {
            var fieldParser = new FieldParser(_repository, this, _sharpDoxConfig);
            fieldParser.ParseFields(sdType, type);
        }

        private void AddParsedConstructorsAndMethods(SDType sdType, IType type)
        {
            var methodParser = new MethodParser(_repository, this, _sharpDoxConfig);
            methodParser.ParseConstructors(sdType, type);
            methodParser.ParseMethods(sdType, type);
        }

        private void AddParsedEvents(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            _eventParser.ParseEvents(sdType, typeSymbol);
        }*/

        private SDType CreateSDType(ITypeSymbol typeSymbol, bool isProjectStranger)
        {
            var sdNamespace = SDRepository.GetNamespaceByIdentifier(typeSymbol.ContainingNamespace.ToDisplayString());
            sdNamespace = sdNamespace ?? new SDNamespace(typeSymbol.ContainingNamespace.ToDisplayString()) { IsProjectStranger = true };

            var sdType = new SDType(typeSymbol.GetIdentifier(), typeSymbol.Name, sdNamespace)
            {
                Accessibility = typeSymbol.DeclaredAccessibility.ToString().ToLower(),
                IsAbstract = typeSymbol.IsAbstract,
                IsReferenceType = typeSymbol.IsReferenceType,
                IsSealed = typeSymbol.IsSealed,
                IsStatic = typeSymbol.IsStatic,
                IsProjectStranger = isProjectStranger,
                Kind = typeSymbol.TypeKind.ToString().ToLower(),
                /*Region = new SDRegion 
                        { 
                            BeginColumn = typeDefinition.Region.BeginColumn,
                            BeginLine = typeDefinition.Region.BeginLine,
                            EndColumn = typeDefinition.Region.EndColumn,
                            EndLine = typeDefinition.Region.EndLine,
                            Filename = typeDefinition.Region.FileName
                        },*/ // TODO
                Documentations = DocumentationParser.ParseDocumentation(typeSymbol.GetDocumentationCommentXml())
            };

            SDRepository.AddType(sdType);

            return sdType;
        }
    }
}
