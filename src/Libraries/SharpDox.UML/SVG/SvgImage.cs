using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgImage : SvgElement
    {
        private XmlAttribute _width;
        private XmlAttribute _height;
        private XmlAttribute _x;
        private XmlAttribute _y;
        private XmlAttribute _href;

        public SvgImage(SvgRoot rootSvg, double x, double y, double width, double height, string href) : base(rootSvg, "image")
        {
            CreateElement();

            X = x;
            Y = y;
            Width = width;
            Height = height;
            Href = href;
        }

        private void CreateElement()
        {
            _width = _rootSvg.CreateAttribute("width");
            _height = _rootSvg.CreateAttribute("height");
            _x = _rootSvg.CreateAttribute("x");
            _y = _rootSvg.CreateAttribute("y");
            _href = _rootSvg.CreateAttribute("xlink:href", "http://www.w3.org/1999/xlink");

            Attributes.Append(_width);
            Attributes.Append(_height);
            Attributes.Append(_x);
            Attributes.Append(_y);
            Attributes.Append(_href);
        }

        public double X { get { return double.Parse(_x.Value, CultureInfo.InvariantCulture); } set { _x.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Y { get { return double.Parse(_y.Value, CultureInfo.InvariantCulture); } set { _y.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public string Href { get { return _href.Value; } set { _href.Value = value; } }
    }
}