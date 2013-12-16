using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgPolygon
    {
        private XmlAttribute _points;
        private XmlAttribute _fill;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;

        public SvgPolygon(SvgRoot svg, string points)
        {
            CreateElement(svg);

            Points = points;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("polygon");

            _points = svg.CreateAttribute("points");
            _fill = svg.CreateAttribute("fill");
            _stroke = svg.CreateAttribute("stroke");
            _strokeWidth = svg.CreateAttribute("stroke-width");

            XmlElement.Attributes.Append(_points);
            XmlElement.Attributes.Append(_fill);
            XmlElement.Attributes.Append(_stroke);
            XmlElement.Attributes.Append(_strokeWidth);
        }

        public XmlElement XmlElement { get; set; }
        public string Points { get { return _points.Value; } set { _points.Value = value; } }
        public string Fill { get { return _fill.Value; } set { _fill.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
    }
}