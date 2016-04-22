using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgElement : XmlElement 
    {
        protected readonly SvgRoot _rootSvg;

        public SvgElement(SvgRoot rootSvg, string elementName) : base(string.Empty, elementName, string.Empty, rootSvg) 
        {
            _rootSvg = rootSvg;
        }
    }
}
