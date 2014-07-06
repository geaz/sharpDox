using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Linq;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class EventParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal EventParser(SDRepository repository, TypeParser typeParser, ICoreConfigSection sharpDoxConfig) : base(repository, sharpDoxConfig)
        {
            _typeParser = typeParser;
        }

        internal void ParseEvents(SDType sdType, IType type)
        {
            var events = type.GetEvents(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var eve in events)
            {
                if (!IsMemberExcluded(eve.GetIdentifier(), eve.Accessibility.ToString()))
                {
                    var parsedEvent = GetParsedEvent(eve);
                    if (sdType.Events.SingleOrDefault(f => f.Name == parsedEvent.Name) == null)
                    {
                        sdType.Events.Add(parsedEvent);
                    }
                }
            }
        }

        private SDEvent GetParsedEvent(IEvent eve)
        {
            var sdEvent = new SDEvent(eve.GetIdentifier())
            {
                Name = eve.Name,
                DeclaringType = _typeParser.GetParsedType(eve.DeclaringType),
                Accessibility = eve.Accessibility.ToString().ToLower(),
                Documentation = _documentationParser.ParseDocumentation(eve)
            };

            _repository.AddMember(sdEvent);
            return sdEvent;
        }

        internal static void ParseMinimalFields(SDType sdType, IType type)
        {
            var events = type.GetEvents(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var eve in events)
            {
                var parsedEvent = GetMinimalParsedEvent(eve);
                if (sdType.Events.SingleOrDefault(f => f.Name == parsedEvent.Name) == null)
                {
                    sdType.Events.Add(parsedEvent);
                }
            }
        }

        private static SDEvent GetMinimalParsedEvent(IEvent eve)
        {
            return new SDEvent(eve.GetIdentifier())
            {
                Name = eve.Name,
                Accessibility = eve.Accessibility.ToString().ToLower()
            };
        }
    }
}
