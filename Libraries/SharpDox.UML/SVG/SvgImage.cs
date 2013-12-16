using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgImage
    {
        private XmlAttribute _width;
        private XmlAttribute _height;
        private XmlAttribute _x;
        private XmlAttribute _y;
        private XmlAttribute _preserveAspectRatio;
        private XmlAttribute _href;

        public SvgImage(SvgRoot svg, double x, double y, double width, double height, string href)
        {
            CreateElement(svg);

            X = x;
            Y = y;
            Width = width;
            Height = height;
            Href = href;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("image");

            _width = svg.CreateAttribute("width");
            _height = svg.CreateAttribute("height");
            _x = svg.CreateAttribute("x");
            _y = svg.CreateAttribute("y");
            _preserveAspectRatio = svg.CreateAttribute("preserveAspectRatio");
            _href = svg.CreateAttribute("href", "xlink");

            XmlElement.Attributes.Append(_width);
            XmlElement.Attributes.Append(_height);
            XmlElement.Attributes.Append(_x);
            XmlElement.Attributes.Append(_y);
            XmlElement.Attributes.Append(_preserveAspectRatio);
            XmlElement.Attributes.Append(_href);
        }

        public XmlElement XmlElement { get; set; }
        public double X { get { return double.Parse(_x.Value, CultureInfo.InvariantCulture); } set { _x.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Y { get { return double.Parse(_y.Value, CultureInfo.InvariantCulture); } set { _y.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public string PreserveAspectRatio { get { return _preserveAspectRatio.Value; } set { _preserveAspectRatio.Value = value; } }
        public string Href { get { return _href.Value; } set { _href.Value = value; } }
    }
}