using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SharpDox.Model.Repository;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.SVG;
using System.Globalization;

namespace SharpDox.UML.Sequence.Model
{
    internal class SequenceDiagram : SequenceDiagramComposite, ISDDiagram
    {
        private readonly SequenceDiagramPngRenderer _sequenceDiagramPngRenderer;
        private readonly SequenceDiagramSvgRenderer _sequenceDiagramSvgRenderer;
        private readonly List<SDType> _types;

        private DrawingVisual _renderedDiagram;
        private SvgRoot _renderedSvgDiagram;

        public SequenceDiagram(List<SDType> types)
        {
            _types = types;
            _sequenceDiagramPngRenderer = new SequenceDiagramPngRenderer();
            _sequenceDiagramSvgRenderer = new SequenceDiagramSvgRenderer();

            Nodes = new List<SequenceDiagramNode>();
        }

        public SequenceDiagramElement AddNode(string typeIdentifier)
        {
            var node = new SequenceDiagramNode
            {
                TypeIdentifier = typeIdentifier,
                Text = _types.Single(t => t.Identifier == typeIdentifier).Fullname
            };
            Nodes.Add(node);

            return node;
        }

        public void ToPng(string outputFilepath)
        {
            if (_renderedDiagram == null)
            {
                _renderedDiagram = _sequenceDiagramPngRenderer.RenderDiagram(this);
            }
            _renderedDiagram.SaveAsPng(outputFilepath);
        }

        public string ToSvg(double maxWidth)
        {
            _renderedSvgDiagram = _sequenceDiagramSvgRenderer.RenderDiagram(this);

            if (_renderedSvgDiagram.Width > maxWidth)
            {
                _renderedSvgDiagram.Scale = maxWidth / _renderedSvgDiagram.Width;
            }

            return _renderedSvgDiagram.ToString();
        }

        public Guid StartNodeID { get; set; }
        public List<SequenceDiagramNode> Nodes { get; set; }
    }
}
