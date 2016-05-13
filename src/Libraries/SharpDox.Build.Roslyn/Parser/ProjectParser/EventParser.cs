using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class EventParser : BaseParser
    {
        private readonly TypeRefParser _typeRefParser;

        internal EventParser(TypeRefParser typeRefParser, ParserOptions parserOptions) : base(parserOptions)
        {
            _typeRefParser = typeRefParser;
        }

        internal void ParseEvents(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var events = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Event).Select(m => m as IEventSymbol);
            foreach (var eve in events)
            {
                if (!IsMemberExcluded(eve.GetIdentifier(), eve.DeclaredAccessibility))
                {
                    var parsedEvent = GetParsedEvent(eve);
                    if (sdType.Events.SingleOrDefault(f => f.Name == parsedEvent.Name) == null)
                    {
                        sdType.Events.Add(parsedEvent);
                    }
                }
            }
        }

        private SDEvent GetParsedEvent(IEventSymbol eve)
        {
            var sdEvent = new SDEvent(eve.GetIdentifier())
            {
                Name = eve.Name,
                DeclaringType = _typeRefParser.GetParsedTypeReference(eve.ContainingType),
                Accessibility = eve.DeclaredAccessibility.ToString().ToLower(),
                Documentations = DocumentationParser.ParseDocumentation(eve)
            };

            ParserOptions.SDRepository.AddMember(sdEvent);
            return sdEvent;
        }
    }
}
