using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SharpDox.UML.SVG
{
    internal class SvgGraphic : SvgElement
    {
        public SvgGraphic(SvgRoot rootSvg) : base(rootSvg, "g") { }

        public void Add(SvgElement element)
        {
            AppendChild((XmlElement)element);
        }
    }
}
