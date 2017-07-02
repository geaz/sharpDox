using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDox.Model.Documentation.Token
{
    public class SDTokenList : List<SDToken>
    {
        private static readonly List<char> SpecialTokensThatDontRequirePreSpace = new List<char>(new[] { ' ', ';', ',', '.', '\n' });
        private static readonly List<char> SpecialTokensThatDontRequirePostSpace = new List<char>(new[] { ' ', '\n' });

        public new string ToString()
        {
            var text = string.Empty;

            foreach (var token in this)
            {
                switch (token.Role)
                {
                    case SDTokenRole.Paragraph:
                        text += string.Format("{0}{1}{0}", System.Environment.NewLine, token.Text);
                        break;
                    case SDTokenRole.Code:
                        text += ((SDCodeToken)token).IsInline ? token.Text : string.Format("{0}{1}{0}", System.Environment.NewLine, token.Text);
                        break;
                    default:
                        text += token.Text;
                        break;
                }
            }
            return text.Trim();
        }

        public SDTemplate ToMarkdown(Dictionary<string, string> tokens)
        {
            var stringBuilder = new StringBuilder();

            foreach (var token in this)
            {
                var textToAppend = string.Empty;
                var addSpace = true;

                switch (token.Role)
                {
                    case SDTokenRole.Paragraph:
                        addSpace = false;
                        textToAppend = string.Format("{0}{1}{1}", token.Text, Environment.NewLine);
                        break;

                    case SDTokenRole.Code:
                        addSpace = false;
                        var splittedText = token.Text.Split(new [] { "\r\n", "\n" }, StringSplitOptions.None);
                        textToAppend = ((SDCodeToken)token).IsInline ?
                                    string.Format("{0}{1}{0}", "`", token.Text) : 
                                    string.Format("{0}{1}{2}{1}{0}", "```", Environment.NewLine, string.Join(Environment.NewLine, splittedText));
                        break;

                    case SDTokenRole.See:
                        var seeToken = (SDSeeToken)token;
                        if(!string.IsNullOrEmpty(seeToken.Namespace) && string.IsNullOrEmpty(seeToken.DeclaringType))
                        {
                            textToAppend = string.Format("[{0}]({{{{type-link:{1}.{0}}}}})", seeToken.Name, seeToken.Namespace);
                        }
                        else
                        {
                            textToAppend = seeToken.Name;
                        }
                        break;

                    default:
                        textToAppend = token.Text;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(textToAppend))
                {
                    var stringBuilderLength = stringBuilder.Length;

                    if (addSpace && stringBuilderLength > 0 && 
                        !SpecialTokensThatDontRequirePostSpace.Contains(stringBuilder[stringBuilderLength - 1]) &&
                        !SpecialTokensThatDontRequirePreSpace.Contains(textToAppend[0]))
                    {
                        stringBuilder.Append(" ");
                    }

                    stringBuilder.Append(textToAppend);
                }
            }

            var text = stringBuilder.ToString().Trim();
            return new SDTemplate(text, tokens);
        }
    }
}
