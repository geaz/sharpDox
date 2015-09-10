using SharpDox.Sdk.Local;

namespace SharpDox.Core.Local
{
    public class LocalStringsItem
    {
        public LocalStringsItem(string language, ILocalStrings localStrings)
        {
            Language = language;
            LocalStrings = localStrings;
        }

        public string Language { get; private set; }
        public ILocalStrings LocalStrings { get; private set; }        
    }
}
