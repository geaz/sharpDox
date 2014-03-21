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

        public string ToMarkdown()
        {
            var text = string.Empty;

            foreach (var token in this)
            {
                switch (token.Role)
                {
                    case SDTokenRole.Paragraph:
                        text += string.Format("<p>{0}</p>", token.Text);
                        break;
                    case SDTokenRole.Code:
                        var splittedText = token.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < splittedText.Length; i++)
                        {
                            splittedText[i] = "    " + splittedText[i];
                        }
                        text += ((SDCodeToken)token).IsInline ? 
                                    string.Format("<span class=\"inline-code\">{0}</span>", token.Text) : 
                                    string.Join(Environment.NewLine, splittedText);
                        break;
                    default:
                        text += token.Text;
                        break;
                }
            }
            return text;
        }
    }
}
