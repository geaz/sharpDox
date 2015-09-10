using SharpDox.Model.Repository;

namespace SharpDox.Model
{
    public static class KnownReferences
    {
        public static void AddKnownNamespaces(SDRepository sdRepository)
        {
            sdRepository.AddNamespace(new SDNamespace("System") { IsProjectStranger = true });
        }

        public static void AddKnownTypes(SDRepository sdRepository)
        {
            var systemNamespace = sdRepository.GetNamespaceByIdentifier("System");
            if(systemNamespace != null)
            {
                sdRepository.AddType(new SDType("System.Object", "Object", systemNamespace) { IsProjectStranger = true, CSharpName = "object" });
                sdRepository.AddType(new SDType("System.Boolean", "Boolean", systemNamespace) { IsProjectStranger = true, CSharpName = "bool" });
                sdRepository.AddType(new SDType("System.Char", "Char", systemNamespace) { IsProjectStranger = true, CSharpName = "char" });
                sdRepository.AddType(new SDType("System.SByte", "SByte", systemNamespace) { IsProjectStranger = true, CSharpName = "sbyte" });
                sdRepository.AddType(new SDType("System.Byte", "Byte", systemNamespace) { IsProjectStranger = true, CSharpName = "byte" });
                sdRepository.AddType(new SDType("System.Int16", "Int16", systemNamespace) { IsProjectStranger = true, CSharpName = "short" });
                sdRepository.AddType(new SDType("System.UInt16", "UInt16", systemNamespace) { IsProjectStranger = true, CSharpName = "ushort" });
                sdRepository.AddType(new SDType("System.Int32", "Int32", systemNamespace) { IsProjectStranger = true, CSharpName = "int" });
                sdRepository.AddType(new SDType("System.UInt32", "UInt32", systemNamespace) { IsProjectStranger = true, CSharpName = "uint" });
                sdRepository.AddType(new SDType("System.Int64", "Int64", systemNamespace) { IsProjectStranger = true, CSharpName = "long" });
                sdRepository.AddType(new SDType("System.UInt64", "UInt64", systemNamespace) { IsProjectStranger = true, CSharpName = "ulong" });
                sdRepository.AddType(new SDType("System.Single", "Single", systemNamespace) { IsProjectStranger = true, CSharpName = "float" });
                sdRepository.AddType(new SDType("System.Double", "Double", systemNamespace) { IsProjectStranger = true, CSharpName = "double" });
                sdRepository.AddType(new SDType("System.Decimal", "Decimal", systemNamespace) { IsProjectStranger = true, CSharpName = "decimal" });
                sdRepository.AddType(new SDType("System.String", "String", systemNamespace) { IsProjectStranger = true, CSharpName = "string" });
                sdRepository.AddType(new SDType("System.Void", "Void", systemNamespace) { IsProjectStranger = true, CSharpName = "void" });
            }
        }
    }
}
