using SharpDox.Model.Repository;
using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class BaseParser
    {
        internal event Action<string> OnItemParseStart;
        internal event Action<string> OnDocLanguageFound;

        protected readonly SDRepository _repository;
        protected readonly ICoreConfigSection _sharpDoxConfig; 
        protected readonly DocumentationParser _documentationParser;

        internal BaseParser(SDRepository repository, ICoreConfigSection sharpDoxConfig = null)
        {
            _repository = repository;
            _sharpDoxConfig = sharpDoxConfig;
            _documentationParser = new DocumentationParser();
        }

        protected bool IsMemberExcluded(string identifier, string accessibility)
        {
            var isExcluded = _sharpDoxConfig.ExcludedIdentifiers.Contains(identifier);
            isExcluded = accessibility.ToLower() == "private" && _sharpDoxConfig.ExcludePrivate || isExcluded;
            isExcluded = accessibility.ToLower() == "protected" && _sharpDoxConfig.ExcludeProtected || isExcluded;
            isExcluded = accessibility.ToLower() == "internal" && _sharpDoxConfig.ExcludeInternal || isExcluded;

            return isExcluded;
        }

        protected void HandleOnItemParseStart(string message)
        {
            var handlers = OnItemParseStart;
            if (handlers != null)
            {
                handlers(message);
            }
        }

        protected void ExecuteOnDocLanguageFound(string lang)
        {
            var handlers = OnDocLanguageFound;
            if (handlers != null)
            {
                handlers(lang);
            }
        }
    }
}
