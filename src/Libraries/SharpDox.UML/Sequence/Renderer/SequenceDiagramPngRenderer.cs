using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.Sequence.Model;

namespace SharpDox.UML.Sequence
{
    internal class SequenceDiagramPngRenderer
    {
        private const int ROWOFFSET = 15;

        private int _openBlocks = 0;
        private Size _diagramSize;
        private DrawingContext _context;
        private Dictionary<Guid, double> _nodeMiddlePoints;
 
        public DrawingVisual RenderDiagram(SequenceDiagram sequenceDiagram)
        {
            _nodeMiddlePoints = new Dictionary<Guid, double>();
            _diagramSize = new Size(0.5, 80.5);

            var canvas = new DrawingVisual();
            _context = canvas.RenderOpen();

            DrawAllNodes(sequenceDiagram);
            DrawAllDiagramElements(sequenceDiagram);
            DrawVerticalLines(sequenceDiagram);

            _context.Close();
            return canvas;
        }

        private void DrawAllNodes(SequenceDiagram sequenceDiagram)
        {
            foreach (var node in sequenceDiagram.Nodes)
            {
                _diagramSize = DrawNode(_diagramSize, node);
            }
        }

        private Size DrawNode(Size diagramSize, SequenceDiagramNode node)
        {
            var text = new FormattedText(node.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 12, Brushes.Black);
            var position = new Point(diagramSize.Width, 10);
            var size = new Size(text.Width + 20, 35);

            diagramSize = new Size(diagramSize.Width + text.Width + 40, diagramSize.Height);

            var rect = new Rect(position.X, position.Y, size.Width, size.Height);
            _context.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 1), rect);
            _context.DrawText(text, new Point(position.X + 10, position.Y + 10));

            _nodeMiddlePoints.Add(node.ID, size.Width / 2 + position.X);

            return diagramSize;
        }

        private void DrawAllDiagramElements(SequenceDiagram sequenceDiagram)
        {
            foreach (var cont in sequenceDiagram.Content)
            {
                _diagramSize = DrawDiagramElement(cont, _diagramSize);
            }
        }

        private Size DrawDiagramElement(SequenceDiagramElement element, Size size)
        {
            var connection = element as SequenceDiagramConnection;
            if (connection != null && !connection.IsReturnConnection)
            {
                size = DrawConnection(size, connection);
            }
            else if (connection != null)
            {
                size = DrawReturnConnection(size, connection);
            }

            var composite = element as SequenceDiagramComposite;
            if (composite != null)
            {
                size = OpenBlock(composite, size);
                foreach (var cont in composite.Content)
                {
                    size = DrawDiagramElement(cont, size);
                }
                size = CloseBlock(size);
            }
            return size;
        }

        private Size DrawConnection(Size size, SequenceDiagramConnection connection)
        {
            var callerNodeMiddlePoint = connection.CallerId == Guid.Empty ? 0 : _nodeMiddlePoints[connection.CallerId];
            var calledNodeMiddlePoint = _nodeMiddlePoints[connection.CalledId];

            var text = new FormattedText(connection.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 12, Brushes.Black);
            _context.DrawText(text, new Point(callerNodeMiddlePoint + 10, size.Height));

            if ((text.Width + callerNodeMiddlePoint + 10) > size.Width)
            {
                size.Width = text.Width + callerNodeMiddlePoint + 20;
            }

            size = DrawConnectionLine(connection, callerNodeMiddlePoint, calledNodeMiddlePoint, size);
            size = DrawConnectionArrow(calledNodeMiddlePoint, size);

            return size;
        }

        private Size DrawConnectionLine(SequenceDiagramConnection connection, double callerNodeMiddlePoint, double calledNodeMiddlePoint, Size size)
        {
            if (connection.CallerId == connection.CalledId)
            {
                double x1 = callerNodeMiddlePoint;
                double x2 = callerNodeMiddlePoint - 10;
                double y1 = size.Height + 10;
                double y2 = size.Height + 20;

                _context.DrawLine(new Pen(Brushes.Black, 1), new Point(x1, y1), new Point(x2, y1));
                _context.DrawLine(new Pen(Brushes.Black, 1), new Point(x2, y1), new Point(x2, y2));
                _context.DrawLine(new Pen(Brushes.Black, 1), new Point(x2, y2), new Point(x1, y2));
            }
            else
            {
                _context.DrawLine(new Pen(Brushes.Black, 1),
                                 new Point(callerNodeMiddlePoint, size.Height + 20),
                                 new Point(calledNodeMiddlePoint, size.Height + 20));
            }

            size.Height += 20;
            return size;
        }

        private Size DrawConnectionArrow(double calledNodeMiddlePoint, Size size)
        {
            var startX = (int)calledNodeMiddlePoint;
            var startY = (int)size.Height;
            var start = new Point(startX, startY);
            var segments = new[] { new LineSegment(new Point(startX - 5, startY + 5), true), new LineSegment(new Point(startX - 5, startY - 5), true) };
            var figure = new PathFigure(start, segments, true);
            var geo = new PathGeometry(new[] { figure });
            _context.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 1), geo);

            size.Height += ROWOFFSET;
            return size;
        }

        private Size DrawReturnConnection(Size size, SequenceDiagramConnection connection)
        {
            var callerNodeMiddlePoint = _nodeMiddlePoints[connection.CallerId];
            var calledNodeMiddlePoint = connection.CalledId == Guid.Empty ? 0 : _nodeMiddlePoints[connection.CalledId];

            var text = new FormattedText("return " + connection.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 12, Brushes.Gray);
            _context.DrawText(text, new Point(calledNodeMiddlePoint + 10, size.Height));

            if ((text.Width + calledNodeMiddlePoint + 10) > size.Width)
            {
                size.Width = text.Width + calledNodeMiddlePoint + 20;
            }

            _context.DrawLine(new Pen(Brushes.Gray, 1), new Point(calledNodeMiddlePoint, size.Height + 20), new Point(callerNodeMiddlePoint, size.Height + 20));

            var startX = (int)calledNodeMiddlePoint;
            var startY = (int)size.Height + 20;
            var start = new Point(startX, startY);
            var segments = new[] { new LineSegment(new Point(startX + 5, startY + 5), true), new LineSegment(new Point(startX + 5, startY - 5), true) };
            var figure = new PathFigure(start, segments, true);
            var geo = new PathGeometry(new[] { figure });
            _context.DrawGeometry(Brushes.Gray, new Pen(Brushes.Gray, 1), geo);

            size.Height += 35;

            return size;
        }
        
        private Size OpenBlock(SequenceDiagramComposite block, Size size)
        {
            var text = new FormattedText(block.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 10, Brushes.DimGray);
            _context.DrawText(text, new Point(20 + _openBlocks * 15, size.Height));

            // Setzen der Breite des Diagramms auf die eventuell größere Breite des Textes
            if ((text.Width + 20 + _openBlocks * 15) > size.Width)
                size.Width = text.Width + 20 + _openBlocks * 15 + 10;
            
            size.Height += 20;
            _openBlocks++;

            return size;
        }

        private Size CloseBlock(Size size)
        {
            _openBlocks--;

            var text = new FormattedText("end", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 10, Brushes.DimGray);
            _context.DrawText(text, new Point(20 + _openBlocks * 15, size.Height));

            size.Height += 20;
            return size;
        }

        private void DrawVerticalLines(SequenceDiagram sequenceDiagram)
        {
            foreach (var node in _nodeMiddlePoints)
            {
                _context.DrawLine(new Pen(Brushes.Black, 1), new Point(node.Value, 45), new Point(node.Value, _diagramSize.Height));
            }
        }
    }
}
