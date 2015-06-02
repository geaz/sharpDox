using System;
using System.Collections.Generic;

namespace SharpDox.Sdk.Config
{
    /// <default>
    ///     <summary>
    ///     This interface exposes all needed functionalities to
    ///     work with config files.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Dieses Interface bildet alle Funktionen ab, die zur
    ///     Arbeit mit Konfigurationsdateien notwendig sind.
    ///     </summary>
    /// </de>
    public interface IConfigController
    {
        /// <default>
        ///     <summary>
        ///     Fires if a new project got added to the recent project collection.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Feuert, falls ein neues Projekt zu der List der zuletzt benutzten Projekte hinzugefügt wurde.
        ///     </summary>
        /// </de>
        event Action OnRecentProjectsChanged;

        /// <default>
        ///     <summary>
        ///     Creates a new config file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Erstellt eine neue Konfigurationsdatei.
        ///     </summary>
        /// </de>
        void New();
        
        /// <default>
        ///     <summary>
        ///     Load a config file from the given filepath.
        ///     </summary>
        ///     <param name="fileToLoad">The path of the config file</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Lädt eine Konfigurationsdatei im angegebenen Dateipfad.
        ///     </summary>
        ///     <param name="fileToLoad">Der Pfad zur Konfigurationsdatei</param>
        /// </de>
        void Load(string fileToLoad);

        /// <default>
        ///     <summary>
        ///     Saves the current config file, if the <c>PathToConfig</c> property of the
        ///     current config file is not null or empty.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Speichert die aktuelle Konfigurationsdatei, falls die <c>PathToConfig</c>
        ///     Eigenschaft der aktuellen Konfigurationsdatei nicht null oder leer ist.
        ///     </summary>
        /// </de>
        void Save();

        /// <default>
        ///     <summary>
        ///     Saves the current config file to the given path.
        ///     </summary>
        ///     <param name="fileToSave">The path for the config file</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Speichert die aktuelle Konfigurationsdatei in den angegebenen Pfad.
        ///     </summary>
        ///     <param name="fileToSave">Der Pfad für die Konfigurationsdatei</param>
        /// </de>
        void SaveTo(string fileToSave);

        /// <default>
        ///     <summary>
        ///     Gets all config sections of the current config file.
        ///     </summary>
        ///     <returns>Returns all config sections.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Holz alle Konfigurationssektionen der aktuellenKonfigurationsdatei.
        ///     </summary>
        ///     <returns>Gibt alle Konfigurationssektionen zurück.</returns>
        /// </de>
        IEnumerable<IConfigSection> GetAllConfigSections(); 
            
        /// <default>
        ///     <summary>
        ///     Gets a specific config section of the current config file.
        ///     </summary>
        ///     <example>
        ///     <code>
        ///         <![CDATA[
        ///         var config = _configController.GetConfigSection<SharpDoxConfig>();
        ///         ]]>
        ///     </code>
        ///     </example>
        ///     <typeparam name="T">The type of the config section</typeparam>
        ///     <returns>Returns the config section, if it exists. Otherwise <c>null</c>.</returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Holt eine spezifische Konfigurationssektion der aktuellen Konfigurationsdatei.
        ///     </summary>
        ///     <example>
        ///     <code>
        ///         <![CDATA[
        ///         var config = _configController.GetConfigSection<SharpDoxConfig>();
        ///         ]]>
        ///     </code>
        ///     </example>
        ///     <typeparam name="T">Der Typ der Konfigurationssektion</typeparam>
        ///     <returns>Liefert die Konfigurationssektion zurück, falls diese existiert. Ansonsten <c>null</c>.</returns>
        /// </de>
        T GetConfigSection<T>();

        /// <default>
        ///     <summary>
        ///     A <c>List</c> of all recently used config files. Limited to the last five.
        ///     The key of the <c>KeyValuePair</c> is the path and the value is the name of the config file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Eine <c>List</c> der zuletzt benutzten Konfigurationsdateien. Limitiert auf die letzten fünf.
        ///     Der <c>Key</c> des <c>KeyValuePair</c>s ist der Pfad und der <c>Value</c> ider der Name der Konfigurationsdatei.
        ///     </summary>
        /// </de>
        List<KeyValuePair<string, string>> RecentProjects { get; }

        /// <default>
        ///     <summary>
        ///     Returns the path of the currently loaded config file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Pfad der aktuellen Konfigurationsdatei zurück.
        ///     </summary>
        /// </de>>
        string CurrentConfigPath { get; }
    }
}