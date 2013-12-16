using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgPath
    {
        private XmlAttribute _d;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;

        public SvgPath(SvgRoot svg, string d)
        {
            CreateElement(svg);
            D = d;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("path");

            _d = svg.CreateAttribute("d");
            _stroke = svg.CreateAttribute("stroke");
            _strokeWidth = svg.CreateAttribute("stroke-width");

            XmlElement.Attributes.Append(_d);
            XmlElement.Attributes.Append(_stroke);
            XmlElement.Attributes.Append(_strokeWidth);
        }

        public XmlElement XmlElement { get; set; }
        public string D { get { return _d.Value; } set { _d.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
    }
}