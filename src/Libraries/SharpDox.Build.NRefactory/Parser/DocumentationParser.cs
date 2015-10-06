using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Xml;
using SharpDox.Model.Documentation;
using SharpDox.Model.Documentation.Token;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class DocumentationParser
    {
        public SDLanguageItemCollection<SDDocumentation> ParseDocumentation(IEntity entity)
        {
            var docDic = new SDLanguageItemCollection<SDDocumentation>();

            try
            {
                if (entity != null)
                {
                    var xmlDoc = XmlDocumentationElement.Get(entity);

                    if (xmlDoc != null)
                    {
                        foreach (XmlDocumentationElement child in xmlDoc.Children)
                        {
                            if (CultureInfo.GetCultures(CultureTypes.NeutralCultures).Any(c => c.TwoLetterISOLanguageName == child.Name.ToLower()) || child.Name.ToLower() == "default")
                            {
                                // TODO
                                //_sdRepository.AddDocumentationLanguage(child.Name.ToLower());
                                var languageDoc = ParseDocumentation(child.Children, true);
                                docDic.Add(child.Name.ToLower(), languageDoc);
                            }
                        }

                        //Es wurde keine Sprachunterstützung in der Doku genutzt.
                        //Deswegen wird die Doku einfach als "default" geladen.
                        if (docDic.Count == 0)
                        {
                            var defaultDoc = ParseDocumentation(xmlDoc.Children);
                            docDic.Add("default", defaultDoc);
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return docDic;
        }

        private SDDocumentation ParseDocumentation(IEnumerable<XmlDocumentationElement> docElements, bool multilang = false)
        {
            var sdDocumentation = new SDDocumentation();

            foreach (var child in docElements)
            {
                switch (child.Name.ToLower())
                {
                    case "typeparam":
                        var typeparamKey = child.GetAttribute("name") ?? "typeparam";
                        if (!sdDocumentation.TypeParams.ContainsKey(typeparamKey))
                            sdDocumentation.TypeParams.Add(typeparamKey, ParseContentTokens(child, multilang));
                        break;
                    case "param":
                        var paramKey = child.GetAttribute("name") ?? "param";
                        if (!sdDocumentation.Params.ContainsKey(paramKey))
                            sdDocumentation.Params.Add(paramKey, ParseContentTokens(child, multilang));
                        break;
                    case "exception":
                        var exKey = child.GetAttribute("cref") ?? "exception";
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

        private SDTokenList ParseContentTokens(XmlDocumentationElement xmlElement, bool multilang)
        {
            var content = new SDTokenList();

            foreach (XmlDocumentationElement element in xmlElement.Children)
            {
                var text = multilang ? Regex.Replace(element.TextContent, "^[ ]{4}", "", RegexOptions.Multiline) : element.TextContent;

                if (element.IsTextNode)
                {
                    content.Add(new SDToken { Role = SDTokenRole.Text, Text = text });
                }
                else
                {
                    switch (element.Name.ToLower())
                    {
                        case "see":
                            var seeRef = GetSeeRef(element);
                            if (seeRef != null)
                            {
                                content.Add(seeRef);
                            }
                            break;
                        case "typeparamref":
                            content.Add(new SDToken { Role = SDTokenRole.TypeParamRef, Text = element.GetAttribute("name") });
                            break;
                        case "paramref":
                            content.Add(new SDToken { Role = SDTokenRole.ParamRef, Text = element.GetAttribute("name") });
                            break;
                        case "code":
                            content.Add(new SDCodeToken { Text = text, IsInline = false });
                            break;
                        case "c":
                            content.Add(new SDCodeToken { Text = text, IsInline = true });
                            break;
                        case "para":
                            content.Add(new SDToken { Text = text, Role = SDTokenRole.Paragraph });
                            break;
                    }
                }
            }
            return content;
        }

        private void AddResultsSection(Dictionary<string, SDTokenList> results, XmlDocumentationElement xmlElement, bool multilang)
        {
            if (!string.IsNullOrEmpty(xmlElement.GetAttribute("httpCode")))
            {
                results.Add(xmlElement.GetAttribute("httpCode"), ParseContentTokens(xmlElement, multilang));
            }
            else if(!results.ContainsKey("default"))
            {
                results.Add("default", ParseContentTokens(xmlElement, multilang));
            }
        }

        private SDToken GetSeeRef(XmlDocumentationElement xmlElement)
        {
            try
            {
                var sdToken = new SDSeeToken();

                if (xmlElement.ReferencedEntity != null)
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
                }

                return sdToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
