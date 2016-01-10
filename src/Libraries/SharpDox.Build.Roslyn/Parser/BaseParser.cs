using System;
using SharpDox.Sdk.Config;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class BaseParser
    {
        internal event Action<string> OnItemParseStart;
        internal event Action<string> OnDocLanguageFound;

        protected readonly SDRepository SDRepository;
        protected readonly ICoreConfigSection SharpDoxConfig;
        protected readonly DocumentationParser DocumentationParser;

        internal BaseParser(SDRepository repository, ICoreConfigSection sharpDoxConfig = null)
        {
            SDRepository = repository;
            SharpDoxConfig = sharpDoxConfig;
            DocumentationParser = new DocumentationParser();
        }

        protected bool IsMemberExcluded(string identifier, Accessibility accessibility)
        {
            var isExcluded = SharpDoxConfig.ExcludedIdentifiers.Contains(identifier);
            isExcluded = accessibility == Accessibility.Private && SharpDoxConfig.ExcludePrivate || isExcluded;
            isExcluded = accessibility == Accessibility.Protected && SharpDoxConfig.ExcludeProtected || isExcluded;
            isExcluded = accessibility == Accessibility.Internal && SharpDoxConfig.ExcludeInternal || isExcluded;

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
