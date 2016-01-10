using SharpDox.Model.Documentation;
using SharpDox.Model.Documentation.Token;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SharpDox.Build.Roslyn.Parser
{
    internal class DocumentationParser
    {
        public SDLanguageItemCollection<SDDocumentation> ParseDocumentation(string documentationXml)
        {
            var docDic = new SDLanguageItemCollection<SDDocumentation>();
            if (!string.IsNullOrEmpty(documentationXml))
            {
                var xml = XDocument.Parse(documentationXml);
                foreach (var child in xml.Descendants())
                {
                    if (CultureInfo.GetCultures(CultureTypes.NeutralCultures).Any(c => c.TwoLetterISOLanguageName == child.Name.LocalName.ToLower()) || child.Name.LocalName.ToLower() == "default")
                    {
                        // TODO
                        //_sdRepository.AddDocumentationLanguage(child.Name.ToLower());
                        var languageDoc = ParseDocumentation(child.Descendants(), true);
                        docDic.Add(child.Name.LocalName.ToLower(), languageDoc);
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
                        sdDocumentation.SeeAlsos.Add(GetSeeRef(child));
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
                if(textElement != null)
                {
                    content.Add(new SDToken
                    {
                        Role = SDTokenRole.Text,
                        Text = multilang ? Regex.Replace(element.ToString(), "^[ ]{4}", "", RegexOptions.Multiline) : element.ToString()
                    });
                }

                var nodeElement = element as XElement;
                if(nodeElement != null)
                {
                    switch (nodeElement.Name.LocalName.ToLower())
                    {
                        case "see":
                            var seeRef = GetSeeRef(nodeElement);
                            if (seeRef != null)
                            {
                                content.Add(seeRef);
                            }
                            break;
                        case "typeparamref":
                            content.Add(new SDToken { Role = SDTokenRole.TypeParamRef, Text = nodeElement.Attribute("name")?.Value });
                            break;
                        case "paramref":
                            content.Add(new SDToken { Role = SDTokenRole.ParamRef, Text = nodeElement.Attribute("name")?.Value });
                            break;
                        case "code":
                            content.Add(new SDCodeToken { Text = nodeElement.Value, IsInline = false });
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

        private SDToken GetSeeRef(XElement xmlElement)
        {
            try
            {
                var sdToken = new SDSeeToken();

                /*if (xmlElement.ReferencedEntity != null)
                {
                    var identifier = xmlElement.ReferencedEntity.DeclaringType != null
                                            ? xmlElement.ReferencedEntity.DeclaringType.GetIdentifier()
                                            : string.Empty;

                    sdToken.Name = xmlElement.ReferencedEntity.Name;
                    sdToken.Namespace = xmlElement.ReferencedEntity.Namespace;
                    sdToken.DeclaringType = identifier;
                    sdToken.Text = xmlElement.ReferencedEntity.Name;
                }
                else
                {
                    sdToken.Name = xmlElement.GetAttribute("cref");
                }*/

                return sdToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
