using System;
using System.Collections.Generic;

namespace SharpDox.Model.Documentation.Token
{
    public class SDTokenList : List<SDToken>
    {
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
            return text;
        }

        public SDTemplate ToMarkdown(Dictionary<string, string> tokens)
        {
            var text = string.Empty;
            foreach (var token in this)
            {
                switch (token.Role)
                {
                    case SDTokenRole.Paragraph:
                        text += string.Format("{0}{1}{1}", token.Text, Environment.NewLine);
                        break;
                    case SDTokenRole.Code:
                        var splittedText = token.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        text += ((SDCodeToken)token).IsInline ?
                                    string.Format("{0}{1}{0}", "```", token.Text) : 
                                    string.Format("{0}{1}{2}{1}{0}", "```", Environment.NewLine, string.Join(Environment.NewLine, splittedText));
                        break;
                    case SDTokenRole.See:
                        var seeToken = (SDSeeToken)token;
                        if(string.IsNullOrEmpty(seeToken.DeclaringType))
                        {
                            text += string.Format("[{0}]({{{{type-link:{1}.{0}}}}})", seeToken.Name, seeToken.Namespace);
                        }
                        else
                        {
                            text += seeToken.Text; 
                        }
                        break;
                    default:
                        text += token.Text;
                        break;
                }
            }
            return new SDTemplate(text, tokens);
        }
    }
}
