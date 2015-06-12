using SharpDox.Model.Repository.Members;
using SharpDox.Model.Repository;
using SharpDox.UML.Class;
using SharpDox.UML.Sequence;

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
    public static class DiagramExtensions
    {
        /// <default>
        ///     <summary>
        ///     Creates the class diagram for the given <c>SDType</c>.
        ///     </summary>
        ///     <param name="type">Create the class diagram for this <c>SDType</c>.</param>
        ///     <returns>The class diagram for the given <c>SDType</c>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Erstellt das Klassendiagramm für den gegebenen <c>SDType</c>.
        ///     </summary>
        ///     <param name="type">Erstellt das Klassendiagramm für diesen <c>SDType</c>.</param>
        ///     <returns>Das Klassendiagramm für den <c>SDType</c>.</returns>
        /// </de>
        public static ISDDiagram GetClassDiagram(this SDType type)
        {
            var classDiagramParser = new ClassDiagramParser();
            return classDiagramParser.CreateClassDiagram(type);
        }

        /// <default>
        ///     <summary>
        ///     Creates the sequence diagram for the given <c>SDMethod</c>.
        ///     </summary>
        ///     <param name="method">Create the sequence diagram for this <c>SDMethod</c>.</param>
        ///     <param name="sdProject">The method needs the complete parsed sharpDox project.</param>
        ///     <returns>The sequence diagram for the given <c>SDMethod</c>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Erstellt das Sequenzdiagramm für die gegebene <c>SDMethod</c>.
        ///     </summary>
        ///     <param name="method">Erstellt das Sequenzdiagramm für dies <c>SDMethod</c>.</param>
        ///     <param name="sdRepository">Die Methode benötigt das eingelesene Repository.</param>
        ///     <returns>Das Sequenzdiagramm für die <c>SDMethod</c>.</returns>
        /// </de>
        public static ISDDiagram GetSequenceDiagram(this SDMethod method, SDRepository sdRepository)
        {
            var sequenceDiagramParser = new SequenceDiagramParser(method, sdRepository);
            return sequenceDiagramParser.CreateSequenceDiagram();
        }

        /// <default>
        ///     <summary>
        ///     With this method you are able to check, if the class diagram, for the 
        ///     given <c>SDType</c>, is empty.
        ///     </summary>
        ///     <param name="type">Check for this <c>SDType</c>.</param>
        ///     <returns>True, if it is empty. False, otherwise.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Mit dieser Method kann überprüft werden, ob das Klassdiagramm , für den
        ///     gegebenen <c>SDType</c>, leer ist.
        ///     </summary>
        ///     <param name="type">Überprüfe diesen <c>SDType</c>.</param>
        ///     <returns>True, falls leer. False, ansonsten.</returns>
        /// </de>
        public static bool IsClassDiagramEmpty(this SDType type)
        {
            return type.Events.Count == 0 && type.Constructors.Count == 0 && type.Methods.Count == 0 &&
                   type.Properties.Count == 0 && type.Fields.Count == 0;
        }

        /// <default>
        ///     <summary>
        ///     With this method you are able to check, if the sequence diagram, for the 
        ///     given <c>SDMethod</c>, is empty.
        ///     </summary>
        ///     <param name="method">Check for this <c>SDMethod</c>.</param>
        ///     <returns>True, if it is empty. False, otherwise.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Mit dieser Method kann überprüft werden, ob das Sequenzdiagramm , für den
        ///     gegebenen <c>SDMethod</c>, leer ist.
        ///     </summary>
        ///     <param name="method">Überprüfe diesen <c>SDMethod</c>.</param>
        ///     <returns>True, falls leer. False, ansonsten.</returns>
        /// </de>
        public static bool IsSequenceDiagramEmpty(this SDMethod method)
        {
            return new SequenceDiagramParser(method).IsSequenceDiagramEmpty();
        }
    }
}
