using System;

namespace SharpDox.Plugins.Chm
{
    internal static class Helper
    {      
        public static string RemoveIllegalCharacters(string filename)
        {
            var illegalCharacters = "\\ / : * ? \" < > | ä ü ö";
            var splitted = illegalCharacters.Split(' ');
            foreach (var character in splitted)
            {
                filename = filename.Replace(character, "");
            }
            return filename;
        }

        public static string TransformLinkToken(string linkType, Guid guid, string identifier)
        {
            var link = string.Empty;
            if (linkType == "image")
            {
                link = string.Format("{0}", identifier);
            }
            else if (guid != Guid.Empty)
            {
                link = string.Format("{0}.html", guid);
            }
            else
            {
                link = string.Format("{0}.html", identifier.Replace(' ', '_'));
            }
            return link;
        }
    }
}
