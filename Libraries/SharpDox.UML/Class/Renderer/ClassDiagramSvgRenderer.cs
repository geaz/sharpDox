using SharpDox.UML.Class.Model;
using SharpDox.UML.Extensions;
using SharpDox.UML.Ressources;
using SharpDox.UML.SVG;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramSvgRenderer
    {
        public SvgRoot RenderDiagram(ClassDiagram classDiagram)
        {
            var diagramWidth = CalculateDiagramWidth(classDiagram);
            var svgRoot = new SvgRoot(100, diagramWidth);

            var rect = new SvgRectangle(svgRoot, 0.5, 0.5, diagramWidth - 1, 100);
            rect.StrokeWidth = 1;
            rect.Stroke = "#979797";
            rect.Fill = "#FFFFFF";
            svgRoot.AppendChild(rect.XmlElement);

            double position = 20.5d;
            position = RenderHeader(classDiagram, svgRoot, position);
            position = RenderAllRowSections(svgRoot, classDiagram, position);

            rect.Height = position - 9.5;
            svgRoot.Height = position - 5;

            RenderAllConnectedDiagrams(svgRoot, classDiagram);

            return svgRoot;
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

        private double RenderHeader(ClassDiagram classDiagram, SvgRoot svgRoot, double position)
        {
            var accessibility = new SvgText(svgRoot, classDiagram.Accessibility, (svgRoot.Width - classDiagram.Accessibility.GetWidth(11, Fonts.FontItalic)) / 2, position);
            accessibility.FontSize = 11;

            var name = new SvgLink(svgRoot, classDiagram.Name, string.Format("{{{{type-link:{0}}}}}", classDiagram.TypeIdentifier), (svgRoot.Width - classDiagram.Name.GetWidth(14, Fonts.FontLight)) / 2, position += 20);
            name.Text.FontSize = 14;

            position += 15;

            var path = new SvgPath(svgRoot, string.Format("M0.5,{0}L{1},{0}", position.ToString("0.00", CultureInfo.InvariantCulture), svgRoot.Width.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";

            svgRoot.AppendChild(accessibility.XmlElement);
            svgRoot.AppendChild(name.XmlElement);
            svgRoot.AppendChild(path.XmlElement);

            return position + 25;
        }

        private double RenderAllRowSections(SvgRoot svgRoot, ClassDiagram classDiagram, double position)
        {
            var renderLine = FollowingSectionsNotEmpty(classDiagram, "Fields");
            position = RenderRowSection(svgRoot, classDiagram.FieldRows, "field", position, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Events");
            position = RenderRowSection(svgRoot, classDiagram.EventRows, "event", position, renderLine);

            renderLine = FollowingSectionsNotEmpty(classDiagram, "Methods");
            position = RenderRowSection(svgRoot, classDiagram.ConstructorRows, "constructor", position, false);
            position = RenderRowSection(svgRoot, classDiagram.MethodRows, "method", position, renderLine);

            position = RenderRowSection(svgRoot, classDiagram.PropertyRows, "property", position, false);

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
                case "Events":
                    empty = classDiagram.MethodRows.Count + classDiagram.EventRows.Count > 0;
                    break;
                case "Methods":
                    empty = classDiagram.PropertyRows.Count > 0;
                    break;
            }

            return empty;
        }

        private double RenderRowSection(SvgRoot svgRoot, List<ClassDiagramRow> classDiagramRows, string memberType, double position, bool renderLine)
        {
            var actualPosition = position;
            foreach (var row in classDiagramRows)
            {
                var image = new SvgImage(svgRoot, 15, actualPosition - 12, 16, 16, string.Format("data:image/png;base64,{0}", Icons.GetBase64Icon(row.Type, row.Accessibility)));
                var text = new SvgLink(svgRoot, row.Text, string.Format("{{{{{0}-link:{1}}}}}", memberType, row.Identifier), 40, actualPosition);
                text.Text.FontSize = 14;

                svgRoot.AppendChild(image.XmlElement);
                svgRoot.AppendChild(text.XmlElement);

                actualPosition += 25;
            }

            if (classDiagramRows.Count > 0 && renderLine)
            {
                actualPosition -= 10;
                actualPosition = RenderLine(svgRoot, actualPosition);
            }

            return actualPosition;
        }

        private double RenderLine(SvgRoot svgRoot, double position)
        {
            var path = new SvgPath(svgRoot, string.Format("M0.5,{0}L{1},{0}", position.ToString("0.00", CultureInfo.InvariantCulture), svgRoot.Width.ToString("0.00", CultureInfo.InvariantCulture)));
            path.StrokeWidth = 1;
            path.Stroke = "#979797";
            svgRoot.AppendChild(path.XmlElement);

            return position + 20;
        }

        private List<SvgRoot> GetSvgs(List<ClassDiagram> diagrams)
        {
            var svgs = new List<SvgRoot>();
            foreach (var diagram in diagrams)
            {
                svgs.Add(RenderDiagram(diagram));
            }

            return svgs;
        }

        private void RenderAllConnectedDiagrams(SvgRoot svgRoot, ClassDiagram classDiagram)
        {
            var uses = GetSvgs(classDiagram.Uses);
            var usedBys = GetSvgs(classDiagram.UsedBy);
            var implementedInterfaces = GetSvgs(classDiagram.ImplementedInterfaces);
            var baseTypes = GetSvgs(classDiagram.BaseTypes);

            var mainDiagramWidth = svgRoot.Width;
            var leftMargin = usedBys.Sum(u => u.Width) + usedBys.Count * 50;
            var topMargin = 50 + Math.Max(
                baseTypes.Count > 0 ? baseTypes.Max(o => o.Height) : 0, 
                implementedInterfaces.Count > 0 ? implementedInterfaces.Max(o => o.Height) : 0);

            var translate = svgRoot.CreateAttribute("transform");
            translate.Value = string.Format("translate({0}, {1})", leftMargin.ToString("0.00", CultureInfo.InvariantCulture), topMargin.ToString("0.00", CultureInfo.InvariantCulture));
            svgRoot.GraphicsElement.Attributes.Append(translate);

            DrawBaseTypes(svgRoot, baseTypes, topMargin, leftMargin);
            DrawImplementedInterfaces(svgRoot, implementedInterfaces, leftMargin + baseTypes.Sum(b => b.Width) + 50 * baseTypes.Count);
            DrawUsedBys(svgRoot, usedBys, topMargin, leftMargin);
            DrawUses(svgRoot, uses, topMargin, leftMargin + mainDiagramWidth);
        }

        private void DrawBaseTypes(SvgRoot svgRoot, List<SvgRoot> baseTypes, double topMargin, double leftMargin)
        {
            var offset = leftMargin;
            for (int i = 0; i < baseTypes.Count; i++)
            {
                var positionX = (i * 50) + offset;
                var positionY = 3;

                var translate = baseTypes[i].CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                baseTypes[i].GraphicsElement.Attributes.Append(translate);

                svgRoot.ImportAppendToRoot(baseTypes[i].GraphicsElement);

                offset += baseTypes[i].Width;

                var pathGraphic = new SvgRoot(50, leftMargin);

                //line
                if(i == 0)
                {
                    var path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{0},{2}",
                        (positionX + 50).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY + baseTypes[i].Height).ToString("0.00", CultureInfo.InvariantCulture),
                        topMargin.ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    pathGraphic.AppendChild(path.XmlElement);
                }

                //arrow
                var startX = positionX + 50;
                var startY = baseTypes[i].Height;

                var points = string.Format("{0},{1} {2},{3} {4},{3}",
                    startX.ToString("0.00", CultureInfo.InvariantCulture),
                    startY.ToString("0.00", CultureInfo.InvariantCulture),
                    (startX - 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (startY + 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (startX + 5).ToString("0.00", CultureInfo.InvariantCulture));
                var arrow = new SvgPolygon(pathGraphic, points);
                arrow.Stroke = "#979797";
                arrow.Fill = "#FFFFFF";

                pathGraphic.AppendChild(arrow.XmlElement);

                svgRoot.ImportAppendToRoot(pathGraphic.GraphicsElement);
            }
        }

        private void DrawImplementedInterfaces(SvgRoot svgRoot, List<SvgRoot> implementedInterfaces, double leftMargin)
        {
            var offset = leftMargin;
            for (int i = 0; i < implementedInterfaces.Count; i++)
            {
                var positionX = (i * 50) + offset;
                var positionY = 0;

                var translate = implementedInterfaces[i].CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                implementedInterfaces[i].GraphicsElement.Attributes.Append(translate);

                svgRoot.ImportAppendToRoot(implementedInterfaces[i].GraphicsElement);

                offset += implementedInterfaces[i].Width;
            }
        }

        private void DrawUsedBys(SvgRoot svgRoot, List<SvgRoot> usedBys, double topMargin, double leftMargin)
        {
            var offset = 0d;
            for (int i = 0; i < usedBys.Count; i++)
            {
                var positionX = (i * 50) + offset;
                var positionY = topMargin + 50;

                var translate = usedBys[i].CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                usedBys[i].GraphicsElement.Attributes.Append(translate);

                svgRoot.ImportAppendToRoot(usedBys[i].GraphicsElement);

                var pathGraphic = new SvgRoot(50, leftMargin);
                var path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{0},{2}",
                    (positionX + (usedBys[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 25).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.AppendChild(path.XmlElement);
                svgRoot.ImportAppendToRoot(pathGraphic.GraphicsElement);

                if (i == 0)
                {
                    pathGraphic = new SvgRoot(50, leftMargin);
                    path = new SvgPath(pathGraphic, string.Format("M{0},{2}L{1},{2}",
                        (positionX + (usedBys[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                        leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.AppendChild(path.XmlElement);

                    path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{2},{3}",
                        (leftMargin - 10).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 30.5d).ToString("0.00", CultureInfo.InvariantCulture),
                        leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.AppendChild(path.XmlElement);

                    path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{2},{3}",
                        (leftMargin - 10).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 20.5d).ToString("0.00", CultureInfo.InvariantCulture),
                        leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.AppendChild(path.XmlElement);

                    svgRoot.ImportAppendToRoot(pathGraphic.GraphicsElement);
                }

                offset += usedBys[i].Width;
            }
        }

        private void DrawUses(SvgRoot svgRoot, List<SvgRoot> uses, double topMargin, double leftOffset)
        {
            var offset = leftOffset;
            for (int i = 0; i < uses.Count; i++)
            {
                var positionX = (i * 50) + offset + 50;
                var positionY = topMargin + 50;

                var translate = uses[i].CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                uses[i].GraphicsElement.Attributes.Append(translate);

                svgRoot.ImportAppendToRoot(uses[i].GraphicsElement);

                var pathGraphic = new SvgRoot(50, leftOffset);

                var path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{0},{2}",
                    (positionX + (uses[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 25).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.AppendChild(path.XmlElement);

                path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{2},{3}",
                    (positionX + (uses[i].Width / 2) - 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionX + (uses[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.AppendChild(path.XmlElement);

                path = new SvgPath(pathGraphic, string.Format("M{0},{1}L{2},{3}",
                    (positionX + (uses[i].Width / 2) + 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionX + (uses[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.AppendChild(path.XmlElement);

                if (i == uses.Count - 1)
                {
                    path = new SvgPath(pathGraphic, string.Format("M{0},{2}L{1},{2}",
                        (positionX + (uses[i].Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                        leftOffset.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    pathGraphic.AppendChild(path.XmlElement);
                }

                svgRoot.ImportAppendToRoot(pathGraphic.GraphicsElement);
                offset += uses[i].Width;
            }
        }
    }
}
