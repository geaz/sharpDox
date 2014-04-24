using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRectangle : SvgElement
    {
        private XmlAttribute _width;
        private XmlAttribute _height;
        private XmlAttribute _x;
        private XmlAttribute _y;
        private XmlAttribute _fill;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;

        public SvgRectangle(SvgRoot rootSvg, double x, double y, double width, double height) : base(rootSvg, "rect")
        {
            CreateElement();

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        private void CreateElement()
        {
            _height = _rootSvg.CreateAttribute("height");
            _width = _rootSvg.CreateAttribute("width");
            _x = _rootSvg.CreateAttribute("x");
            _y = _rootSvg.CreateAttribute("y");
            _fill = _rootSvg.CreateAttribute("fill");
            _stroke = _rootSvg.CreateAttribute("stroke");
            _strokeWidth = _rootSvg.CreateAttribute("stroke-width");

            Attributes.Append(_height);
            Attributes.Append(_width);
            Attributes.Append(_x);
            Attributes.Append(_y);
            Attributes.Append(_fill);
            Attributes.Append(_stroke);
            Attributes.Append(_strokeWidth);
        }

        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0"); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0"); } }
        public double X { get { return double.Parse(_x.Value, CultureInfo.InvariantCulture); } set { _x.Value = value.ToString("0.50", CultureInfo.InvariantCulture); } }
        public double Y { get { return double.Parse(_y.Value, CultureInfo.InvariantCulture); } set { _y.Value = value.ToString("0.50", CultureInfo.InvariantCulture); } }
        public string Fill { get { return _fill.Value; } set { _fill.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
    }
}