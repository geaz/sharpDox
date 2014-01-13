using System;
using System.Collections.Generic;
using System.Windows.Media;
using SharpDox.Model.Repository;
using SharpDox.UML.Class.Renderer;
using SharpDox.UML.Extensions;
using SharpDox.UML.SVG;
using System.Globalization;

namespace SharpDox.UML.Class.Model
{
    internal class ClassDiagram : ISDDiagram
    {
        private readonly ClassDiagramPngRenderer _classDiagramPngRenderer;
        private readonly ClassDiagramSvgRenderer _classDiagramSvgRenderer;

        private DrawingVisual _renderedDiagram;
        private SvgRoot _renderedSvgDiagram;

        public ClassDiagram(string typeIdentifier, string name, string kind, string accessibility, string attribute)
        {
            _classDiagramPngRenderer = new ClassDiagramPngRenderer();
            _classDiagramSvgRenderer = new ClassDiagramSvgRenderer();

            TypeIdentifier = typeIdentifier;
            Name = name;
            Accessibility = string.Format("{0} {1} {2}", accessibility, attribute, kind);

            ConstructorRows = new List<ClassDiagramRow>();
            MethodRows = new List<ClassDiagramRow>();
            FieldRows = new List<ClassDiagramRow>();
            PropertyRows = new List<ClassDiagramRow>();
            EventRows = new List<ClassDiagramRow>();
        }

        public void ToPng(string outputFilepath)
        {
            if (_renderedDiagram == null)
            {
                _renderedDiagram = _classDiagramPngRenderer.RenderDiagram(this);
            }
            _renderedDiagram.SaveAsPng(outputFilepath);
        }

        public string ToSvg(double maxWidth)
        {
            _renderedSvgDiagram = _classDiagramSvgRenderer.RenderDiagram(this); 

            if (_renderedSvgDiagram.Width > maxWidth)
            {
                _renderedSvgDiagram.Scale = maxWidth / _renderedSvgDiagram.Width;
            }

            return _renderedSvgDiagram.ToString();
        }

        public string TypeIdentifier { get; set; }
        public string Accessibility { get; set; }
        public string Name { get; set; }

        public List<ClassDiagramRow> FieldRows { get; private set; }
        public List<ClassDiagramRow> ConstructorRows { get; private set; }
        public List<ClassDiagramRow> MethodRows { get; private set; }
        public List<ClassDiagramRow> PropertyRows { get; private set; }
        public List<ClassDiagramRow> EventRows { get; private set; }
    }
}
