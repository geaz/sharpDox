using System.Globalization;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgRoot
    {
        private readonly XmlElement _root;
        private readonly XmlElement _g;
        private readonly XmlAttribute _width;
        private readonly XmlAttribute _height;
        private readonly XmlAttribute _transform;

        private double _currentScale;

        public SvgRoot(double height, double width)
        {
            Document = new XmlDocument();

            _height = Document.CreateAttribute("height");
            _width = Document.CreateAttribute("width");
            _transform = Document.CreateAttribute("transform");

            _g = CreateElement("g");
            _g.Attributes.Append(_transform);

            _root = CreateElement("svg");
            _root.Attributes.Append(_height);
            _root.Attributes.Append(_width);
            _root.AppendChild(_g);

            Document.AppendChild(_root);
            

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
            _g.AppendChild(element);
        }

        public new string ToString()
        {
            return Document.OuterXml;
        }

        public XmlDocument Document { get; set; }
        public double Width { get { return double.Parse(_width.Value, CultureInfo.InvariantCulture); } set { _width.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        public double Height { get { return double.Parse(_height.Value, CultureInfo.InvariantCulture); } set { _height.Value = value.ToString("0.00", CultureInfo.InvariantCulture); } }
        
        public double Scale 
        { 
            get { return _currentScale; } 
            set 
            {
                Height = Height * value;
                Width = Width * value;

                _transform.Value = string.Format("scale({0})", value.ToString("0.00", CultureInfo.InvariantCulture));
                _currentScale = value; 
            } 
        }
    }
}
