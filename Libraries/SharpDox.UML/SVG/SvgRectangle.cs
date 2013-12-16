using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRectangle
    {
        private XmlAttribute _width;
        private XmlAttribute _height;
        private XmlAttribute _x;
        private XmlAttribute _y;
        private XmlAttribute _fill;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;

        public SvgRectangle(SvgRoot svg, double x, double y, double width, double height)
        {
            CreateElement(svg);

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("rect");

            _height = svg.CreateAttribute("height");
            _width = svg.CreateAttribute("width");
            _x = svg.CreateAttribute("x");
            _y = svg.CreateAttribute("y");
            _fill = svg.CreateAttribute("fill");
            _stroke = svg.CreateAttribute("stroke");
            _strokeWidth = svg.CreateAttribute("stroke-width");

            XmlElement.Attributes.Append(_height);
            XmlElement.Attributes.Append(_width);
            XmlElement.Attributes.Append(_x);
            XmlElement.Attributes.Append(_y);
            XmlElement.Attributes.Append(_fill);
            XmlElement.Attributes.Append(_stroke);
            XmlElement.Attributes.Append(_strokeWidth);
        }

        public XmlElement XmlElement { get; set; }
        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0"); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0"); } }
        public double X { get { return double.Parse(_x.Value, CultureInfo.InvariantCulture); } set { _x.Value = value.ToString("0.50", CultureInfo.InvariantCulture); } }
        public double Y { get { return double.Parse(_y.Value, CultureInfo.InvariantCulture); } set { _y.Value = value.ToString("0.50", CultureInfo.InvariantCulture); } }
        public string Fill { get { return _fill.Value; } set { _fill.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
    }
}