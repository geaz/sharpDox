using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class BaseParser
    {
        internal event Action<string> OnItemParseStart;
        internal event Action<string> OnDocLanguageFound;
        
        protected readonly ICoreConfigSection SharpDoxConfig; 

        internal BaseParser(ICoreConfigSection sharpDoxConfig = null)
        {
            SharpDoxConfig = sharpDoxConfig;
        }

        protected bool IsMemberExcluded(string identifier, string accessibility)
        {
            var isExcluded = SharpDoxConfig.ExcludedIdentifiers.Contains(identifier);
            isExcluded = accessibility.ToLower() == "private" && SharpDoxConfig.ExcludePrivate || isExcluded;
            isExcluded = accessibility.ToLower() == "protected" && SharpDoxConfig.ExcludeProtected || isExcluded;
            isExcluded = accessibility.ToLower() == "internal" && SharpDoxConfig.ExcludeInternal || isExcluded;

            return isExcluded;
        }

        protected void HandleOnItemParseStart(string message)
        {
            var handlers = OnItemParseStart;
            handlers?.Invoke(message);
        }

        protected void ExecuteOnDocLanguageFound(string lang)
        {
            var handlers = OnDocLanguageFound;
            handlers?.Invoke(lang);
        }
    }
}
