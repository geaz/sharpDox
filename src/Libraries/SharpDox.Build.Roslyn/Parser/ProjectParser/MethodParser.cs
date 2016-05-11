using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class MethodParser : BaseParser
    {
        private readonly TypeParameterParser _typeParameterParser;
        private readonly TypeRefParser _typeRefParser;

        internal MethodParser(TypeParameterParser typeParameterParser, TypeRefParser typeRefParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeParameterParser = typeParameterParser;
            _typeRefParser = typeRefParser;
        }

        internal void ParseConstructors(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var constructors = typeSymbol.Constructors.ToList();
            constructors = constructors.Where(o => !o.ContainingType.GetIdentifier().StartsWith("System.Object") && !o.IsImplicitlyDeclared).ToList();
            ParseMethodList(sdType.Constructors, constructors, true);
        }

        internal void ParseMethods(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var methods = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method && !m.IsImplicitlyDeclared).Select(f => f as IMethodSymbol);
            methods = methods.Where(o => !o.ContainingType.GetIdentifier().StartsWith("System.Object") && o.MethodKind == MethodKind.Ordinary);
            ParseMethodList(sdType.Methods, methods, false);
        }

        private void ParseMethodList(SortedList<SDMethod> sdMethodList, IEnumerable<IMethodSymbol> methodList, bool isCtor)
        {
            foreach (var method in methodList)
            {
                if (sdMethodList.SingleOrDefault((i => i.Identifier == method.GetIdentifier())) == null
                    && !IsMemberExcluded(method.GetIdentifier(), method.DeclaredAccessibility))
                {
                    var sdMethod = GetParsedMethod(method, isCtor);
                    sdMethodList.Add(sdMethod);
                }
            }
        }

        private SDMethod GetParsedMethod(IMethodSymbol method, bool isCtor)
        {
            var sdMethod = ParserOptions.SDRepository.GetMethodByIdentifier(method.GetIdentifier());
            if (sdMethod != null)
            {
                return sdMethod;
            }

            var returnType = _typeRefParser.GetParsedTypeReference(method.ReturnType);
            var syntaxReference = method.DeclaringSyntaxReferences.Any() ? method.DeclaringSyntaxReferences.Single() : null;
            sdMethod = new SDMethod(method.GetIdentifier(), isCtor ? method.ContainingType.Name : method.Name)
            {
                Namespace = method.ContainingNamespace.GetIdentifier(),
                DeclaringType = _typeRefParser.GetParsedTypeReference(method.ContainingType),
                ReturnType = returnType,
                IsCtor = isCtor,
                Accessibility = method.DeclaredAccessibility.ToString().ToLower(),
                IsAbstract = method.IsAbstract,
                IsOverride = method.IsOverride,
                IsPrivate = method.DeclaredAccessibility == Accessibility.Private,
                IsProtected = method.DeclaredAccessibility == Accessibility.Protected,
                IsPublic = method.DeclaredAccessibility == Accessibility.Public,
                IsSealed = method.IsSealed,
                IsVirtual = method.IsVirtual,
                IsStatic = method.IsStatic,
                Documentations = DocumentationParser.ParseDocumentation(method),
                Region = syntaxReference != null ? new SDRegion
                {
                    Start = syntaxReference.Span.Start,
                    End = syntaxReference.Span.End,
                    Filename = syntaxReference.SyntaxTree.FilePath
                } : null
            };

            sdMethod.TypeParameters = _typeParameterParser.ParseTypeParameters(method.TypeParameters);

            foreach (var parameter in method.Parameters)
            {
                sdMethod.Parameters.Add(new SDParameter
                {
                    Name = parameter.Name,
                    ParamType = _typeRefParser.GetParsedTypeReference(parameter.Type),
                    IsOptional = parameter.IsOptional,
                    IsConst = parameter.HasExplicitDefaultValue,
                    ConstantValue = parameter.HasExplicitDefaultValue ? parameter.ExplicitDefaultValue?.ToString() ?? "null" : null,
                    IsRef = parameter.RefKind == RefKind.Ref,
                    IsOut = parameter.RefKind == RefKind.Out
                });
            }

            ParserOptions.SDRepository.AddMethod(sdMethod);
            return sdMethod;
        }
    }
}
