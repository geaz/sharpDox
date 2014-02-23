using System;
using SharpDox.Model.Repository;

namespace SharpDox.Sdk.Exporter
{
    /// <default>
    ///     <summary>
    ///     Every plugin which wants to expose export functionality has to implement this interface.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Exportplugins müssen dieses Interface implementieren.
    ///     </summary>
    /// </de>
    public interface IExporter
    {
        /// <default>
        ///     <summary>
        ///     The BuildController of sharpDox listens on this event.
        ///     The Exporter is able to post warnings during the requirements check.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der BuildController von sharpDox lauscht an diesem Event.
        ///     Der Exporter kann darüber Warnungen verschicken, die im Baufenster angezeigt werden.
        ///     </summary>
        /// </de>
        event Action<string> OnRequirementsWarning;

        /// <default>
        ///     <summary>
        ///     The BuildController of sharpDox listens on this event.
        ///     The Exporter is able to post messages which will displayed in the progressbar.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der BuildController von sharpDox lauscht an diesem Event.
        ///     Der Exporter kann darüber Nachrichten verschicken, welche im Fortschrittsbalken angezeigt werden.
        ///     </summary>
        /// </de>
        event Action<string> OnStepMessage;

        /// <summary>
        /// The BuildController of sharpDox listens on this event.
        /// The Exporter is able to post a progress which will displayed in the progressbar.
        /// </summary>
        /// <default>
        ///     <summary>
        ///     The BuildController of sharpDox listens on this event.
        ///     The Exporter is able to post a progress which will displayed in the progressbar.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der BuildController von sharpDox lauscht an diesem Event.
        ///     Der Exporter kann darüber den Fortschritt verschicken, welcher im Fortschrittsbalken angezeigt wird.
        ///     </summary>
        /// </de>
        event Action<int> OnStepProgress;

        /// <default>
        ///     <summary>
        ///     This method gets called by the build process to start the export process.
        ///     </summary>
        ///     <param name="repository">The parsed solution.</param>
        ///     <param name="outputPath">
        ///     Outputpath for the plugin. Every output generated
        ///     by the plugin should be saved at this position.
        ///     </param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Diese Methode wird während des Bauvorgangs aufgerufen, um den Exportvorgang anzustoßen.
        ///     </summary>
        ///     <param name="repository">Die eingelesene Lösung.</param>
        ///     <param name="outputPath">
        ///     Der ausgabepfad für das Plugin. Alle Dateien, welche
        ///     durch das Plugin erstellt werden, sollten in diesem
        ///     Pfad gespeichert werden.
        ///     </param>     
        /// </de>
        void Export(SDRepository repository, string outputPath);

        /// <default>
        ///     <summary>
        ///     This method gets called by the build process to check any requirements of the plugin.
        ///     </summary>
        ///     <returns>
        ///     True if all requirements are fulfilled, false otherwise and the build process stops.
        ///     </returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Diese Methode wird während des Bauvorgangs aufgerufen, um die Vorraussetzungen des Plugins zu überprüfen.
        ///     </summary>
        ///     <returns>
        ///     True, falls alle Vorraussetzungen erfüllt wurden. Ansonsten false und der Bauvorgang wird gestoppt.
        ///     </returns> 
        /// </de>
        bool CheckRequirements();

        /// <default>
        ///     <summary>
        ///     Name of the exporter. This will be shown during the build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der Name des Exporters. Wird während des Bauvorgangs angezeigt.
        ///     </summary>
        /// </de>
        string ExporterName { get; }
    }
}
