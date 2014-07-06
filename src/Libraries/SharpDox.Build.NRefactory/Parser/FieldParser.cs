using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Model.Repository;
using SharpDox.Model.Repository.Members;
using System.Linq;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class FieldParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        internal FieldParser(SDRepository repository, TypeParser typeParser, ICoreConfigSection sharpDoxConfig) : base(repository, sharpDoxConfig)
        {
            _typeParser = typeParser;
        }

        internal void ParseFields(SDType sdType, IType type)
        {
            var fields = type.GetFields(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var field in fields)
            {
                if (!IsMemberExcluded(field.GetIdentifier(), field.Accessibility.ToString()))
                {
                    var parsedField = GetParsedField(field);
                    if (sdType.Fields.SingleOrDefault(f => f.Name == parsedField.Name) == null)
                    {
                        sdType.Fields.Add(parsedField);
                    }
                }
            }
        }

        private SDField GetParsedField(IField field)
        {
            var sdField = new SDField(field.GetIdentifier())
            {
                Name = field.Name,
                DeclaringType = _typeParser.GetParsedType(field.DeclaringType),
                Accessibility = field.Accessibility.ToString().ToLower(),
                ReturnType = _typeParser.GetParsedType(field.ReturnType),
                ConstantValue = field.ConstantValue != null ? field.ConstantValue.ToString() : string.Empty,
                IsConst = field.IsConst,
                IsReadonly = field.IsReadOnly,
                Documentation = _documentationParser.ParseDocumentation(field)
            };

            _repository.AddMember(sdField);
            return sdField;
        }

        internal static void ParseMinimalFields(SDType sdType, IType type)
        {
            var fields = type.GetFields(null, GetMemberOptions.IgnoreInheritedMembers);
            foreach (var field in fields)
            {
                var parsedField = GetMinimalParsedField(field);
                if (sdType.Fields.SingleOrDefault(f => f.Name == parsedField.Name) == null)
                {
                    sdType.Fields.Add(parsedField);
                }
            }
        }

        private static SDField GetMinimalParsedField(IField field)
        {
            return new SDField(field.GetIdentifier())
            {
                Name = field.Name,
                Accessibility = field.Accessibility.ToString().ToLower()
            };
        }
    }
}
