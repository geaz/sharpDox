using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using SharpDox.UML.Extensions;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.Sequence.Model;
using SharpDox.UML.SVG;

namespace SharpDox.UML.Sequence
{
    internal class SequenceDiagramSvgRenderer
    {
        private const int ROWOFFSET = 15;

        private int _openBlocks = 0;
        private Size _diagramSize;
        private SvgRoot _svgRoot;
        private Dictionary<Guid, double> _nodeMiddlePoints;

        public SvgRoot RenderDiagram(SequenceDiagram sequenceDiagram)
        {
            _nodeMiddlePoints = new Dictionary<Guid, double>();
            _diagramSize = new Size(0.5, 80.5);
            _svgRoot = new SvgRoot(0, 0);

            DrawAllNodes(sequenceDiagram);
            DrawAllDiagramElements(sequenceDiagram);
            DrawVerticalLines(sequenceDiagram);

            _svgRoot.Height = _diagramSize.Height;
            _svgRoot.Width = _diagramSize.Width;

            return _svgRoot;
        }

        private void DrawAllNodes(SequenceDiagram sequenceDiagram)
        {
            foreach (var node in sequenceDiagram.Nodes)
            {
                DrawNode(node);
            }
        }

        private void DrawNode(SequenceDiagramNode node)
        {
            var textWidth = node.Text.GetWidth(12, Fonts.FontLight);
            var textPosition = new Point(_diagramSize.Width, 10);
            var textSize = new Size(textWidth + 20, 35);

            _diagramSize = new Size(_diagramSize.Width + textWidth + 40, _diagramSize.Height);

            var rectangle = new SvgRectangle(_svgRoot, textPosition.X, textPosition.Y, textSize.Width, textSize.Height);
            rectangle.StrokeWidth = 1;
            rectangle.Stroke = "#979797";
            rectangle.Fill = "#FFFFFF";

            var link = new SvgLink(_svgRoot, node.Text, string.Format("{{{{type-link:{0}}}}}", node.TypeIdentifier), textPosition.X + 15, textPosition.Y + 22);
            link.Text.FontSize = 12;

            _svgRoot.AppendChild(rectangle.XmlElement);
            _svgRoot.AppendChild(link.XmlElement);

            _nodeMiddlePoints.Add(node.ID, textSize.Width / 2 + textPosition.X);
        }

        private void DrawAllDiagramElements(SequenceDiagram sequenceDiagram)
        {
            foreach (var cont in sequenceDiagram.Content)
            {
                DrawDiagramElement(cont);
            }
        }

        private void DrawDiagramElement(SequenceDiagramElement element)
        {
            var connection = element as SequenceDiagramConnection;
            if (connection != null && !connection.IsReturnConnection)
            {
                DrawConnection(connection);
            }
            else if (connection != null)
            {
                DrawReturnConnection(connection);
            }

            var composite = element as SequenceDiagramComposite;
            if (composite != null)
            {
                OpenBlock(composite);
                foreach (var cont in composite.Content)
                {
                    DrawDiagramElement(cont);
                }
                CloseBlock();
            }
        }

        private void DrawConnection(SequenceDiagramConnection connection)
        {
            var callerNodeMiddlePoint = connection.CallerId == Guid.Empty ? 0 : _nodeMiddlePoints[connection.CallerId];
            var calledNodeMiddlePoint = _nodeMiddlePoints[connection.CalledId];

            var textWidth = connection.Text.GetWidth(12, Fonts.FontLight);
            var link = new SvgLink(_svgRoot, connection.Text, string.Format("{{{{method-link:{0}}}}}", connection.CalledMethodIdentifier), callerNodeMiddlePoint + 10, _diagramSize.Height + 10);
            link.Text.FontSize = 12;
            _svgRoot.AppendChild(link.XmlElement);

            if ((textWidth + callerNodeMiddlePoint + 10) > _diagramSize.Width)
            {
                _diagramSize.Width = textWidth + callerNodeMiddlePoint + 20;
            }

            DrawConnectionLine(connection, callerNodeMiddlePoint, calledNodeMiddlePoint);
            DrawConnectionArrow(calledNodeMiddlePoint);
        }

        private void DrawConnectionLine(SequenceDiagramConnection connection, double callerNodeMiddlePoint, double calledNodeMiddlePoint)
        {
            if (connection.CallerId == connection.CalledId)
            {
                double x1 = callerNodeMiddlePoint;
                double x2 = callerNodeMiddlePoint - 10;
                double y1 = _diagramSize.Height + 10;
                double y2 = _diagramSize.Height + 20;

                var path = new SvgPath(_svgRoot, string.Format("M{0},{1}L{2},{1}",
                    x1.ToString("0.00", CultureInfo.InvariantCulture),
                    y1.ToString("0.00", CultureInfo.InvariantCulture),
                    x2.ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";

                var path2 = new SvgPath(_svgRoot, string.Format("M{0},{1}L{0},{2}",
                    x2.ToString("0.00", CultureInfo.InvariantCulture),
                    y1.ToString("0.00", CultureInfo.InvariantCulture),
                    y2.ToString("0.00", CultureInfo.InvariantCulture)));
                path2.StrokeWidth = 1;
                path2.Stroke = "#979797";

                var path3 = new SvgPath(_svgRoot, string.Format("M{0},{1}L{2},{1}",
                    x2.ToString("0.00", CultureInfo.InvariantCulture),
                    y2.ToString("0.00", CultureInfo.InvariantCulture),
                    x1.ToString("0.00", CultureInfo.InvariantCulture)));
                path3.StrokeWidth = 1;
                path3.Stroke = "#979797";

                _svgRoot.AppendChild(path.XmlElement);
                _svgRoot.AppendChild(path2.XmlElement);
                _svgRoot.AppendChild(path3.XmlElement);
            }
            else
            {
                var path = new SvgPath(_svgRoot, string.Format("M{0},{2}L{1},{2}",
                    callerNodeMiddlePoint.ToString("0.00", CultureInfo.InvariantCulture),
                    calledNodeMiddlePoint.ToString("0.00", CultureInfo.InvariantCulture), 
                    (_diagramSize.Height + 20).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";

                _svgRoot.AppendChild(path.XmlElement);
            }

            _diagramSize.Height += 20;
        }

        private void DrawConnectionArrow(double calledNodeMiddlePoint)
        {
            var startX = (int)calledNodeMiddlePoint;
            var startY = (int)_diagramSize.Height;

            var points = string.Format("{0},{1} {2},{3} {2},{4}", startX, startY, startX - 5, startY - 5, startY + 5);
            var arrow = new SvgPolygon(_svgRoot, points);
            arrow.Stroke = arrow.Fill = "#979797";

            _svgRoot.AppendChild(arrow.XmlElement);

            _diagramSize.Height += ROWOFFSET;
        }

        private void DrawReturnConnection(SequenceDiagramConnection connection)
        {
            var callerNodeMiddlePoint = _nodeMiddlePoints[connection.CallerId];
            var calledNodeMiddlePoint = connection.CalledId == Guid.Empty ? 0 : _nodeMiddlePoints[connection.CalledId];

            var textWidth = ("return " + connection.Text).GetWidth(12, Fonts.FontLight);
            var text = new SvgText(_svgRoot, "return " + connection.Text, calledNodeMiddlePoint + 10, _diagramSize.Height + 10);
            text.FontSize = 12;
            _svgRoot.AppendChild(text.XmlElement);

            if ((textWidth + calledNodeMiddlePoint + 10) > _diagramSize.Width)
            {
                _diagramSize.Width = textWidth + calledNodeMiddlePoint + 20;
            }

            var path = new SvgPath(_svgRoot, string.Format("M{0},{1}L{2},{1}",
                    calledNodeMiddlePoint.ToString("0.00", CultureInfo.InvariantCulture),
                    (_diagramSize.Height + 20).ToString("0.00", CultureInfo.InvariantCulture),
                    callerNodeMiddlePoint.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";
            _svgRoot.AppendChild(path.XmlElement);

            var startX = (int)calledNodeMiddlePoint;
            var startY = (int)_diagramSize.Height + 20;

            var points = string.Format("{0},{1} {2},{3} {2},{4}", startX, startY, startX + 5, startY + 5, startY - 5);
            var arrow = new SvgPolygon(_svgRoot, points);
            arrow.Stroke = arrow.Fill = "#979797";

            _svgRoot.AppendChild(arrow.XmlElement);

            _diagramSize.Height += 35;
        }
        
        private void OpenBlock(SequenceDiagramComposite block)
        {
            var textWidth = block.Text.GetWidth(12, Fonts.FontLight);
            var text = new SvgText(_svgRoot, block.Text, 20 + _openBlocks * 15, _diagramSize.Height + 10);
            text.FontSize = 10;
            _svgRoot.AppendChild(text.XmlElement);

            // Setzen der Breite des Diagramms auf die eventuell größere Breite des Textes
            if ((textWidth + 20 + _openBlocks * 15) > _diagramSize.Width)
                _diagramSize.Width = textWidth + 20 + _openBlocks * 15 + 10;

            _diagramSize.Height += 20;
            _openBlocks++;
        }

        private void CloseBlock()
        {
            _openBlocks--;

            var text = new SvgText(_svgRoot, "end", 20 + _openBlocks * 15, _diagramSize.Height);
            text.FontSize = 10;
            _svgRoot.AppendChild(text.XmlElement);

            _diagramSize.Height += 20;
        }

        private void DrawVerticalLines(SequenceDiagram sequenceDiagram)
        {
            foreach (var node in _nodeMiddlePoints)
            {
                var path = new SvgPath(_svgRoot, string.Format("M{0},{1}L{0},{2}", 
                    node.Value.ToString("0.50", CultureInfo.InvariantCulture), 
                    45, 
                    _diagramSize.Height.ToString("0.50", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";

                _svgRoot.AppendChild(path.XmlElement);
            }
        }
    }
}
