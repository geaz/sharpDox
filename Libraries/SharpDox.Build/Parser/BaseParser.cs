using System.Collections.Generic;
using SharpDox.Model.Repository;
using System;

namespace SharpDox.Build.Parser
{
    internal class BaseParser
    {
        internal event Action<string, int, int> OnItemParseStart;

        protected readonly SDRepository _repository;
        protected readonly List<string> _excludedIdentifiers; 
        protected readonly DocumentationParser _documentationParser;

        internal BaseParser(SDRepository repository, List<string> excludedIdentifiers = null)
        {
            _repository = repository;
            _excludedIdentifiers = excludedIdentifiers;
            _documentationParser = new DocumentationParser(repository);
        }

        protected void HandleOnItemParseStart(string message, int itemIndex, int itemsCount)
        {
            var handlers = OnItemParseStart;
            if (handlers != null)
            {
                handlers(message, itemIndex, itemsCount);
            }
        }
    }
}
