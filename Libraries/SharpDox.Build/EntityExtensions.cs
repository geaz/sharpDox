using ICSharpCode.NRefactory.TypeSystem;
using System.Linq;

namespace SharpDox.Build
{
    internal static class EntityExtensions
    {
        public static string GetIdentifier(this IType type)
        {
            var namespaceFullname = type.Namespace;
            var name = type.Name;
            var typeParams = string.Join(", ", type.TypeArguments.Select(t => t.FullName));
            typeParams = typeParams != string.Empty ? "<" + typeParams + ">" : typeParams;

            return string.Format("{0}.{1}{2}", namespaceFullname, name, typeParams);
        }

        public static string GetIdentifier(this IMethod method)
        {
            var name = method.Name;

            var typeParams = string.Join(", ", method.TypeArguments.Select(t => t.FullName));
            typeParams = typeParams != string.Empty ? "<" + typeParams + ">" : typeParams;

            var parameters = string.Join(", ", method.Parameters.Select(i => i.Type.FullName));
            parameters = parameters != string.Empty ? "(" + parameters + ")" : parameters;

            return string.Format("{0}.{1}{2}{3}", method.DeclaringType.GetIdentifier(), name, typeParams, parameters);
        }

        public static string GetIdentifier(this IEvent eve)
        {
            return string.Format("{0}.{1}", eve.DeclaringType.GetIdentifier(), eve.Name);
        }

        public static string GetIdentifier(this IField field)
        {
            return string.Format("{0}.{1}", field.DeclaringType.GetIdentifier(), field.Name);
        }

        public static string GetIdentifier(this IProperty property)
        {
            return string.Format("{0}.{1}", property.DeclaringType.GetIdentifier(), property.Name);
        }
    } 
}