using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRoot
    {
        public SvgRoot(double height, double width)
        {
            Document = new XmlDocument();

            GraphicsElement = CreateElement("g");

            Root = CreateElement("svg");
            Root.AppendChild(GraphicsElement);

            Document.AppendChild(Root);
            
            Height = height;
            Width = width;
        }

        public XmlElement CreateElement(string name)
        {
            return Document.CreateElement(name);
        }

        public XmlAttribute CreateAttribute(string name)
        {
            return Document.CreateAttribute(name);
        }

        public XmlAttribute CreateAttribute(string name, string xmlNamespace)
        {
            return Document.CreateAttribute(xmlNamespace, name, xmlNamespace);
        }

        public void AppendChild(XmlElement element)
        {
            GraphicsElement.AppendChild(element);
        }

        public void ImportAppendToRoot(XmlNode node)
        {
            var importedNode = Document.ImportNode(node, true);
            Root.AppendChild(importedNode);
        }

        public new string ToString()
        {
            return Document.OuterXml;
        }

        public XmlDocument Document { get; set; }
        public XmlElement Root { get; set; }
        public XmlElement GraphicsElement { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }
}
