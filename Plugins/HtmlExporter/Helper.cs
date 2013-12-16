using System;

namespace SharpDox.Plugins.Html
{
    public static class Helper
    {      
        public static string TransformLinkToken(string linkType, Guid guid, string identifier)
        {
            var link = string.Empty;
            if (linkType == "image")
            {
                link = string.Format("{0}s/{1}", linkType, identifier);
            }
            else if (guid != Guid.Empty)
            {
                link = string.Format("{0}/{1}.html", linkType, guid);
            }
            else
            {
                link = string.Format("{0}/{1}.html", linkType, identifier);
            }
            return link;
        }
    }
}
