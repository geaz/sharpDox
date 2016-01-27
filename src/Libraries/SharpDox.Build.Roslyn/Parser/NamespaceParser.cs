using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Documentation;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class NamespaceParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        private readonly List<string> _descriptionFiles;
        private readonly Dictionary<string, string> _tokens;
        
        internal NamespaceParser(ParserOptions parserOptions, string solutionFile, Dictionary<string, string> tokens) : base(parserOptions)
        {
            _typeParser = new TypeParser(parserOptions);
            _descriptionFiles = Directory.EnumerateFiles(Path.GetDirectoryName(solutionFile), "*.sdnd", SearchOption.AllDirectories).ToList();
            _tokens = tokens;
        }

        internal void ParseProjectNamespacesRecursively(INamespaceSymbol namespaceSymbol)
        {
            HandleOnItemParseStart(namespaceSymbol.Name);
            if (!ParserOptions.SharpDoxConfig.ExcludedIdentifiers.Contains(namespaceSymbol.ToDisplayString(SDoxDisplayFormat.IdentifierFormat)))
            {
                ParserOptions.SDRepository.AddNamespace(GetParsedNamespace(namespaceSymbol));
                _typeParser.ParseProjectTypes(namespaceSymbol.GetTypeMembers().ToList());
            }

            foreach(var childNamespaceSymbol in namespaceSymbol.GetNamespaceMembers())
            {
                ParseProjectNamespacesRecursively(childNamespaceSymbol);
            }
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