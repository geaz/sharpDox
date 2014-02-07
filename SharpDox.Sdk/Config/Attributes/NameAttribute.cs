using System;

namespace SharpDox.Sdk.Config.Attributes
{
    public class NameAttribute : Attribute
    {
        public NameAttribute(Type localType, string displayName)
        {
            LocalType = localType;
            DisplayName = displayName;
        }

        public Type LocalType { get; set; }
        public string DisplayName { get; set; }
    }
}
