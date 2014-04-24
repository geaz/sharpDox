using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgPolygon : SvgElement
    {
        private XmlAttribute _points;
        private XmlAttribute _fill;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;

        public SvgPolygon(SvgRoot rootSvg, string points) : base(rootSvg, "polygon")
        {
            CreateElement();

            Points = points;
        }

        private void CreateElement()
        {
            _points = _rootSvg.CreateAttribute("points");
            _fill = _rootSvg.CreateAttribute("fill");
            _stroke = _rootSvg.CreateAttribute("stroke");
            _strokeWidth = _rootSvg.CreateAttribute("stroke-width");

            Attributes.Append(_points);
            Attributes.Append(_fill);
            Attributes.Append(_stroke);
            Attributes.Append(_strokeWidth);
        }

        public string Points { get { return _points.Value; } set { _points.Value = value; } }
        public string Fill { get { return _fill.Value; } set { _fill.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
    }
}