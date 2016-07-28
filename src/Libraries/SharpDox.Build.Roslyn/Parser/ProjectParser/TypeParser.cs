using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class TypeParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;
        private readonly TypeParameterParser _typeParameterParser;
        private readonly EventParser _eventParser;
        private readonly FieldParser _fieldParser;
        private readonly MethodParser _methodParser;
        private readonly PropertyParser _propertyParser;

        internal TypeParser(ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = new TypeRefParser(new StrangerTypeParser(parserOptions), parserOptions);
            _typeParameterParser = new TypeParameterParser(_typeRefParser, parserOptions);
            _eventParser = new EventParser(_typeRefParser, parserOptions);
            _fieldParser = new FieldParser(_typeRefParser, parserOptions);
            _methodParser = new MethodParser(_typeParameterParser, _typeRefParser, parserOptions);
            _propertyParser = new PropertyParser(_typeRefParser, parserOptions);
        }

        internal void ParseProjectTypes(List<INamedTypeSymbol> typeSymbols)
        {
            foreach (var typeSymbol in typeSymbols)
            {
                HandleOnItemParseStart(typeSymbol.GetIdentifier());
                if (!IsMemberExcluded(typeSymbol.GetIdentifier(), typeSymbol.DeclaredAccessibility))
                {
                    ParseTheProjectType(typeSymbol);
                }
            }
        }

        private void ParseTheProjectType(INamedTypeSymbol typeSymbol)
        {
            var sdNamespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(typeSymbol.ContainingNamespace.GetIdentifier());
            var sdType = ParserOptions.SDRepository.GetTypeByIdentifier(typeSymbol.GetIdentifier());
            if (sdType == null)
            {
                sdType = CreateSDType(typeSymbol, sdNamespace);
                sdNamespace.Types.Add(sdType);
                ParserOptions.SDRepository.AddType(sdType);
            }
            else // already parsed as stranger
            {
                sdType.Namespace = ParserOptions.SDRepository.GetNamespaceByIdentifier(typeSymbol.ContainingNamespace.GetIdentifier());
                sdType.IsProjectStranger = false;

                if(!sdNamespace.Types.Contains(sdType)) sdNamespace.Types.Add(sdType);
            }

            sdType.Documentations = DocumentationParser.ParseDocumentation(typeSymbol);
            AddParsedBaseType(sdType, typeSymbol.BaseType);
            AddParsedInterfaces(sdType, typeSymbol.Interfaces);
            AddParsedNestedTypes(sdType, typeSymbol.GetTypeMembers());

            _propertyParser.ParseProperties(sdType, typeSymbol);
            _fieldParser.ParseFields(sdType, typeSymbol);
            _methodParser.ParseConstructors(sdType, typeSymbol);
            _methodParser.ParseMethods(sdType, typeSymbol);
            _eventParser.ParseEvents(sdType, typeSymbol);

            sdType.TypeParameters = _typeParameterParser.ParseTypeParameters(typeSymbol.TypeParameters);
            foreach (var typeArgument in typeSymbol.TypeArguments)
            {
                sdType.TypeArguments.Add(_typeRefParser.GetParsedTypeReference(typeArgument));
            }
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

        private SDType CreateSDType(INamedTypeSymbol typeSymbol, SDNamespace sdNamespace)
        {
            var sdType = new SDType(typeSymbol.GetIdentifier(), typeSymbol.Name, sdNamespace)
            {
                Accessibility = typeSymbol.DeclaredAccessibility.ToString().ToLower(),
                IsAbstract = typeSymbol.IsAbstract,
                IsReferenceType = typeSymbol.IsReferenceType,
                IsSealed = typeSymbol.IsSealed,
                IsStatic = typeSymbol.IsStatic,
                IsProjectStranger = false,
                Kind = typeSymbol.TypeKind.ToString().ToLower()
            };
            
            foreach (var reference in typeSymbol.DeclaringSyntaxReferences.ToList())
            {
                var region = new SDRegion
                {
                    Start = reference.Span.Start,
                    End = reference.Span.End,
                    FilePath = reference.SyntaxTree.FilePath,
                    Filename = Path.GetFileName(reference.SyntaxTree.FilePath),
                    Content = File.ReadAllText(reference.SyntaxTree.FilePath)
                };
                sdType.Regions.Add(region);
            }
            return sdType;
        }
    }
}
