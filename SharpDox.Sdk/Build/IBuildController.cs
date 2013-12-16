using SharpDox.Sdk.Config;

namespace SharpDox.Sdk.Build
{
    /// <default>
    ///     <summary>
    ///     This interface exposes all needed functionalities to
    ///     parse and build documentations.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Dieses Interface bildet alle Funktionen ab, die zum
    ///     Einlesen und Bauen der Dokumentation benötigt wird.
    ///     </summary>
    /// </de>
    public interface IBuildController
    {
        /// <default>
        ///     <summary>
        ///     Starts a structure parse for a given c# solution.
        ///     This method only parses minimal information about namespaces, types
        ///     and members. Just enough to show a tree of all members as seen in the
        ///     visibility settings of the sharpDox GUI.
        ///     </summary>
        ///     <param name="sharpDoxConfig">A config file with all necessary parameters</param>
        ///     <param name="thread">Indicates, if the process should start in an own thread.</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Startet das Einlesen der Struktur einer gegebenen C# Lösung.
        ///     Die Methode liest lediglich minimale Informationen über Namensräume, Typen und Mitglieder.
        ///     Gerade genug, um einen Baum aufzubauen wie er in den Sichtbarkeitseinstellungen der sharpDox GUI zu sehen ist.
        ///     </summary>
        ///     <param name="sharpDoxConfig">Eine Konfigurationsdatei mit allen notwendigen Einstellungen</param>
        ///     <param name="thread">Gibt an, ob der Vorgang in einem eigenen Thread gestartet werden soll</param>
        /// </de>
        void StartParse(SharpDoxConfig sharpDoxConfig, bool thread);

        /// <default>
        ///     <summary>
        ///     Starts a documentation build for a given c# solution.
        ///     The method parses the solution and starts all registered exporters.
        ///     </summary>
        ///     <param name="sharpDoxConfig">A config file with all necessary parameters</param>
        ///     <param name="thread">Indicates, if the process should start in an own thread.</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Startet den Bau der Dokumentation einer gegebenen C# Lösung.
        ///     Die Methode liest die Lösung ein und startet alle registrierten 
        ///     </summary>
        ///     <param name="sharpDoxConfig">Eine Konfigurationsdatei mit allen notwendigen Einstellungen</param>
        ///     <param name="thread">Gibt an, ob der Vorgang in einem eigenen Thread gestartet werden soll</param>
        /// </de>
        void StartBuild(SharpDoxConfig sharpDoxConfig, bool thread);

        /// <default>
        ///     <summary>
        ///     Stops any current running parse or build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Stoppt den aktuell laufenden Einlese- oder Bau-Prozess.
        ///     </summary>
        /// </de>
        void Stop();
    }
}