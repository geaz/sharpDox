using SharpDox.Sdk.Local;

namespace SharpDox.Local
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
