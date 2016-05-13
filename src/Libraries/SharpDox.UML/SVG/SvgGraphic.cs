using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgGraphic : SvgElement
    {
        public SvgGraphic(SvgRoot rootSvg) : base(rootSvg, "g") { }

        public void Add(SvgElement element)
        {
            AppendChild((XmlElement)element);
        }
    }
}
