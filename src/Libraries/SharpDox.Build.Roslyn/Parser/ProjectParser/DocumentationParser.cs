using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Options;
using SharpDox.Model.Documentation;
using SharpDox.Model.Documentation.Token;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class DocumentationParser
    {
        private readonly List<SDToken> _seeTokens;

        public DocumentationParser(List<SDToken> seeTokens)
        {
            _seeTokens = seeTokens;
        }

        public SDLanguageItemCollection<SDDocumentation> ParseDocumentation(ISymbol symbol)
        {
            var documentationXml = symbol.GetDocumentationCommentXml();
            var docDic = new SDLanguageItemCollection<SDDocumentation>();
            if (!string.IsNullOrEmpty(documentationXml))
            {
                var xml = XDocument.Parse($"<doc>{documentationXml}</doc>");
                foreach (var child in xml.Descendants())
                {
                    var isoCode = child.Name.LocalName.ToLower();
                    if (CultureInfo.GetCultures(CultureTypes.NeutralCultures).Any(c => c.TwoLetterISOLanguageName == isoCode) || isoCode == "default")
                    {
                        // TODO
                        //_sdRepository.AddDocumentationLanguage(child.Name.ToLower());
                        var languageDoc = ParseDocumentation(child.Descendants(), true);
                        if(!docDic.ContainsKey(isoCode)) docDic.Add(isoCode, languageDoc);
                    }
                }

                //Es wurde keine Sprachunterstützung in der Doku genutzt.
                //Deswegen wird die Doku einfach als "default" geladen.
                if (docDic.Count == 0)
                {
                    var defaultDoc = ParseDocumentation(xml.Descendants());
                    docDic.Add("default", defaultDoc);
                }
            }
            return docDic;
        }

        private SDDocumentation ParseDocumentation(IEnumerable<XElement> docElements, bool multilang = false)
        {
            var sdDocumentation = new SDDocumentation();

            foreach (var child in docElements)
            {
                switch (child.Name.LocalName.ToLower())
                {
                    case "typeparam":
                        var typeparamKey = child.Attribute("name")?.Value ?? "typeparam";
                        if (!sdDocumentation.TypeParams.ContainsKey(typeparamKey))
                            sdDocumentation.TypeParams.Add(typeparamKey, ParseContentTokens(child, multilang));
                        break;
                    case "param":
                        var paramKey = child.Attribute("name")?.Value ?? "param";
                        if (!sdDocumentation.Params.ContainsKey(paramKey))
                            sdDocumentation.Params.Add(paramKey, ParseContentTokens(child, multilang));
                        break;
                    case "exception":
                        var exKey = child.Attribute("cref")?.Value ?? "exception";
                        if (!sdDocumentation.Exceptions.ContainsKey(exKey))
                            sdDocumentation.Exceptions.Add(exKey, ParseContentTokens(child, multilang));
                        break;
                    case "summary":
                        sdDocumentation.Summary = ParseContentTokens(child, multilang);
                        break;
                    case "remarks":
                        sdDocumentation.Remarks = ParseContentTokens(child, multilang);
                        break;
                    case "example":
                        sdDocumentation.Example = ParseContentTokens(child, multilang);
                        break;
                    case "returns":
                        AddResultsSection(sdDocumentation.Returns, child, multilang);
                        break;
                    case "seealso":
                        var seeToken = new SDSeeToken(child.ToString());
                        _seeTokens.Add(seeToken);
                        sdDocumentation.SeeAlsos.Add(seeToken);
                        break;
                }
            }

            return sdDocumentation;
        }

        private SDTokenList ParseContentTokens(XElement xmlElement, bool multilang)
        {
            var content = new SDTokenList();

            foreach (var element in xmlElement.Nodes())
            {
                var textElement = element as XText;
                if (textElement != null)
                {
                    var newLines = new[] {"\r\n", "\n"};
                    var text = element.ToString();

                    while (true)
                    {
                        foreach (var newLine in newLines)
                        {
                            if (text.StartsWith(newLine))
                            {
                                text = text.Substring(newLine.Length);
                            }

                            if (text.EndsWith(newLine))
                            {
                                text = text.Substring(0, text.Length - newLine.Length);
                            }
                        }

                        text = text.Trim();

                        var shouldBreak = true;

                        foreach (var newLine in newLines)
                        {
                            if (text.StartsWith(newLine) || text.EndsWith(newLine))
                            {
                                shouldBreak = false;
                                break;
                            }
                        }

                        if (shouldBreak)
                        {
                            // Replace single newline occurrences by a space (this is just a newline in the docs, not a new paragraph)
                            foreach (var newLine in newLines)
                            {
                                text = text.Replace(newLine, " ");
                            }

                            // Remove duplicate spaces that were used for indentation
                            while (text.Contains("  "))
                            {
                                text = text.Replace("  ", " ");
                            }

                            break;
                        }
                    }

                    content.Add(new SDToken
                    {
                        Role = SDTokenRole.Text,
                        Text = text
                    });
                }

                var nodeElement = element as XElement;
                if(nodeElement != null)
                {
                    switch (nodeElement.Name.LocalName.ToLower())
                    {
                        case "see":
                            var seeToken = new SDSeeToken(nodeElement.ToString());
                            _seeTokens.Add(seeToken);
                            content.Add(seeToken);
                            break;
                        case "typeparamref":
                            content.Add(new SDToken { Role = SDTokenRole.TypeParamRef, Text = nodeElement.Attribute("name")?.Value });
                            break;
                        case "paramref":
                            content.Add(new SDToken { Role = SDTokenRole.ParamRef, Text = nodeElement.Attribute("name")?.Value });
                            break;
                        case "code":
                            var workspace = MSBuildWorkspace.Create();
                            var formattedResult = Formatter.Format(CSharpSyntaxTree.ParseText(nodeElement.Value, CSharpParseOptions.Default).GetRoot(), workspace);
                            content.Add(new SDCodeToken { Text = formattedResult.ToString(), IsInline = false });
                            break;
                        case "c":
                            content.Add(new SDCodeToken { Text = nodeElement.Value, IsInline = true });
                            break;
                        case "para":
                            content.Add(new SDToken { Text = nodeElement.Value, Role = SDTokenRole.Paragraph });
                            break;
                    }
                }                
            }
            return content;
        }

        private void AddResultsSection(Dictionary<string, SDTokenList> results, XElement xmlElement, bool multilang)
        {
            if (!string.IsNullOrEmpty(xmlElement.Attribute("httpCode")?.Value))
            {
                results.Add(xmlElement.Attribute("httpCode")?.Value, ParseContentTokens(xmlElement, multilang));
            }
            else if(!results.ContainsKey("default"))
            {
                results.Add("default", ParseContentTokens(xmlElement, multilang));
            }
        }
    }
}
