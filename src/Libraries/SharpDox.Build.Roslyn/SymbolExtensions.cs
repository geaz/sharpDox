using Microsoft.CodeAnalysis;
using System.Linq;

namespace SharpDox.Build.Roslyn
{
    internal static class EntityExtensions
    {
        public static string GetIdentifier(this INamespaceSymbol namespaceSymbol)
        {
            var namespaceIdentifier = namespaceSymbol.ToDisplayString();
            if (namespaceSymbol.IsGlobalNamespace)
            {
                namespaceIdentifier = "GlobalNamespace";
            }
            return namespaceIdentifier;
        }

        public static string GetIdentifier(this ITypeSymbol typeSymbol)
        {
            var identifier = string.Empty;

            var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                var namespaceFullname = typeSymbol.ContainingNamespace.GetIdentifier();
                var name = typeSymbol.Name;
                var typeParams = string.Join(", ", namedTypeSymbol.TypeArguments.Select(t => t.GetIdentifier()));
                typeParams = typeParams != string.Empty ? "<" + typeParams + ">" : typeParams;

                identifier = $"{namespaceFullname}.{name}{typeParams}";
            }

            var arrayTypeSymbol = typeSymbol as IArrayTypeSymbol;
            if (arrayTypeSymbol != null)
            {
                identifier = $"{arrayTypeSymbol.ElementType.GetIdentifier()}[]";
            }

            var typeParameterSymbol = typeSymbol as ITypeParameterSymbol;
            if(typeParameterSymbol != null)
            {
                identifier = typeParameterSymbol.Name;
            }

            return identifier;
        }

        public static string GetIdentifier(this IMethodSymbol method)
        {
            var name = method.Name;

            var typeParams = string.Join(", ", method.TypeArguments.Select(t => t.GetIdentifier()));
            typeParams = typeParams != string.Empty ? "<" + typeParams + ">" : typeParams;

            var parameters = string.Join(", ", method.Parameters.Select(i => i.Type.GetIdentifier()));
            parameters = parameters != string.Empty ? "(" + parameters + ")" : parameters;

            return string.Format("{0}.{1}{2}{3}", method.ContainingType.GetIdentifier(), name, typeParams, parameters);
        }

        public static string GetIdentifier(this IEventSymbol eve)
        {
            return string.Format("event.{0}.{1}", eve.ContainingType.GetIdentifier(), eve.Name);
        }

        public static string GetIdentifier(this IFieldSymbol field)
        {
            return string.Format("field.{0}.{1}", field.ContainingType.GetIdentifier(), field.Name);
        }

        public static string GetIdentifier(this IPropertySymbol property)
        {
            return string.Format("property.{0}.{1}", property.ContainingType.GetIdentifier(), property.Name);
        }
    } 
}