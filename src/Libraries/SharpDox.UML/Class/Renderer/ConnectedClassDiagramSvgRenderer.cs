using SharpDox.UML.Class.Model;
using SharpDox.UML.SVG;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace SharpDox.UML.Class.Renderer
{
    internal class ConnectedClassDiagramSvgRenderer
    {
        private ClassDiagram _classDiagram;
        private SvgRoot _mainDiagram;
        private double _leftMargin, _topMargin;
                
        private readonly ClassDiagramSvgRenderer _classDiagramSvgRenderer = new ClassDiagramSvgRenderer();

        public SvgRoot RenderConnectedDiagram(ClassDiagram classDiagram)
        {
            _classDiagram = classDiagram;
            _mainDiagram = _classDiagramSvgRenderer.RenderDiagram(_classDiagram);

            CalculateMainMarginsAndSize();
            PositionMainDiagram();
            DrawBaseTypes();
            DrawImplementedInterfaces();
            DrawUsedBy();
            DrawUses();

            return _mainDiagram;
        }

        private void CalculateMainMarginsAndSize()
        {
            _leftMargin = _classDiagram.UsedBy.Sum(u => _classDiagramSvgRenderer.CalculateDiagramWidth(u)) + _classDiagram.UsedBy.Count * 50;
            _topMargin = 50 + Math.Max(
                _classDiagram.BaseTypes.Count > 0 ? _classDiagram.BaseTypes.Max(o => _classDiagramSvgRenderer.CalculateDiagramHeight(o)) : 0,
                _classDiagram.ImplementedInterfaces.Count > 0 ? _classDiagram.ImplementedInterfaces.Max(o => _classDiagramSvgRenderer.CalculateDiagramHeight(o)) : 0);

            var width = _leftMargin + _mainDiagram.Width + _classDiagram.Uses.Sum(u => _classDiagramSvgRenderer.CalculateDiagramWidth(u)) + _classDiagram.Uses.Count * 50;

            var maxUsedBy = _classDiagram.UsedBy.Any() ? _classDiagram.UsedBy.Max(u => _classDiagramSvgRenderer.CalculateDiagramHeight(u)) : 0.0;
            var maxUses = _classDiagram.Uses.Any() ? _classDiagram.Uses.Max(u => _classDiagramSvgRenderer.CalculateDiagramHeight(u)) : 0.0; 
            var height = _topMargin + Math.Max(_mainDiagram.Height, Math.Max(maxUsedBy, maxUses));

            _mainDiagram.Width = width;
            _mainDiagram.Height = height;
        }

        private void PositionMainDiagram()
        {
            var translate = _mainDiagram.CreateAttribute("transform");
            translate.Value = string.Format("translate({0}, {1})", _leftMargin.ToString("0.00", CultureInfo.InvariantCulture), _topMargin.ToString("0.00", CultureInfo.InvariantCulture));
            _mainDiagram["svg"]["g"].Attributes.Append(translate);
        }

        private void DrawBaseTypes()
        {
            var offset = _leftMargin;            
            for (int i = 0; i < _classDiagram.BaseTypes.Count; i++)
            {
                var diagram = _classDiagramSvgRenderer.RenderDiagram(_classDiagram.BaseTypes[i]);
                var diagramSize = new Size(
                    _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram.BaseTypes[i]), 
                    _classDiagramSvgRenderer.CalculateDiagramHeight(_classDiagram.BaseTypes[i]));

                var positionX = (i * 50) + offset;
                var positionY = 0;

                var translate = diagram.CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                diagram["svg"]["g"].Attributes.Append(translate);

                _mainDiagram.ImportAdd((SvgElement)diagram["svg"]["g"]);

                offset += diagramSize.Width;

                var pathGraphic = new SvgGraphic(_mainDiagram);

                //line
                if (i == 0)
                {
                    var path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{0},{2}",
                        (positionX + 50).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY + diagramSize.Height).ToString("0.00", CultureInfo.InvariantCulture),
                        _topMargin.ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    pathGraphic.Add(path);
                }

                //arrow
                var startX = positionX + 50;
                var startY = diagramSize.Height;

                var points = string.Format("{0},{1} {2},{3} {4},{3}",
                    startX.ToString("0.00", CultureInfo.InvariantCulture),
                    startY.ToString("0.00", CultureInfo.InvariantCulture),
                    (startX - 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (startY + 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (startX + 5).ToString("0.00", CultureInfo.InvariantCulture));
                var arrow = new SvgPolygon(_mainDiagram, points);
                arrow.Stroke = "#979797";
                arrow.Fill = "#FFFFFF";

                pathGraphic.Add(arrow);

                _mainDiagram.Add(pathGraphic);
            }
        }
        
        private void DrawImplementedInterfaces()
        {
            var offset = _leftMargin + _classDiagram.BaseTypes.Sum(b => _classDiagramSvgRenderer.CalculateDiagramWidth(b) + 25);
            for (int i = 0; i < _classDiagram.ImplementedInterfaces.Count; i++)
            {
                var diagram = _classDiagramSvgRenderer.RenderDiagram(_classDiagram.ImplementedInterfaces[i]);
                var diagramSize = new Size(
                    _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram.ImplementedInterfaces[i]),
                    _classDiagramSvgRenderer.CalculateDiagramHeight(_classDiagram.ImplementedInterfaces[i]));

                var positionX = (i * 50) + offset;
                var positionY = 0;

                var translate = diagram.CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                diagram["svg"]["g"].Attributes.Append(translate);

                _mainDiagram.ImportAdd((SvgElement)diagram["svg"]["g"]);

                var pathGraphic = new SvgGraphic(_mainDiagram);
                
                var path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{0},{2}",
                        (positionX + 50).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY + diagramSize.Height).ToString("0.00", CultureInfo.InvariantCulture),
                        (_topMargin - 12.5).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                path.StrokeDashArray = "5, 5";
                pathGraphic.Add(path);

                var startX = positionX + 50;
                var startY = diagramSize.Height;
                var points = string.Format("{0},{1} {2},{3} {4},{3}",
                    startX.ToString("0.00", CultureInfo.InvariantCulture),
                    startY.ToString("0.00", CultureInfo.InvariantCulture),
                    (startX - 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (startY + 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (startX + 5).ToString("0.00", CultureInfo.InvariantCulture));
                var arrow = new SvgPolygon(_mainDiagram, points);
                arrow.Stroke = "#979797";
                arrow.Fill = "#FFFFFF";
                pathGraphic.Add(arrow);

                if (i == _classDiagram.ImplementedInterfaces.Count - 1)
                {
                    var mainDiagramWidth = _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram);

                    path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{2},{1}",
                        (positionX + 50).ToString("0.00", CultureInfo.InvariantCulture),
                        (_topMargin - 12.5).ToString("0.00", CultureInfo.InvariantCulture),
                        (_leftMargin + (mainDiagramWidth - 50)).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    path.StrokeDashArray = "5, 5";
                    pathGraphic.Add(path);

                    path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{0},{2}",
                        (_leftMargin + (mainDiagramWidth - 50)).ToString("0.00", CultureInfo.InvariantCulture),
                        (_topMargin - 12.5).ToString("0.00", CultureInfo.InvariantCulture),
                        _topMargin.ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    path.StrokeDashArray = "5, 5";
                    pathGraphic.Add(path);
                }
                                
                _mainDiagram.Add(pathGraphic);

                offset += diagramSize.Width;
            }
        }

        private void DrawUsedBy()
        {
            var offset = 0d;
            for (int i = 0; i < _classDiagram.UsedBy.Count; i++)
            {
                var diagram = _classDiagramSvgRenderer.RenderDiagram(_classDiagram.UsedBy[i]);
                var diagramSize = new Size(
                    _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram.UsedBy[i]),
                    _classDiagramSvgRenderer.CalculateDiagramHeight(_classDiagram.UsedBy[i]));

                var positionX = (i * 50) + offset;
                var positionY = _topMargin + 50;

                var translate = diagram.CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                diagram["svg"]["g"].Attributes.Append(translate);

                _mainDiagram.ImportAdd((SvgElement)diagram["svg"]["g"]);

                var pathGraphic = new SvgGraphic(_mainDiagram);

                var path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{0},{2}",
                    (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 25).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";

                pathGraphic.Add(path);
                _mainDiagram.Add(pathGraphic);

                if (i == 0)
                {
                    pathGraphic = new SvgGraphic(_mainDiagram);
                    path = new SvgPath(_mainDiagram, string.Format("M{0},{2}L{1},{2}",
                        (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                        _leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.Add(path);

                    path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{2},{3}",
                        (_leftMargin - 10).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 30.5d).ToString("0.00", CultureInfo.InvariantCulture),
                        _leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.Add(path);

                    path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{2},{3}",
                        (_leftMargin - 10).ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 20.5d).ToString("0.00", CultureInfo.InvariantCulture),
                        _leftMargin.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";

                    pathGraphic.Add(path);

                    _mainDiagram.Add(pathGraphic);
                }

                offset += diagramSize.Width;
            }
        }

        private void DrawUses()
        {
            var leftOffset = _leftMargin + _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram);
            var offset = leftOffset;
            for (int i = 0; i < _classDiagram.Uses.Count; i++)
            {
                var diagram = _classDiagramSvgRenderer.RenderDiagram(_classDiagram.Uses[i]);
                var diagramSize = new Size(
                    _classDiagramSvgRenderer.CalculateDiagramWidth(_classDiagram.Uses[i]),
                    _classDiagramSvgRenderer.CalculateDiagramHeight(_classDiagram.Uses[i]));

                var positionX = (i * 50) + offset + 50;
                var positionY = _topMargin + 50;

                var translate = diagram.CreateAttribute("transform");
                translate.Value = string.Format("translate({0}, {1})", positionX.ToString("0.00", CultureInfo.InvariantCulture), positionY.ToString("0.00", CultureInfo.InvariantCulture));
                diagram["svg"]["g"].Attributes.Append(translate);

                _mainDiagram.ImportAdd((SvgElement)diagram["svg"]["g"]);

                var pathGraphic = new SvgGraphic(_mainDiagram);

                var path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{0},{2}",
                    (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 25).ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.Add(path);

                path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{2},{3}",
                    (positionX + (diagramSize.Width / 2) - 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.Add(path);

                path = new SvgPath(_mainDiagram, string.Format("M{0},{1}L{2},{3}",
                    (positionX + (diagramSize.Width / 2) + 5).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionY - 10).ToString("0.00", CultureInfo.InvariantCulture),
                    (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                    positionY.ToString("0.00", CultureInfo.InvariantCulture)));
                path.StrokeWidth = 1;
                path.Stroke = "#979797";
                pathGraphic.Add(path);

                if (i == _classDiagram.Uses.Count - 1)
                {
                    path = new SvgPath(_mainDiagram, string.Format("M{0},{2}L{1},{2}",
                        (positionX + (diagramSize.Width / 2)).ToString("0.00", CultureInfo.InvariantCulture),
                        leftOffset.ToString("0.00", CultureInfo.InvariantCulture),
                        (positionY - 25.5d).ToString("0.00", CultureInfo.InvariantCulture)));
                    path.StrokeWidth = 1;
                    path.Stroke = "#979797";
                    pathGraphic.Add(path);
                }

                _mainDiagram.Add(pathGraphic);
                offset += diagramSize.Width;
            }
        }
    }
}
