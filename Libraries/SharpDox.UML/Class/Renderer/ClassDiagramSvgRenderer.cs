using SharpDox.UML.Class.Model;
using SharpDox.UML.SVG;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramSvgRenderer
    {
        private SvgRoot _svgRoot;
        private double _diagramWidth;

        public SvgRoot RenderDiagram(ClassDiagram classDiagram)
        {
            _diagramWidth = CalculateDiagramWidth(classDiagram);
            _svgRoot = new SvgRoot(100, _diagramWidth);

            var rect = new SvgRectangle(_svgRoot, 0.5, 0.5, _diagramWidth - 1, 100);
            rect.StrokeWidth = 1;
            rect.Stroke = "#979797";
            rect.Fill = "#FFFFFF";
            _svgRoot.AppendChild(rect.XmlElement);

            double position = 20.5d;
            position = RenderHeader(classDiagram, position);
            position = RenderAllRowSections(classDiagram, position);

            rect.Height = position - 9.5;
            _svgRoot.Height = position - 5;            
            return _svgRoot;
        }

        private double CalculateDiagramWidth(ClassDiagram classDiagram)
        {
            var allRows = GetAllRows(classDiagram);
            var headerWidth = (int)Math.Max(classDiagram.Name.GetWidth(14, Fonts.FontLight), classDiagram.Accessibility.GetWidth(11, Fonts.FontItalic));
            var maxWidthRows = allRows.Count > 0 ? allRows.Max(o => (int)o.Text.GetWidth(14, Fonts.FontLight)) : 0;

            return Math.Max(headerWidth, maxWidthRows) + 65;
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

        private double RenderHeader(ClassDiagram classDiagram, double position)
        {
            var accessibility = new SvgText(_svgRoot, classDiagram.Accessibility, (_diagramWidth - classDiagram.Accessibility.GetWidth(11, Fonts.FontItalic)) / 2, position);
            accessibility.FontSize = 11;

            var name = new SvgLink(_svgRoot, classDiagram.Name, string.Format("{{{{type-link:{0}}}}}", classDiagram.TypeIdentifier), (_diagramWidth - classDiagram.Name.GetWidth(14, Fonts.FontLight)) / 2, position += 20);
            name.Text.FontSize = 14;

            position += 15;

            var path = new SvgPath(_svgRoot, string.Format("M0.5,{0}L{1},{0}", position.ToString("0.00", CultureInfo.InvariantCulture), _diagramWidth.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";

            _svgRoot.AppendChild(accessibility.XmlElement);
            _svgRoot.AppendChild(name.XmlElement);
            _svgRoot.AppendChild(path.XmlElement);

            return position + 25;
        }

        private double RenderAllRowSections(ClassDiagram classDiagram, double position)
        {
            var renderLine = FollowingSectionsNotEmpty(classDiagram, "Fields");
            position = RenderRowSection(classDiagram.FieldRows, "field", position, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Properties");
            position = RenderRowSection(classDiagram.PropertyRows, "property", position, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Methods");
            position = RenderRowSection(classDiagram.MethodRows, "method", position, renderLine);

            position = RenderRowSection(classDiagram.EventRows, "event", position, false);

            return position;
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

        private double RenderRowSection(List<ClassDiagramRow> classDiagramRows, string memberType, double position, bool renderLine)
        {
            var actualPosition = position;
            foreach (var row in classDiagramRows)
            {
                var image = new SvgImage(_svgRoot, 15, actualPosition - 12, 16, 16, string.Format("data:image/png;base64,{0}", Icons.GetBase64Icon(row.Type, row.Accessibility)));
                var text = new SvgLink(_svgRoot, row.Text, string.Format("{{{{{0}-link:{1}}}}}", memberType, row.Identifier), 40, actualPosition);
                text.Text.FontSize = 14;

                _svgRoot.AppendChild(image.XmlElement);
                _svgRoot.AppendChild(text.XmlElement);

                actualPosition += 25;
            }

            if (classDiagramRows.Count > 0 && renderLine)
            {
                actualPosition -= 10;
                actualPosition = RenderLine(actualPosition);
            }

            return actualPosition;
        }

        private double RenderLine(double position)
        {
            var path = new SvgPath(_svgRoot, string.Format("M0.5,{0}L{1},{0}", position.ToString("0.00", CultureInfo.InvariantCulture), _diagramWidth.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";
            _svgRoot.AppendChild(path.XmlElement);

            return position + 20;
        }
    }
}
