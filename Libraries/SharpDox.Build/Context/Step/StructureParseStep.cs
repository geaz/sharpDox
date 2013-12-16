using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using SharpDox.Build.Loader;
using SharpDox.Build.Parser;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Context.Step
{
    internal class StructureParseStep
    {
        private CSharpSolution _solution;
        private SDRepository _repository;

        private readonly SDBuildStrings _sdBuildStrings;
        private readonly BuildMessenger _buildMessenger;

        public StructureParseStep(SDBuildStrings sdBuildStrings, BuildMessenger buildMessenger)
        {
            _sdBuildStrings = sdBuildStrings;
            _buildMessenger = buildMessenger;
        }

        public SDRepository ParseStructure(CSharpSolution solution)
        {
            _solution = solution;
            _repository = new SDRepository();

            _buildMessenger.ExecuteOnStepProgress(0);
            _buildMessenger.ExecuteOnBuildMessage(_sdBuildStrings.ParsingSolution);

            ParseNamespaces();
            ParseTypes();

            _buildMessenger.ExecuteOnStepProgress(100);

            return _repository;
        }

        private void ParseNamespaces()
        {
            var pi = 0;
            for (int i = 0; i < _solution.Projects.Count; i++)
            {
                pi = i;
                var types = _solution.Projects[i].Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
                for (int j = 0; j < types.Count; j++)
                {
                    PostProgress(_sdBuildStrings.ParsingNamespace + ": " + types[j].Namespace, j, types.Count, pi, _solution.Projects.Count);

                    var sdNamespace = new SDNamespace(types[j].Namespace);
                    _repository.AddNamespace(sdNamespace);
                }
            }
        }

        private void ParseTypes()
        {
            var pi = 0;
            for (int i = 0; i < _solution.Projects.Count; i++)
            {
                pi = i;
                var types = _solution.Projects[i].Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
                for (int j = 0; j < types.Count; j++)
                {
                    if (types[j].Kind != TypeKind.Delegate)
                    {
                        PostProgress(_sdBuildStrings.ParsingClass + ": " + string.Format("{0}.{1}", types[j].Namespace, types[j].Name), j, types.Count, pi, _solution.Projects.Count);

                        var sdType = CreateSDType(types[j]);

                        EventParser.ParseMinimalFields(sdType, types[j]);
                        PropertyParser.ParseMinimalProperties(sdType, types[j]);
                        FieldParser.ParseMinimalFields(sdType, types[j]);
                        MethodParser.ParseMinimalConstructors(sdType, types[j]);
                        MethodParser.ParseMinimalMethods(sdType, types[j]);

                        _repository.AddNamespaceTypeRelation(types[j].Namespace, sdType.Identifier);
                    }
                }
            }
        }

        private void PostProgress(string message, double itemIndex, double itemTotal, double parentIndex, double parentTotal)
        {
            var percentage = ((itemIndex / itemTotal) * (100d / parentTotal)) + (parentIndex * (100d / parentTotal));

            _buildMessenger.ExecuteOnStepMessage(message);
            _buildMessenger.ExecuteOnStepProgress((int)percentage);
        }

        private SDType CreateSDType(IType type)
        {
            var nameSpace = _repository.GetNamespaceByIdentifier(type.Namespace);
            var namespaceRef = nameSpace ?? new SDNamespace(type.Namespace) { IsProjectStranger = true };

            var sdType = new SDType(type.GetIdentifier(), type.Name, namespaceRef)
            {
                Accessibility = type.GetDefinition().Accessibility.ToString().ToLower()
            };

            _repository.AddType(sdType);

            return sdType;
        }
    }
}
