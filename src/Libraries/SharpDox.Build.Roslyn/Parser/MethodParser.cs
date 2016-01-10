using SharpDox.Model;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Sdk.Config;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class MethodParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal MethodParser(SDRepository sdRepository, TypeParser typeParser, ICoreConfigSection sharpDoxConfig) : base(sdRepository, sharpDoxConfig)
        {
            _typeParser = typeParser;
        }

        internal void ParseConstructors(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var constructors = typeSymbol.Constructors.ToList();
            constructors = constructors.Where(o => !o.ContainingType.ToDisplayString().StartsWith("System.Object")).ToList();
            ParseMethodList(sdType.Constructors, constructors, true);
        }

        internal void ParseMethods(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var methods = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method).Select(f => f as IMethodSymbol);
            methods = methods.Where(o => !o.ContainingType.ToDisplayString().StartsWith("System.Object"));
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
            var sdMethod = SDRepository.GetMethodByIdentifier(method.GetIdentifier());
            if (sdMethod != null)
            {
                return sdMethod;
            }

            var returnType = _typeParser.GetParsedType(method.ReturnType);

            sdMethod = new SDMethod(method.GetIdentifier(), isCtor ? method.ContainingType.Name : method.Name)
            {
                Namespace = method.ContainingNamespace.ToDisplayString(),
                DeclaringType = _typeParser.GetParsedType(method.ContainingType),
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
                Documentations = DocumentationParser.ParseDocumentation(method.GetDocumentationCommentXml()),
                Region = new SDRegion
                {
                  /*  BeginColumn = method.Region.BeginColumn,
                    BeginLine = method.Region.BeginLine,
                    EndColumn = method.Region.EndColumn,
                    EndLine = method.Region.EndLine,
                    Filename = method.Region.FileName*/ //TODO
                }
            };

            foreach (var typeParameter in method.TypeParameters)
            {
                sdMethod.TypeParameters.Add(_typeParser.GetParsedType(typeParameter));
            }

            foreach (var parameter in method.Parameters)
            {
                sdMethod.Parameters.Add(new SDParameter
                {
                    Name = parameter.Name,
                    ParamType = _typeParser.GetParsedType(parameter.Type),
                    IsOptional = parameter.IsOptional,
                    IsConst = parameter.HasExplicitDefaultValue,
                    ConstantValue = parameter.ExplicitDefaultValue.ToString(),
                    IsRef = parameter.RefKind == RefKind.Ref,
                    IsOut = parameter.RefKind == RefKind.Out
                });
            }

            SDRepository.AddMethod(sdMethod);
            return sdMethod;
        }

        internal static void ParseMinimalConstructors(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var constructors = typeSymbol.Constructors.ToList();
            constructors = constructors.Where(o => !o.ContainingType.ToDisplayString().StartsWith("System.Object")).ToList();
            MinimalParseMethodList(sdType.Constructors, constructors, true);
        }

        internal static void ParseMinimalMethods(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var methods = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method).Select(f => f as IMethodSymbol);
            methods = methods.Where(o => !o.ContainingType.ToDisplayString().StartsWith("System.Object"));
            MinimalParseMethodList(sdType.Methods, methods, false);
        }

        private static void MinimalParseMethodList(SortedList<SDMethod> sdMethods, IEnumerable<IMethodSymbol> methods, bool isCtor)
        {
            foreach (var method in methods)
            {
                var parsedMethod = GetMinimalParsedMethod(method, isCtor);
                if (sdMethods.SingleOrDefault(f => f.Name == parsedMethod.Name) == null)
                {
                    sdMethods.Add(parsedMethod);
                }
            }
        }

        private static SDMethod GetMinimalParsedMethod(IMethodSymbol method, bool isCtor)
        {
            return new SDMethod(method.GetIdentifier(), isCtor ? method.ContainingType.Name : method.Name)
            {
                IsCtor = isCtor,
                Accessibility = method.DeclaredAccessibility.ToString().ToLower()
            };
        }
    }
}
