using System;
using SharpDox.Model.Repository;

namespace SharpDox.Sdk.Build
{
    /// <default>
    ///     <summary>
    ///     This interface exposes all events used by the implementation of the IBuildController.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Dieses Interface bildet alle Events ab, die von der Implementierung des <c>IBuildController</c>s genutzt werden.
    ///     </summary>
    /// </de>
    public interface IBuildMessenger
    {
        /// <default>
        ///     <summary>
        ///     Gets fired, if the <see cref="IBuildController.StartParse"/> method of the <see cref="IBuildController"/> has finished.
        ///     The <c>SDRepository</c> can be <c>null</c>, if the parse wasn't successful.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls die Methode <see cref="IBuildController.StartParse"/> des <see cref="IBuildController"/> beendet wurde.
        ///     Das <c>SDRepository</c> kann <c>null</c> sein, falls das Einlesen nicht erfolgreich war.
        ///     </summary>
        /// </de>
        event Action<SDRepository> OnParseCompleted;
                
        event Action OnParseFailed;

        event Action OnParseStopped;

        /// <default>
        ///     <summary>
        ///     Gets fired, if the <c>BuildController</c> sends a message during the build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls der <c>BuildController</c> eine Nachricht während des Bauvorgangs sendet.
        ///     </summary>
        /// </de>
        event Action<string> OnBuildMessage;

        /// <default>
        ///     <summary>
        ///     Gets fired, if the <c>BuildController</c> sends a message during the current step of the build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls der <c>BuildController</c> eine Nachricht während des aktuellen Vorgangs sendet.
        ///     </summary>
        /// </de>
        event Action<string> OnStepMessage;

        /// <default>
        ///     <summary>
        ///     Gets fired, if the <c>BuildController</c> sends a progress update for the current build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls der <c>BuildController</c> eine Fortschrittsmeldung für den aktuellen Bauvorgang sendet.
        ///     </summary>
        /// </de>
        event Action<int> OnBuildProgress;

        /// <default>
        ///     <summary>
        ///     Gets fired, if the <c>BuildController</c> sends a progress update for the current step of the build process.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls der <c>BuildController</c> eine Fortschrittsmeldung für den aktuellen Vorgang sendet.
        ///     </summary>
        /// </de>
        event Action<int> OnStepProgress;

        /// <default>
        ///     <summary>
        ///     Gets fired, if the <see cref="IBuildController.StartBuild"/> method of the <see cref="IBuildController"/> has stopped.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Wird gefeurt, falls die Methode <see cref="IBuildController.StartBuild"/> des <see cref="IBuildController"/> gestoppt wurde.
        ///     </summary>
        /// </de>
        event Action OnBuildStopped;

        event Action OnBuildFailed;

        event Action OnBuildCompleted;
    }
}