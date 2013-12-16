using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SharpDox.UML.Class.Model;

namespace SharpDox.UML.Class.Renderer
{
    internal class ClassDiagramPngRenderer
    {
        private const int OFFSET = 10;

        private DrawingContext _context;
        private Size _diagramSize;

        public DrawingVisual RenderDiagram(ClassDiagram classDiagram)
        {
            _diagramSize = CalculateDiagramSize(classDiagram);

            var canvas = new DrawingVisual();
            _context = canvas.RenderOpen();

            int position = OFFSET;
            position = RenderHeader(classDiagram, position);
            RenderAllRowSections(classDiagram, position);
            RenderDiagramFrame(classDiagram);

            return canvas;
        }

        private Size CalculateDiagramSize(ClassDiagram classDiagram)
        {
            var allRows = GetAllRows(classDiagram);
            var headerWidth = (int)Math.Max(classDiagram.Name.GetWidth(12, Fonts.FontLight), classDiagram.Accessibility.GetWidth(10, Fonts.FontItalic));
            var maxWidthRows = allRows.Count > 0 ? allRows.Max(o => (int)o.Text.GetWidth(12, Fonts.FontLight)) : 0;
            var horizontalLineCount = CountSections(classDiagram);

            var size = new Size();
            size.Width = Math.Max(headerWidth, maxWidthRows) + 65;
            size.Height = (3 * 20) + (10 * (horizontalLineCount > 0 ? horizontalLineCount - 1 : 0)) + allRows.Count * 20;

            return size;
        }

        private List<ClassDiagramRow> GetAllRows(ClassDiagram classDiagram)
        {
            var allRows = new List<ClassDiagramRow>();
            allRows.AddRange(classDiagram.FieldRows);
            allRows.AddRange(classDiagram.PropertyRows);
            allRows.AddRange(classDiagram.MethodRows);
            allRows.AddRange(classDiagram.EventRows);
            return allRows;
        }

        private int CountSections(ClassDiagram classDiagram)
        {
            return (classDiagram.FieldRows.Count > 0 ? 1 : 0) + (classDiagram.PropertyRows.Count > 0 ? 1 : 0) +
                   (classDiagram.MethodRows.Count > 0 ? 1 : 0) + (classDiagram.EventRows.Count > 0 ? 1 : 0);
        }

        private int RenderHeader(ClassDiagram classDiagram, int position)
        {
            var formattedAccessibility = new FormattedText(classDiagram.Accessibility, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontItalic, 10, Brushes.Black);
            _context.DrawText(formattedAccessibility, new Point((_diagramSize.Width - classDiagram.Accessibility.GetWidth(10, Fonts.FontItalic)) / 2, position));

            var formattedName = new FormattedText(classDiagram.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 12, Brushes.Black);
            _context.DrawText(formattedName, new Point((_diagramSize.Width - classDiagram.Name.GetWidth(12, Fonts.FontLight)) / 2, position += 15));
            position += 20;

            _context.DrawLine(new Pen(Brushes.Black, 1), new Point(1, position), new Point(_diagramSize.Width - 1, position));
            position += 10;
            return position;
        }

        private void RenderAllRowSections(ClassDiagram classDiagram, int position)
        {
            var renderLine = FollowingSectionsNotEmpty(classDiagram, "Fields");
            position = RenderRowSection(classDiagram.FieldRows, position, _diagramSize, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Properties");
            position = RenderRowSection(classDiagram.PropertyRows, position, _diagramSize, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Methods");
            position = RenderRowSection(classDiagram.MethodRows, position, _diagramSize, renderLine);

            RenderRowSection(classDiagram.EventRows, position, _diagramSize, false);
        }

        private bool FollowingSectionsNotEmpty(ClassDiagram classDiagram, string section)
        {
            var empty = true;

            switch (section)
            {
                case "Fields":
                    empty = classDiagram.PropertyRows.Count + classDiagram.MethodRows.Count +
                            classDiagram.EventRows.Count > 0;
                    break;
                case "Properties":
                    empty = classDiagram.MethodRows.Count + classDiagram.EventRows.Count > 0;
                    break;
                case "Methods":
                    empty = classDiagram.EventRows.Count > 0;
                    break;
            }

            return empty;
        }

        private int RenderRowSection(List<ClassDiagramRow> classDiagramRows, int position, Size size,
                                      bool renderLine)
        {
            var actualPosition = position;
            foreach (var row in classDiagramRows)
            {
                var formattedText = new FormattedText(row.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Fonts.FontLight, 12, Brushes.Black);
                _context.DrawImage(Icons.GetIcon(row.Type, row.Accessibility), new Rect(new Point(15, actualPosition), new Size(16, 16)));
                _context.DrawText(formattedText, new Point(40, actualPosition));
                actualPosition += 20;
            }

            if (classDiagramRows.Count > 0 && renderLine)
            {
                actualPosition = RenderLine(actualPosition, size);
            }

            return actualPosition;
        }

        private int RenderLine(int position, Size size)
        {
            _context.DrawLine(new Pen(Brushes.Black, 1), new Point(1, position), new Point(size.Width - 1, position));
            return position + 10;
        }

        private void RenderDiagramFrame(ClassDiagram classDiagram)
        {
            _context.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, 1), new Rect(1, 1, _diagramSize.Width - 2, _diagramSize.Height - 2));
            _context.Close();
        }
    }
}
