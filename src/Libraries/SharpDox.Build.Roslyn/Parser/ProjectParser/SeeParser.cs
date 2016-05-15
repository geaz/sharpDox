using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SharpDox.Model.Documentation.Token;
using SharpDox.Model.Repository;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class SeeParser
    {
        private readonly SDRepository _sdRepository;
        private readonly List<SDToken> _seeTokens;

        internal SeeParser(SDRepository sdRepository, List<SDToken> seeTokens)
        {
            _sdRepository = sdRepository;
            _seeTokens = seeTokens;
        }

        internal void ResolveAllSeeTokens()
        {
            foreach (var token in _seeTokens)
            {
                var seeToken = ((SDSeeToken) token);
                var cref = XElement.Parse(seeToken.AttributeValue).Attributes().FirstOrDefault(a => a.Name == "cref");
                if (!string.IsNullOrEmpty(cref?.Value))
                {
                    var cleanedRef = cref.Value.Substring(2);
                    var splitted = cleanedRef.Split('.');
                    switch (cref.Value[0])
                    {
                        case 'T':
                            seeToken.Name = splitted.Last();
                            seeToken.Namespace = string.Join(".", splitted.Take(splitted.Length - 1));
                            seeToken.Text = splitted.Last();
                            seeToken.Identifier = $"{seeToken.Namespace}.{seeToken.Name}";

                            if (cleanedRef.Contains('`'))
                            {
                                var splittedName = seeToken.Name.Split('`');
                                var type = _sdRepository.GetAllTypes().SingleOrDefault(
                                    t => t.Namespace.Fullname == seeToken.Namespace &&
                                    t.Name == splittedName[0] && t.TypeArguments.Count == int.Parse(splittedName[1]));

                                seeToken.Identifier = type?.Identifier;
                                seeToken.Name = type != null ? type.Name : $"Missing: {seeToken.AttributeValue}";
                                seeToken.Text = seeToken.Name;
                            }
                            break;
                        default:
                            //TODO See parsing, if type is generic
                            seeToken.Name = splitted.Last();
                            seeToken.Namespace = string.Join(".", splitted.Take(splitted.Length - 2));
                            seeToken.DeclaringType = splitted[splitted.Length - 2];
                            seeToken.Text = splitted.Last();
                            seeToken.Identifier = $"{seeToken.Namespace}.{seeToken.DeclaringType}.{seeToken.Name}";

                            if (seeToken.Name.Contains('`'))
                            {
                                var splittedName = seeToken.Name.Replace("(", string.Empty).Replace(")", string.Empty).Split(new [] {"`"}, StringSplitOptions.RemoveEmptyEntries);
                                var sdMethod = _sdRepository.GetAllMethods().SingleOrDefault(
                                    m => m.Namespace == seeToken.Namespace &&
                                    m.Name == splittedName[0] && m.TypeParameters.Count == int.Parse(splittedName[1]));

                                seeToken.Identifier = sdMethod?.Identifier;
                                seeToken.Name = sdMethod != null ? sdMethod.Name : $"Missing: {seeToken.AttributeValue}";
                                seeToken.Text = seeToken.Name;
                            }
                            break;
                    }
                }
            }
        }
    }
}
