using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRoot : XmlDocument
    {
        private readonly XmlElement _root;

        public SvgRoot()
        {
            _root = CreateElement("svg");
            AppendChild(_root);
        }

        public void Add(SvgElement element)
        {
            _root.AppendChild((XmlElement)element);
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
    }
}
