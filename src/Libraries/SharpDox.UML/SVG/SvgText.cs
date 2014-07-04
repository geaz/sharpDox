using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgText : SvgElement
    {
        private XmlAttribute _x;
        private XmlAttribute _y;
        private XmlAttribute _style;
        private XmlAttribute _textAnchor;
        private XmlAttribute _fontFamily;
        private XmlAttribute _fontSize;
        private XmlAttribute _fill;
        private XmlAttribute _onMouseOver;
        private XmlAttribute _onMouseOut;

        public SvgText(SvgRoot rootSvg, string text, double x, double y) : base(rootSvg, "text")
        {
            CreateElement();

            X = x;
            Y = y;
            Text = text;
        }

        private void CreateElement()
        {
            _x = _rootSvg.CreateAttribute("x");
            _y = _rootSvg.CreateAttribute("y");
            _fill = _rootSvg.CreateAttribute("fill");
            _style = _rootSvg.CreateAttribute("style");
            _textAnchor = _rootSvg.CreateAttribute("text-anchor");
            _fontFamily = _rootSvg.CreateAttribute("font-family");
            _fontSize = _rootSvg.CreateAttribute("font-size");
            _onMouseOver = _rootSvg.CreateAttribute("onmouseover");
            _onMouseOut = _rootSvg.CreateAttribute("onmouseout");

            Attributes.Append(_x);
            Attributes.Append(_y);
            Attributes.Append(_fill);
            Attributes.Append(_style);
            Attributes.Append(_textAnchor);
            Attributes.Append(_fontFamily);
            Attributes.Append(_fontSize);
            Attributes.Append(_onMouseOver);
            Attributes.Append(_onMouseOut);
        }

        public string Text { get { return InnerXml; } set { InnerXml = string.Format("<![CDATA[{0}]]>", value.Replace("]]>", "]]&gt;").Replace("]", "] ")); } }
        public double X { get { return double.Parse(_x.Value, CultureInfo.InvariantCulture); } set { _x.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Y { get { return double.Parse(_y.Value, CultureInfo.InvariantCulture); } set { _y.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public string Fill { get { return _fill.Value; } set { _fill.Value = value; } }
        public string Style { get { return _style.Value; } set { _style.Value = value; } }
        public string TextAnchor { get { return _textAnchor.Value; } set { _textAnchor.Value = value; } }
        public string FontFamily { get { return _fontFamily.Value; } set { _fontFamily.Value = value; } }
        public int FontSize { get { return int.Parse(_fontSize.Value, CultureInfo.InvariantCulture); } set { _fontSize.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public string OnMouseOver { get { return _onMouseOver.Value; } set { _onMouseOver.Value = value; } }
        public string OnMouseOut { get { return _onMouseOut.Value; } set { _onMouseOut.Value = value; } }
    }
}