using System;
using Microsoft.CodeAnalysis;
using SharpDox.Build.Roslyn.Parser.ProjectParser;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class BaseParser
    {
        internal event Action<string> OnItemParseStart;
        internal event Action<string> OnDocLanguageFound;

        protected readonly ParserOptions ParserOptions;
        protected readonly DocumentationParser DocumentationParser;

        internal BaseParser(ParserOptions parserOptions)
        {
            ParserOptions = parserOptions;
            DocumentationParser = new DocumentationParser(parserOptions.SeeTokens);
        }

        protected bool IsMemberExcluded(string identifier, Accessibility accessibility)
        {
            var isExcluded = ParserOptions.SharpDoxConfig.ExcludedIdentifiers.Contains(identifier);
            isExcluded = accessibility == Accessibility.Private && ParserOptions.SharpDoxConfig.ExcludePrivate || isExcluded;
            isExcluded = accessibility == Accessibility.Protected && ParserOptions.SharpDoxConfig.ExcludeProtected || isExcluded;
            isExcluded = accessibility == Accessibility.Internal && ParserOptions.SharpDoxConfig.ExcludeInternal || isExcluded;

            return isExcluded && !ParserOptions.IgnoreExcludes;
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
