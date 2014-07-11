using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpDox.Sdk.Config;
using SharpDox.Model.Documentation;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class NamespaceParser : BaseParser
    {
        private readonly List<string> _descriptionFiles;
        private readonly Dictionary<string, string> _tokens;

        internal NamespaceParser(SDRepository repository, ICoreConfigSection sharpDoxConfig, string inputFile, Dictionary<string, string> tokens) : base(repository, sharpDoxConfig)
        {
            _descriptionFiles = Directory.EnumerateFiles(Path.GetDirectoryName(inputFile), "*.sdnd", SearchOption.AllDirectories).ToList();
            _tokens = tokens;
        }

        internal void ParseProjectNamespaces(CSharpProject project)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int i = 0; i < types.Count; i++)
            {
                HandleOnItemParseStart(types[i].Namespace, i, types.Count);
                if (!_sharpDoxConfig.ExcludedIdentifiers.Contains(types[i].Namespace))
                {
                    _repository.AddNamespace(GetParsedNamespace(types[i]));
                }
            }
        }

        internal SDNamespace GetParsedNamespace(IType type)
        {
            var descriptionFiles = _descriptionFiles.Where(d => Path.GetFileName(d).ToLower().Contains(type.Namespace.ToLower() + ".sdnd"));

            var descriptions = new SDLanguageItemCollection<SDTemplate>();
            foreach (var file in descriptionFiles)
            {
                var splitted = Path.GetFileName(file).ToLower().Replace(type.Namespace.ToLower(), " ").Split('.');
                if (splitted.Length > 0 && splitted[0].Length == 2 && CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.TwoLetterISOLanguageName == splitted[0]))
                {
                    descriptions.Add(splitted[0], new SDTemplate(File.ReadAllText(file), _tokens));
                    ExecuteOnDocLanguageFound(splitted[0].ToLower());
                }
                else if (splitted.Length > 0 && string.IsNullOrEmpty(splitted[0].Trim()))
                {
                    descriptions.Add("default", new SDTemplate(File.ReadAllText(file), _tokens));
                }
            }

            return new SDNamespace(type.Namespace)
            {
                Assemblyname = ((DefaultResolvedTypeDefinition)type).ParentAssembly.FullAssemblyName,
                Description = descriptions
            };
        }
    }
}