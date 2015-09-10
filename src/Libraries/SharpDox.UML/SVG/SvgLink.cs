using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgLink : SvgElement
    {
        private XmlAttribute _href;

        public SvgLink(SvgRoot rootSvg, string text, string href, double x, double y) : base(rootSvg, "a")
        {
            Text = new SvgText(rootSvg, text, x, y);
            CreateElement();

            Text.Fill = "#1382CE";
            Text.OnMouseOut = "this.setAttribute('fill', '#1382CE')";
            Text.OnMouseOver = "this.setAttribute('fill', '#F58026')";

            Href = href;
            Text.X = x;
            Text.Y = y;
        }

        private void CreateElement()
        {
            _href = _rootSvg.CreateAttribute("xlink:href", "http://www.w3.org/1999/xlink");
            Attributes.Append(_href);

            var target = _rootSvg.CreateAttribute("target");
            target.Value = "_top";
            Attributes.Append(target);

            AppendChild(Text);
        }

        public SvgText Text { get; set; }
        public string Href { get { return _href.Value; } set { _href.Value = value; } }
    }
}