using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Linq;
using SharpDox.Sdk.Config;
using Microsoft.CodeAnalysis;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class EventParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal EventParser(TypeParser typeParser, SDRepository sdRepository, ICoreConfigSection sharpDoxConfig) : base(sdRepository, sharpDoxConfig)
        {
            _typeParser = typeParser;
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
                DeclaringType = _typeParser.GetParsedType(eve.ContainingType),
                Accessibility = eve.DeclaredAccessibility.ToString().ToLower(),
                Documentations = DocumentationParser.ParseDocumentation(eve.GetDocumentationCommentXml())
            };

            SDRepository.AddMember(sdEvent);
            return sdEvent;
        }

        internal static void ParseMinimalFields(SDType sdType, INamedTypeSymbol typeSymbol)
        {
            var events = typeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Event).Select(m => m as IEventSymbol);
            foreach (var eve in events)
            {
                var parsedEvent = GetMinimalParsedEvent(eve);
                if (sdType.Events.SingleOrDefault(f => f.Name == parsedEvent.Name) == null)
                {
                    sdType.Events.Add(parsedEvent);
                }
            }
        }

        private static SDEvent GetMinimalParsedEvent(IEventSymbol eve)
        {
            return new SDEvent(eve.GetIdentifier())
            {
                Name = eve.Name,
                Accessibility = eve.DeclaredAccessibility.ToString().ToLower()
            };
        }
    }
}
