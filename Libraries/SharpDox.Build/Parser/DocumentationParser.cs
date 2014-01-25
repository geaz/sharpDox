using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Xml;
using SharpDox.Model.Documentation;
using SharpDox.Model.Documentation.Token;
using SharpDox.Model.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpDox.Build.Parser
{
	internal class DocumentationParser
    {
	    private readonly SDRepository _sdRepository;

	    public DocumentationParser(SDRepository sdRepository)
	    {
	        _sdRepository = sdRepository;
	    }

	    public Dictionary<string, SDDocumentation> ParseDocumentation(IEntity entity)
        {
            var docDic = new Dictionary<string, SDDocumentation>();

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
                                _sdRepository.AddDocumentationLanguage(child.Name.ToLower());
                                var languageDoc = ParseDocumentation(child.Children);
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

        private SDDocumentation ParseDocumentation(IEnumerable<XmlDocumentationElement> docElements)
        {
            var sdDocumentation = new SDDocumentation();

            foreach (var child in docElements)
            {
                switch (child.Name.ToLower())
                {
                    case "typeparam":
                        var typeparamKey = child.GetAttribute("name") ?? "typeparam";
                        if (!sdDocumentation.TypeParams.ContainsKey(typeparamKey))
                            sdDocumentation.TypeParams.Add(typeparamKey, ParseContentTokens(child));
                        break;
                    case "param":
                        var paramKey = child.GetAttribute("name") ?? "param";
                        if (!sdDocumentation.Params.ContainsKey(paramKey))
                            sdDocumentation.Params.Add(paramKey, ParseContentTokens(child));
                        break;
                    case "exceptions":
                        var exKey = child.GetAttribute("name") ?? "exceptions";
                        if (!sdDocumentation.Exceptions.ContainsKey(exKey))
                            sdDocumentation.Exceptions.Add(exKey, ParseContentTokens(child));
                        break;
                    case "summary":
                        sdDocumentation.Summary = ParseContentTokens(child);
                        break;
                    case "remarks":
                        sdDocumentation.Remarks = ParseContentTokens(child);
                        break;
                    case "example":
                        sdDocumentation.Example = ParseContentTokens(child);
                        break;
                    case "returns":
                        sdDocumentation.Returns = ParseContentTokens(child);
                        break;
                }
            }

            return sdDocumentation;
        }

        private SDTokenList ParseContentTokens(XmlDocumentationElement xmlElement)
		{
            var content = new SDTokenList();

			foreach (XmlDocumentationElement element in xmlElement.Children)
			{
                var text = Regex.Replace(element.TextContent, "^[ ]{4}", "", RegexOptions.Multiline);

			    if (element.IsTextNode)
			    {
                    content.Add(new SDToken { Role = SDTokenRole.Text, Text = text });
			    }
			    else
			    {
			        switch (element.Name.ToLower())
			        {
			            case "see":
			                if (element.ReferencedEntity != null)
			                {
			                    var identifier = element.ReferencedEntity.DeclaringType != null
                                                        ? element.ReferencedEntity.DeclaringType.GetIdentifier()
			                                            : string.Empty;
			                    content.Add(new SDSeeToken
			                        {
			                            Name = element.ReferencedEntity.Name,
			                            Namespace = element.ReferencedEntity.Namespace,
                                        DeclaringType = identifier != string.Empty ? _sdRepository.GetTypeByIdentifier(identifier) : null,
                                        Text = element.ReferencedEntity.Name
			                        });
			                }
			                else
			                {
			                    content.Add(new SDSeeToken {Name = element.GetAttribute("cref")});
			                }
			                break;
                        case "typeparamref":
                            content.Add(new SDToken {Role = SDTokenRole.TypeParamRef, Text = element.GetAttribute("name")});
			                break;
			            case "paramref":
			                content.Add(new SDToken {Role = SDTokenRole.ParamRef, Text = element.GetAttribute("name")});
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
	}
}
