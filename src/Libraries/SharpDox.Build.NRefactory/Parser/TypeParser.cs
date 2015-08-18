using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Linq;
using SharpDox.Sdk.Config;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class TypeParser : BaseParser
    {
        internal TypeParser(SDRepository repository, ICoreConfigSection sharpDoxConfig) : base(repository, sharpDoxConfig) { }

        internal void ParseProjectTypes(CSharpProject project)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int i = 0; i < types.Count; i++ )
            {
                if (types[i].Kind != TypeKind.Delegate)
                {
                    HandleOnItemParseStart(string.Format("{0}.{1}", types[i].Namespace, types[i].Name));
                    if (!IsMemberExcluded(types[i].GetIdentifier(), types[i].Accessibility.ToString()))
                    {
                        var sdType = GetParsedType(types[i].GetDefinition(), false);
                        _repository.AddNamespaceTypeRelation(types[i].Namespace, sdType.Identifier);
                    }
                }
            }
        }

        internal SDType GetParsedType(IType type, bool isProjectStranger = true)
        {
            var parsedType = _repository.GetTypeByIdentifier(type.GetIdentifier());
            if (parsedType == null)
            {
                parsedType = type.GetDefinition() != null ? CreateSDType(type, type.GetDefinition(), isProjectStranger) : CreateSDType(type, isProjectStranger);
                ParseForeignTypeToModel(parsedType, type);
            }

            if (!isProjectStranger)
            {
                ParseTypeToModel(parsedType, type);
            }

            return parsedType;
        }

        private void ParseTypeToModel(SDType sdType, IType type)
        {
            sdType.IsProjectStranger = false;
            AddParsedTypeArguments(sdType, type.TypeArguments);
            AddParsedTypeParameters(sdType, type.GetDefinition().TypeParameters);
            AddParsedNestedTypes(sdType, type.GetNestedTypes());
            AddParsedBaseTypes(sdType, type.DirectBaseTypes);
            AddParsedInterfaces(sdType, type.DirectBaseTypes);            
            AddParsedProperties(sdType, type);
            AddParsedFields(sdType, type);
            AddParsedConstructorsAndMethods(sdType, type);
            AddParsedEvents(sdType, type);
        }

        private void ParseForeignTypeToModel(SDType sdType, IType type)
        {
            AddParsedArrayTypeElement(sdType, type);
            AddParsedTypeArguments(sdType, type.TypeArguments);
            AddParsedBaseTypes(sdType, type.DirectBaseTypes);
            AddParsedInterfaces(sdType, type.DirectBaseTypes);            
        }

        private void AddParsedArrayTypeElement(SDType sdType, IType type)
        {
            var arrayType = type as ArrayType;
            if (arrayType != null)
            {
                sdType.ArrayElementType = GetParsedType(arrayType.ElementType);
            }
        }

        private void AddParsedNestedTypes(SDType sdType, IEnumerable<IType> nestedTypes)
        {
            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.Kind != TypeKind.Interface)
                {
                    var type = GetParsedType(nestedType);
                    if (sdType.NestedTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.NestedTypes.Add(type);
                    }
                }
            }
        }

        private void AddParsedBaseTypes(SDType sdType, IEnumerable<IType> directBaseTypes)
        {
            foreach (var baseType in directBaseTypes)
            {
                if (baseType.Kind != TypeKind.Interface)
                {
                    var type = GetParsedType(baseType);
                    if (sdType.BaseTypes.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.BaseTypes.Add(type);
                    }
                }
            }
        }

        private void AddParsedInterfaces(SDType sdType, IEnumerable<IType> implementedInterfaces)
        {
            foreach (var implementedInterface in implementedInterfaces)
            {
                if (implementedInterface.Kind == TypeKind.Interface)
                {
                    var type = GetParsedType(implementedInterface);
                    if (sdType.ImplementedInterfaces.SingleOrDefault((i => i.Identifier == type.Identifier)) == null && type.Fullname != "System.Object")
                    {
                        sdType.ImplementedInterfaces.Add(type);
                    }
                }
            }
        }

        private void AddParsedTypeArguments(SDType sdType, IEnumerable<IType> typeArguments)
        {
            foreach (var typeArgument in typeArguments)
            {
                var type = GetParsedType(typeArgument);
                if (sdType.TypeArguments.SingleOrDefault((i => i.Identifier == type.Identifier)) == null)
                {
                    sdType.TypeArguments.Add(GetParsedType(typeArgument));
                }
            }
        }

        private void AddParsedTypeParameters(SDType sdType, IEnumerable<ITypeParameter> typeParameters)
        {
            foreach (var typeParameter in typeParameters)
            {
                var sdTypeParameter = new SDTypeParameter
                {
                    Name = typeParameter.Name,
                    HasDefaultConstructorConstraint = typeParameter.HasDefaultConstructorConstraint,
                    HasReferenceTypeConstraint = typeParameter.HasReferenceTypeConstraint,
                    HasValueTypeConstraint = typeParameter.HasValueTypeConstraint,
                    BaseClass = GetParsedType(typeParameter.EffectiveBaseClass)
                };
                foreach (var interfaceConstraint in typeParameter.EffectiveInterfaceSet)
                {
                    sdTypeParameter.Interfaces.Add(GetParsedType(interfaceConstraint));
                }

                if (sdType.TypeParameters.SingleOrDefault((i => i.Name == sdTypeParameter.Name)) == null)
                {
                    sdType.TypeParameters.Add(sdTypeParameter);
                }
            }
        }

        private void AddParsedProperties(SDType sdType, IType type)
        {
            var propertyParser = new PropertyParser(_repository, this, _sharpDoxConfig);
            propertyParser.ParseProperties(sdType, type);
        }

        private void AddParsedFields(SDType sdType, IType type)
        {
            var fieldParser = new FieldParser(_repository, this, _sharpDoxConfig);
            fieldParser.ParseFields(sdType, type);
        }

        private void AddParsedConstructorsAndMethods(SDType sdType, IType type)
        {
            var methodParser = new MethodParser(_repository, this, _sharpDoxConfig);
            methodParser.ParseConstructors(sdType, type);
            methodParser.ParseMethods(sdType, type);
        }

        private void AddParsedEvents(SDType sdType, IType type)
        {
            var eventParser = new EventParser(_repository, this, _sharpDoxConfig);
            eventParser.ParseEvents(sdType, type);
        }

        private SDType CreateSDType(IType type, ITypeDefinition typeDefinition, bool isProjectStranger)
        {
            var nameSpace = _repository.GetNamespaceByIdentifier(typeDefinition.Namespace);
            var namespaceRef = nameSpace ?? new SDNamespace(typeDefinition.Namespace) { IsProjectStranger = true };

            var sdType = new SDType(type.GetIdentifier(), typeDefinition.Name, namespaceRef)
            {
                Accessibility = typeDefinition.Accessibility.ToString().ToLower(),
                IsAbstract = typeDefinition.IsAbstract,
                IsReferenceType = typeDefinition.IsReferenceType.GetValueOrDefault(),
                IsSealed = typeDefinition.IsSealed,
                IsShadowing = typeDefinition.IsShadowing,
                IsStatic = typeDefinition.IsStatic,
                IsSynthetic = typeDefinition.IsSynthetic,
                IsProjectStranger = isProjectStranger,
                Kind = typeDefinition.Kind.ToString().ToLower(),
                Region = new SDRegion 
                        { 
                            BeginColumn = typeDefinition.Region.BeginColumn,
                            BeginLine = typeDefinition.Region.BeginLine,
                            EndColumn = typeDefinition.Region.EndColumn,
                            EndLine = typeDefinition.Region.EndLine,
                            Filename = typeDefinition.Region.FileName
                        },
                Documentations = _documentationParser.ParseDocumentation(typeDefinition)
            };

            _repository.AddType(sdType);

            return sdType;
        }

        private SDType CreateSDType(IType type, bool isProjectStranger)
        {
            var nameSpace = _repository.GetNamespaceByIdentifier(type.Namespace);
            var namespaceRef = nameSpace ?? new SDNamespace(type.Namespace) { IsProjectStranger = true };

            var sdType = new SDType(type.GetIdentifier(), type.Name, namespaceRef)
            {
                IsProjectStranger = isProjectStranger,
                Kind = type.Kind.ToString()
            };

            _repository.AddType(sdType);

            return sdType;
        }
    }
}
