using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharpDox.Model.Documentation
{
    public class SDTemplate
    {
        private string _transformedTemplate;

        private readonly Dictionary<string, string> _tokens;

        public SDTemplate(string template, Dictionary<string, string> tokens = null)
        {
            Template = template;
            _tokens = tokens;
            _transformedTemplate = Template;
        }

        /// <default>
        ///     <summary>
        ///     This function takes a delegate which 
        ///     gets called for each link placeholder.
        ///     
        ///     The delegate gets three parameters:
        ///     
        ///     - The type of the link placeholder.
        ///       Possible values are: 'article', 'image', 'namespace', 'type',
        ///       'method', 'field', 'property' and 'event'.
        ///     - The name/identifier of the linked element.
        ///       For articles it is the title, images the filename, namespaces the fullname and
        ///       the identifier for the remaining elements.
        ///       
        ///     The delegate has to return a string which replaces the old link placeholder.
        ///     </summary>
        ///     <param name="transform">A delegate with two string and a string return value.</param>
        ///     <returns>The transformed text.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Dieser Funktion muss ein Delegat übergeben werden,
        ///     welches von sharpDox für jeden Link-Platzhalter aufgerufen wird.
        ///     
        ///     Dabei übergibt sharpDox dem Delegat drei Parameter:
        ///     
        ///     - Die Art des übergebene Link-Platzhalters. 
        ///       Mögliche Werte sind: 'article', 'image', 'namespace', 'type',
        ///       'method', 'field', 'property' und 'event'.
        ///     - Der Name/Identifikator des verlinkten Elements.
        ///       Für Artikel der Titel, Bilder der Dateiname, Namensräume der Vollname und
        ///       für den Rest der Identifikator des Elements.
        ///       
        ///     Der Delegat muss einen String zurückliefern, welcher genutzt wird um den alten Platzhalter zu ersetzen.
        ///     </summary>
        ///     <param name="transform">Ein Delegat mit zwei String- und einem String-Rückgabewert.</param>
        ///     <returns>Der transformierte Text.</returns>
        /// </de>
        public string Transform(Func<string, string, string> transform)
        {
            ReplaceTokens();
            ReplaceLink("image", @"{{image-link:[^}}]*}}", transform);
            ReplaceLink("method", @"{{method-link:[^}}]*}}", transform);
            ReplaceLink("constructor", @"{{constructor-link:[^}}]*}}", transform);
            ReplaceLink("event", @"{{event-link:[^}}]*}}", transform);
            ReplaceLink("property", @"{{property-link:[^}}]*}}", transform);
            ReplaceLink("field", @"{{field-link:[^}}]*}}", transform);
            ReplaceLink("type", @"{{type-link:[^}}]*}}", transform);
            ReplaceLink("namespace", @"{{namespace-link:[^}}]*}}", transform);
            ReplaceLink("article", @"{{article-link:[^}}]*}}", transform);

            return _transformedTemplate;
        }

        private void ReplaceTokens()
        {
            if (_tokens != null)
            {
                var regEx = new Regex(@"{{token:[^}}]*}}");
                var templateTokens = regEx.Matches(_transformedTemplate);
                foreach (Match match in templateTokens)
                {
                    var token = match.Value.Remove(match.Value.Length - 2).Remove(0, 2);
                    var key = token.Split(':')[1].Replace("&lt;", "<").Replace("&gt;", ">");

                    var tokenValue = "TOKEN_NOT_DEFINED";
                    if (_tokens.ContainsKey(key))
                    {
                        tokenValue = _tokens[key];
                    }
                    _transformedTemplate = _transformedTemplate.Replace(match.Value, tokenValue);
                }
            }
        }

        private void ReplaceLink(string linkType, string regex, Func<string, string, string> transform)
        {
            var regEx = new Regex(regex);
            var links = regEx.Matches(_transformedTemplate);
            foreach (Match link in links)
            {
                var splitted = link.Value.Split(new string[] { "}}" }, StringSplitOptions.None);
                var value = splitted[0].Split(':')[1].Replace("&lt;", "<").Replace("&gt;", ">");

                _transformedTemplate = _transformedTemplate.Replace(splitted[0] + "}}", transform(linkType, value));
            }
        }

        public string Template { get; private set; }
    }
}
