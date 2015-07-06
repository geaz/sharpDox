using SharpDox.UML.Class.Model;
using SharpDox.UML.Extensions;
using SharpDox.UML.Ressources;
using SharpDox.UML.SVG;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramSvgRenderer
    {
        private const double ACCESSIBILITY_LABEL_Y = 20.5d;
        private const double CLASSLABEL_Y = 35.5d;
        private const double FIRSTROW_OFFSET_Y = 65.5d;

        private SvgRoot _svgRoot;
        private SvgGraphic _svgGraphic;
        private ClassDiagram _classDiagram;
        private Size _diagramSize;

        public SvgRoot RenderDiagram(ClassDiagram classDiagram)
        {
            _classDiagram = classDiagram;
            _diagramSize = new Size(CalculateDiagramWidth(classDiagram), CalculateDiagramHeight(classDiagram));

            _svgRoot = new SvgRoot() { Width = _diagramSize.Width, Height = _diagramSize.Height };
            _svgGraphic = new SvgGraphic(_svgRoot);
            _svgRoot.Add(_svgGraphic);

            RenderFrame();
            RenderHeader();
            RenderAllRowSections();

            return _svgRoot;
        }

        public double CalculateDiagramWidth(ClassDiagram classDiagram)
        {
            var allRows = GetAllRows(classDiagram);
            var headerWidth = (int)Math.Max(classDiagram.Name.GetWidth(14, Fonts.FontLight), classDiagram.Accessibility.GetWidth(11, Fonts.FontItalic));
            var maxWidthRows = allRows.Count > 0 ? allRows.Max(o => (int)o.Text.GetWidth(14, Fonts.FontLight)) : 0;

            return Math.Max(headerWidth, maxWidthRows) + 65;
        }

        public double CalculateDiagramHeight(ClassDiagram classDiagram)
        {
            var allRows = GetAllRows(classDiagram);
            var notEmptySections = 
                (classDiagram.FieldRows.Count != 0 ? 1 : 0) +
                (classDiagram.PropertyRows.Count != 0 ? 1 : 0) +
                (classDiagram.MethodRows.Count != 0 || classDiagram.ConstructorRows.Count != 0 ? 1 : 0) +
                (classDiagram.EventRows.Count != 0 ? 1 : 0);

            var allRowsHeight = allRows.Count * 25 + (notEmptySections > 0 ? (notEmptySections - 1) * 10 : 0);                

            //Das Firstrowoffset gibt den y wert der unterkante der ersten row an
            //damit der wert korrekt ist muss eine row (ohne margin) von der berechnung abgezogen werden
            return (FIRSTROW_OFFSET_Y - 15) + allRowsHeight;
        }

        private List<ClassDiagramRow> GetAllRows(ClassDiagram classDiagram)
        {
            var allRows = new List<ClassDiagramRow>();
            allRows.AddRange(classDiagram.FieldRows);
            allRows.AddRange(classDiagram.PropertyRows);
            allRows.AddRange(classDiagram.MethodRows);
            allRows.AddRange(classDiagram.ConstructorRows);
            allRows.AddRange(classDiagram.EventRows);
            return allRows;
        }

        private void RenderFrame()
        {
            var rect = new SvgRectangle(_svgRoot, 0.5, 0.5, _diagramSize.Width, _diagramSize.Height);
            rect.StrokeWidth = 1;
            rect.Stroke = "#979797";
            rect.Fill = "#FFFFFF";
            _svgGraphic.Add(rect);
        }

        private void RenderHeader()
        {
            var accessibility = new SvgText(
                _svgRoot, 
                _classDiagram.Accessibility, 
                (_diagramSize.Width - _classDiagram.Accessibility.GetWidth(11, Fonts.FontItalic)) / 2,
                ACCESSIBILITY_LABEL_Y);
            accessibility.FontSize = 11;

            SvgElement name;
            if (_classDiagram.IsProjectStranger)
            {
                name = new SvgText(
                    _svgRoot,
                    _classDiagram.Name,
                    (_diagramSize.Width - _classDiagram.Name.GetWidth(14, Fonts.FontLight)) / 2,
                    CLASSLABEL_Y);
                ((SvgText)name).FontSize = 14;
            }
            else
            {
                name = new SvgLink(
                    _svgRoot,
                    _classDiagram.Name,
                    string.Format("{{{{type-link:{0}}}}}", _classDiagram.TypeIdentifier),
                    (_diagramSize.Width - _classDiagram.Name.GetWidth(14, Fonts.FontLight)) / 2,
                    CLASSLABEL_Y);
                ((SvgLink)name).Text.FontSize = 14;
            }

            var path = new SvgPath(
                _svgRoot, 
                string.Format("M0.5,{0}L{1},{0}", 
                    (CLASSLABEL_Y + 10).ToString("0.00", CultureInfo.InvariantCulture),
                    _diagramSize.Width.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";

            _svgGraphic.Add(accessibility);
            _svgGraphic.Add(name);
            _svgGraphic.Add(path);
        }

        private void RenderAllRowSections()
        {
            var rowCountOffset = 0;
            var sectionOffset = 0;

            RenderRowSection(_classDiagram.FieldRows, "field", rowCountOffset, sectionOffset);
            rowCountOffset += _classDiagram.FieldRows.Count;
            sectionOffset += _classDiagram.FieldRows.Count > 0 ? 1 : 0;

            RenderRowSection(_classDiagram.EventRows, "event", rowCountOffset, sectionOffset);
            rowCountOffset += _classDiagram.EventRows.Count;
            sectionOffset += _classDiagram.EventRows.Count > 0 ? 1 : 0;

            var rows = new List<ClassDiagramRow>();
            rows.AddRange(_classDiagram.MethodRows);
            rows.AddRange(_classDiagram.ConstructorRows);
            RenderRowSection(rows, "method", rowCountOffset, sectionOffset);
            rowCountOffset += rows.Count;
            sectionOffset += rows.Count > 0 ? 1 : 0;

            RenderRowSection(_classDiagram.PropertyRows, "property", rowCountOffset, sectionOffset);
        }

        private void RenderRowSection(List<ClassDiagramRow> classDiagramRows, string memberType, int rowCountOffset, int sectionOffset)
        {
            for (int i = 0; i < classDiagramRows.Count; i++)
            {
                var image = new SvgImage(
                    _svgRoot, 
                    15, 
                    FIRSTROW_OFFSET_Y + ((i + rowCountOffset) * 25) + (sectionOffset * 10) - 12, 
                    16, 
                    16,
                    string.Format("data:image/png;base64,{0}", Icons.GetBase64Icon(classDiagramRows[i].Type, classDiagramRows[i].Accessibility)));

                SvgElement text;
                if (_classDiagram.IsProjectStranger)
                {
                    text = new SvgText(
                        _svgRoot,
                        classDiagramRows[i].Text,
                        40,
                        FIRSTROW_OFFSET_Y + ((i + rowCountOffset) * 25) + (sectionOffset * 10));
                    ((SvgText)text).FontSize = 14;
                }
                else
                {
                    text = new SvgLink(
                        _svgRoot,
                        classDiagramRows[i].Text,
                        string.Format("{{{{{0}-link:{1}}}}}", memberType, classDiagramRows[i].Identifier),
                        40,
                        FIRSTROW_OFFSET_Y + ((i + rowCountOffset) * 25) + (sectionOffset * 10));
                    ((SvgLink)text).Text.FontSize = 14;
                }

                _svgGraphic.Add(image);
                _svgGraphic.Add(text);
            }

            if (classDiagramRows.Count > 0 && FollowingSectionsNotEmpty(memberType))
            {
                RenderLine(FIRSTROW_OFFSET_Y + ((classDiagramRows.Count + rowCountOffset) * 25) + (sectionOffset * 10) - 10);
            }
        }

        private void RenderLine(double position)
        {
            var path = new SvgPath(
                _svgRoot, 
                string.Format("M0.5,{0}L{1},{0}", 
                    position.ToString("0.00", CultureInfo.InvariantCulture),
                    _diagramSize.Width.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";
            _svgGraphic.Add(path);
        }

        private bool FollowingSectionsNotEmpty(string section)
        {
            var notEmpty = false;

            switch (section)
            {
                case "field":
                    notEmpty = (_classDiagram.PropertyRows.Count + _classDiagram.ConstructorRows.Count + _classDiagram.MethodRows.Count + _classDiagram.EventRows.Count) > 0;
                    break;
                case "event":
                    notEmpty = (_classDiagram.PropertyRows.Count + _classDiagram.ConstructorRows.Count + _classDiagram.MethodRows.Count) > 0;
                    break;
                case "method":
                    notEmpty = _classDiagram.PropertyRows.Count > 0;
                    break;
            }

            return notEmpty;
        }
    }
}
