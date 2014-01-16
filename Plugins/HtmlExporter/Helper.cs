using SharpDox.Model.Repository;
using System;

namespace SharpDox.Plugins.Html
{
    public class Helper
    {
        private SDRepository _repository;

        public Helper(SDRepository repository)
        {
            _repository = repository;
        }

        public string TransformLinkToken(string linkType, Guid guid, string identifier)
        {
            var link = string.Empty;
            if (linkType == "image")
            {
                link = string.Format("../{0}s/{1}", linkType, identifier);
            }
            else if(linkType == "type" || linkType == "namespace")
            {
                link = string.Format("../{0}/{1}.html", linkType, guid);
            }
            else if (guid != Guid.Empty)
            {
                var member = _repository.GetMemberByIdentifier(identifier);
                link = string.Format("../{0}/{1}.html#{2}", "type", member.DeclaringType.Guid, guid);
            }
            else
            {
                link = string.Format("../{0}/{1}.html", linkType, identifier.Replace(' ', '_'));
            }
            return link;
        }
    }
}
