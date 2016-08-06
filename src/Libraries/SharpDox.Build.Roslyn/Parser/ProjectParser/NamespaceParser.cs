using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class NamespaceParser : BaseParser
    {
        private readonly TypeParser _typeParser;

        private readonly List<string> _descriptionFiles;
        
        internal NamespaceParser(ParserOptions parserOptions) : base(parserOptions)
        {
            _typeParser = new TypeParser(parserOptions);
            _descriptionFiles = Directory.EnumerateFiles(Path.GetDirectoryName(parserOptions.SharpDoxConfig.InputFile), "*.sdnd", SearchOption.AllDirectories).ToList();
        }

        internal void ParseProjectNamespacesRecursively(INamespaceSymbol namespaceSymbol)
        {
            HandleOnItemParseStart(namespaceSymbol.Name);
            if (!ParserOptions.SharpDoxConfig.ExcludedIdentifiers.Contains(namespaceSymbol.GetIdentifier()) || ParserOptions.IgnoreExcludes)
            {
                var sdNamespace = GetParsedNamespace(namespaceSymbol);
                ParserOptions.SDRepository.AddNamespace(sdNamespace);
                _typeParser.ParseProjectTypes(namespaceSymbol.GetTypeMembers().ToList());
            }

            foreach (var childNamespaceSymbol in namespaceSymbol.GetNamespaceMembers())
            {
                ParseProjectNamespacesRecursively(childNamespaceSymbol);
            }
        }

        private SDNamespace GetParsedNamespace(INamespaceSymbol namespaceSymbol)
        {
            var descriptionFiles = _descriptionFiles.Where(d => Path.GetFileName(d).ToLower().Contains(namespaceSymbol.ToDisplayString().ToLower() + ".sdnd"));

            var descriptions = new SDLanguageItemCollection<SDTemplate>();
            foreach (var file in descriptionFiles)
            {
                if (!string.IsNullOrEmpty(namespaceSymbol.ToDisplayString().Trim()))
                {
                    var splitted = Path.GetFileName(file).ToLower().Replace(namespaceSymbol.ToDisplayString().ToLower(), " ").Split('.');
                    if (splitted.Length > 0 && splitted[0].Length == 2 && CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.TwoLetterISOLanguageName == splitted[0]))
                    {
                        descriptions.Add(splitted[0], new SDTemplate(File.ReadAllText(file), ParserOptions.Tokens));
                        ExecuteOnDocLanguageFound(splitted[0].ToLower());
                    }
                    else if (splitted.Length > 0 && string.IsNullOrEmpty(splitted[0].Trim()))
                    {
                        descriptions.Add("default", new SDTemplate(File.ReadAllText(file), ParserOptions.Tokens));
                    }
                }
            }

            return new SDNamespace(namespaceSymbol.GetIdentifier())
            {
                Assemblyname = namespaceSymbol.ContainingAssembly.Name,
                Descriptions = descriptions
            };
        }
    }
}