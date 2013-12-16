using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgLink
    {
        private XmlAttribute _href;

        public SvgLink(SvgRoot svg, string text, string href, double x, double y)
        {
            Text = new SvgText(svg, text, x, y);
            CreateElement(svg);

            Text.Fill = "#1382CE";
            Text.OnMouseOut = "this.setAttribute('fill', '#1382CE')";
            Text.OnMouseOver = "this.setAttribute('fill', '#F58026')";

            Href = href;
            Text.X = x;
            Text.Y = y;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("a");
            _href = svg.CreateAttribute("href", "xlink");

            XmlElement.Attributes.Append(_href);
            XmlElement.AppendChild(Text.XmlElement);
        }

        public XmlElement XmlElement { get; set; }
        public SvgText Text { get; set; }
        public string Href { get { return _href.Value; } set { _href.Value = value; } }
    }
}