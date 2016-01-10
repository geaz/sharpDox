using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Sdk.Config;
using SharpDox.Model.Documentation;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class NamespaceParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        private readonly List<string> _descriptionFiles;
        private readonly Dictionary<string, string> _tokens;
        
        internal NamespaceParser(ICoreConfigSection sharpDoxConfig, string inputFile, Dictionary<string, string> tokens) : base(sharpDoxConfig)
        {
            _typeParser = new TypeParser(sharpDoxConfig);
            _descriptionFiles = Directory.EnumerateFiles(Path.GetDirectoryName(inputFile), "*.sdnd", SearchOption.AllDirectories).ToList();
            _tokens = tokens;
        }

        internal void ParseProjectNamespaces(Compilation projectCompilation, SDRepository repository)
        {
            var allNamespaceSymbols = GetAllNamespaces(projectCompilation.Assembly.GlobalNamespace.GetNamespaceMembers().ToList());
            for (int i = 0; i < allNamespaceSymbols.Count; i++)
            {
                HandleOnItemParseStart(allNamespaceSymbols[i].Name);
                if (!SharpDoxConfig.ExcludedIdentifiers.Contains(allNamespaceSymbols[i].ToDisplayString()))
                {
                    repository.AddNamespace(GetParsedNamespace(allNamespaceSymbols[i]));
                    _typeParser.ParseProjectTypes(allNamespaceSymbols[i].GetTypeMembers().ToList(), repository);
                }
            }
        }

        private List<INamespaceSymbol> GetAllNamespaces(List<INamespaceSymbol> namespaceSymbols)
        {
            var allNamespaceSymbols = new List<INamespaceSymbol>();

            foreach (var namespaceSymbol in namespaceSymbols)
            {
                allNamespaceSymbols.Add(namespaceSymbol);
                allNamespaceSymbols.AddRange(GetAllNamespaces(namespaceSymbol.GetNamespaceMembers().ToList()));
            }

            return allNamespaceSymbols;
        } 

        private SDNamespace GetParsedNamespace(INamespaceSymbol namespaceSymbol)
        {
            var descriptionFiles = _descriptionFiles.Where(d => Path.GetFileName(d).ToLower().Contains(namespaceSymbol.Name.ToLower() + ".sdnd"));

            var descriptions = new SDLanguageItemCollection<SDTemplate>();
            foreach (var file in descriptionFiles)
            {
                if (!string.IsNullOrEmpty(namespaceSymbol.Name.Trim()))
                {
                    var splitted = Path.GetFileName(file).ToLower().Replace(namespaceSymbol.Name.ToLower(), " ").Split('.');
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
            }

            return new SDNamespace(namespaceSymbol.ToDisplayString())
            {
                Assemblyname = namespaceSymbol.ContainingAssembly.Name,
                Descriptions = descriptions
            };
        }
    }
}