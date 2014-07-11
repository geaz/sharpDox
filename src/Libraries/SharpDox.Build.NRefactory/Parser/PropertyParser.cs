using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Linq;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class PropertyParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal PropertyParser(SDRepository repository, TypeParser typeParser, ICoreConfigSection sharpDoxConfig) : base(repository, sharpDoxConfig)
        {
            _typeParser = typeParser;
        }

        internal void ParseProperties(SDType sdType, IType type)
        {
            var properties = type.GetProperties(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var property in properties)
            {
                if (!IsMemberExcluded(property.GetIdentifier(), property.Accessibility.ToString()))
                {
                    var parsedProperty = GetParsedProperty(property);
                    if (sdType.Properties.SingleOrDefault(p => p.Name == parsedProperty.Name) == null)
                    {
                        sdType.Properties.Add(parsedProperty);
                    }
                }
            }
        }

        private SDProperty GetParsedProperty(IProperty property)
        {
            var sdProperty = new SDProperty(property.GetIdentifier())
            {
                Name = property.Name,
                DeclaringType = _typeParser.GetParsedType(property.DeclaringType),
                Accessibility = property.Accessibility.ToString().ToLower(),
                ReturnType = _typeParser.GetParsedType(property.ReturnType),
                CanGet = property.CanGet,
                CanSet = property.CanSet,
                IsAbstract = property.IsAbstract,
                IsVirtual = property.IsVirtual,
                IsOverride = property.IsOverride,
                Documentations = _documentationParser.ParseDocumentation(property)
            };

            _repository.AddMember(sdProperty);
            return sdProperty;
        }

        internal static void ParseMinimalProperties(SDType sdType, IType type)
        {
            var properties = type.GetProperties(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var property in properties)
            {
                var parsedProperty = GetMinimalParsedProperty(property);
                if (sdType.Properties.SingleOrDefault(p => p.Name == parsedProperty.Name) == null)
                {
                    sdType.Properties.Add(parsedProperty);
                }
            }
        }

        private static SDProperty GetMinimalParsedProperty(IProperty property)
        {
            return new SDProperty(property.GetIdentifier())
            {
                Name = property.Name,
                Accessibility = property.Accessibility.ToString().ToLower()
            };
        }
    }
}
