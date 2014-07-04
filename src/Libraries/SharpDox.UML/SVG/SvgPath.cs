using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgPath : SvgElement
    {
        private XmlAttribute _d;
        private XmlAttribute _stroke;
        private XmlAttribute _strokeWidth;
        private XmlAttribute _strokeDashArray;

        public SvgPath(SvgRoot rootSvg, string d) : base(rootSvg, "path")
        {
            CreateElement();
            D = d;
        }

        private void CreateElement()
        {
            _d = _rootSvg.CreateAttribute("d");
            _stroke = _rootSvg.CreateAttribute("stroke");
            _strokeWidth = _rootSvg.CreateAttribute("stroke-width");
            _strokeDashArray = _rootSvg.CreateAttribute("stroke-dasharray");

            Attributes.Append(_d);
            Attributes.Append(_stroke);
            Attributes.Append(_strokeWidth);
            Attributes.Append(_strokeDashArray);
        }

        public string D { get { return _d.Value; } set { _d.Value = value; } }
        public string Stroke { get { return _stroke.Value; } set { _stroke.Value = value; } }
        public double StrokeWidth { get { return double.Parse(_strokeWidth.Value, CultureInfo.InvariantCulture); } set { _strokeWidth.Value = value.ToString("0"); } }
        public string StrokeDashArray { get { return _strokeDashArray.Value; } set { _strokeDashArray.Value = value; } }
    }
}