using System.Drawing;
using SharpDox.Model.Documentation;
namespace SharpDox.UML
{
    /// <default>
    ///     <summary>
    ///     Extensions for <c>SDType</c>s and <c>SDMethod</c>s to create UML-Diagrams.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Erweiterungen für <c>SDType</c>s und <c>SDMethod</c>s um UML-Diagramme zu erstellen.
    ///     </summary>
    /// </de>
    public interface ISDDiagram
    {
        /// <default>
        ///     <summary>
        ///     Saves a png of the diagram to the given path.
        ///     </summary>
        ///     <param name="outputFilepath">Save the diagram to this path.</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Diese Methode speichert ein PNG des Diagramms in den angegebenen Pfad.
        ///     </summary>
        ///     <param name="outputFilepath">Der Pfad in den Diagramm gespeichert werden soll.</param>
        /// </de>
        void ToPng(string outputFilepath);
        
        /// <default>
        ///     <summary>
        ///     This method returns a svg representation of this diagram.
        ///     </summary>
        ///     <returns>The svg representation as <c>SDTemplate</c></returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Diese Methode erstellt eine SVG-Repräsentation des Diagramms.
        ///     </summary>
        ///     <returns>Die SVG-Repräsentation als <c>SDTemplate</c></returns>
        /// </de>
        SDTemplate ToSvg();

        /// <default>
        ///     <summary>
        ///     This method returns the size of the svg diagram.
        ///     </summary>
        ///     <returns>The svg size.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Diese Methode liefert die Größe des Diagrams zurück. 
        ///     </summary>
        ///     <returns>Die SVG Größe.</returns>
        /// </de>
        Size GetSvgSize();
    }
}
