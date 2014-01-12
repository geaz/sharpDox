using System;
using System.Text.RegularExpressions;
using SharpDox.Model.Repository;

namespace SharpDox.Sdk
{
    /// <default>
    ///     <summary>
    ///     The <c>Templater</c> can be used to replace all link placeholders
    ///     in articles, descriptions and diagrams.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Der <c>Templater</c> kann genutzt werden um alle
    ///     Link-Platzhalter in Artikeln, Beschreibungen und Diagrammen
    ///     zu ersetzen.
    ///     </summary>
    /// </de>
    public class Templater
    {
        private string _template;
        private readonly SDRepository _repository;

        /// <default>
        ///     <summary>
        ///     The constructor takes two parameters.
        ///     You have to create a new instance 
        ///     for each text you want to transform.
        ///     </summary>
        ///     <param name="template">The text you want to transform.</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     The Konstruktor akzeptiert zwei Parameter.
        ///     Für jeden Text der transformiert werden soll
        ///     muss eine neue Instanz erstellt werden.
        ///     </summary>
        ///     <param name="template">Der Text der transformiert werden soll.</param>
        /// </de>
        public Templater(SDRepository repository, string template)
        {
            _repository = repository;
            _template = template;
        }

        /// <default>
        ///     <summary>
        ///     This function takes a delegate which 
        ///     gets called for each link placeholder.
        ///     
        ///     The delegate gets two parameters:
        ///     
        ///     - The type of the link placeholder.
        ///       Possible values are: 'article', 'image', 'namespace', 'type',
        ///       'method', 'field', 'property' and 'event'.
        ///     - The name/identifier of the link
        ///       For articles it is the title, images the filename, namespaces the fullname and
        ///       the identifier for the remaining elements.
        ///       
        ///     The delegate has to return a string which replaces the old link placeholder.
        ///     </summary>
        ///     <param name="transform">A delegate with two string parameters and a string returnvalue.</param>
        ///     <returns>The transformed text.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Dieser Funktion muss ein Delegat übergeben werden,
        ///     welches von sharpDox für jeden Link-Platzhalter aufgerufen wird.
        ///     
        ///     Dabei übergibt sharpDox dem Delegat zwei Parameter:
        ///     
        ///     - Die Art des übergebene Link-Platzhalters. 
        ///       Mögliche Werte sind: 'article', 'image', 'namespace', 'type',
        ///       'method', 'field', 'property' und 'event'.
        ///     - Der Name/Identifikator des Links
        ///       Für Artikel der Titel, Bilder der Dateiname, Namensräume der Vollname und
        ///       für den Rest der Identifikator des Elements.
        ///       
        ///     Der Delegat muss einen String zurückliefern, welcher genutzt wird um den alten Platzhalter zu ersetzen.
        ///     </summary>
        ///     <param name="transform">Ein Delegat mit zwei String-Parametern und einem String-Rückgabewert.</param>
        ///     <returns>Der transformierte Text.</returns>
        /// </de>
        public string TransformText(Func<string, Guid, string, string> transform)
        {
            ReplaceLink("image", @"{{image-link:[^}}]*}}", transform);
            ReplaceLink("method", @"{{method-link:[^}}]*}}", transform);
            ReplaceLink("constructor", @"{{constructor-link:[^}}]*}}", transform);
            ReplaceLink("event", @"{{event-link:[^}}]*}}", transform);
            ReplaceLink("property", @"{{property-link:[^}}]*}}", transform);
            ReplaceLink("field", @"{{field-link:[^}}]*}}", transform);
            ReplaceLink("type", @"{{type-link:[^}}]*}}", transform);
            ReplaceLink("namespace", @"{{namespace-link:[^}}]*}}", transform);
            ReplaceLink("article", @"{{article-link:[^}}]*}}", transform);

            return _template;
        }

        private void ReplaceLink(string linkType, string regex, Func<string, Guid, string, string> transform)
        {
            var regEx = new Regex(regex);
            var links = regEx.Matches(_template);
            foreach (Match link in links)
            {
                var splitted = link.Value.Split(new string[] {"}}"}, StringSplitOptions.None);
                var value = splitted[0].Split(':')[1].Replace("&lt;", "<").Replace("&gt;", ">");
                var guid = _repository.GetGuidByIdentifier(value);

                _template = _template.Replace(splitted[0] + "}}", transform(linkType, guid, value));
            }
        }
    }
}
