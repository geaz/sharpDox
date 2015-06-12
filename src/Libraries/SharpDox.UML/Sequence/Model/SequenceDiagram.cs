using System;
using System.Collections.Generic;
using System.Windows.Media;
using SharpDox.UML.Extensions;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.SVG;
using SharpDox.Model.Documentation;
using SharpDox.Model.Repository;

namespace SharpDox.UML.Sequence.Model
{
    internal class SequenceDiagram : SequenceDiagramComposite, ISDDiagram
    {
        private readonly SequenceDiagramPngRenderer _sequenceDiagramPngRenderer;
        private readonly SequenceDiagramSvgRenderer _sequenceDiagramSvgRenderer;
        private readonly SDRepository _sdRepository;

        private DrawingVisual _renderedDiagram;
        private SvgRoot _renderedSvgDiagram;

        public SequenceDiagram(SDRepository sdRepository)
        {
            _sdRepository = sdRepository;
            _sequenceDiagramPngRenderer = new SequenceDiagramPngRenderer();
            _sequenceDiagramSvgRenderer = new SequenceDiagramSvgRenderer();

            Nodes = new List<SequenceDiagramNode>();
        }

        public SequenceDiagramElement AddNode(string typeIdentifier)
        {
            var node = new SequenceDiagramNode
            {
                TypeIdentifier = typeIdentifier,
                Text = _sdRepository.GetTypeByIdentifier(typeIdentifier).Fullname
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

        public SDTemplate ToSvg()
        {
            _renderedSvgDiagram = _sequenceDiagramSvgRenderer.RenderDiagram(this);
            return new SDTemplate(_renderedSvgDiagram.ToString());
        }

        public Guid StartNodeID { get; set; }
        public List<SequenceDiagramNode> Nodes { get; set; }
    }
}
