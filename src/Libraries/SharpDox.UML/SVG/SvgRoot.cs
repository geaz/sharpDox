using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRoot : XmlDocument
    {
        private XmlAttribute _width;
        private XmlAttribute _height;

        private XmlElement _root;

        public SvgRoot()
        {
            CreateElement();
        }

        public void Add(SvgElement element)
        {
            _root.AppendChild(element);
        }

        public void ImportAdd(SvgElement element)
        {
            var importedNode = ImportNode(element, true);
            _root.AppendChild(importedNode);
        }

        public new string ToString()
        {
            return OuterXml;
        }

        private void CreateElement()
        {
            _root = CreateElement("svg");

            var xmlns = CreateAttribute("xmlns");
            xmlns.Value = "http://www.w3.org/2000/svg";

            var xlink = CreateAttribute("xmlns:xlink");
            xlink.Value = "http://www.w3.org/1999/xlink";

            _width = CreateAttribute("width");
            _height = CreateAttribute("height");

            _root.Attributes.Append(_width);
            _root.Attributes.Append(_height); 
            _root.Attributes.Append(xmlns);
            _root.Attributes.Append(xlink);
            AppendChild(_root);
        }


        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
    }
}
