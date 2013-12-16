using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgText
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

        public SvgText(SvgRoot svg, string text, double x, double y)
        {
            CreateElement(svg);

            X = x;
            Y = y;
            Text = text;
        }

        private void CreateElement(SvgRoot svg)
        {
            XmlElement = svg.CreateElement("text");

            _x = svg.CreateAttribute("x");
            _y = svg.CreateAttribute("y");
            _fill = svg.CreateAttribute("fill");
            _style = svg.CreateAttribute("style");
            _textAnchor = svg.CreateAttribute("text-anchor");
            _fontFamily = svg.CreateAttribute("font-family");
            _fontSize = svg.CreateAttribute("font-size");
            _onMouseOver = svg.CreateAttribute("onmouseover");
            _onMouseOut = svg.CreateAttribute("onmouseout");

            XmlElement.Attributes.Append(_x);
            XmlElement.Attributes.Append(_y);
            XmlElement.Attributes.Append(_fill);
            XmlElement.Attributes.Append(_style);
            XmlElement.Attributes.Append(_textAnchor);
            XmlElement.Attributes.Append(_fontFamily);
            XmlElement.Attributes.Append(_fontSize);
            XmlElement.Attributes.Append(_onMouseOver);
            XmlElement.Attributes.Append(_onMouseOut);
        }

        public XmlElement XmlElement { get; set; }
        public string Text { get { return XmlElement.InnerXml; } set { XmlElement.InnerXml = string.Format("<![CDATA[{0}]]>", value); } }
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