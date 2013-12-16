namespace SharpDox.Sdk.UI
{
    /// <default>
    ///     <summary>
    ///     All shells used by sharpDox are implementing this interface.
    ///     Developers are able to implement this interface to create new shells.
    ///     For example it could be used to create an alternative gui.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Alle Shells von sharpDox implementieren dieses Interface.
    ///     Entwicklern ist möglich durch die Implementierung dieses
    ///     Interfaces eigene Shells zu erstellen. Zum Beispiel
    ///     wären alternative GUIs denkbar.
    ///     </summary>
    /// </de>
    public interface IShell
    {
        /// <default>
        ///     <summary>
        ///     This is the entrypoint for the shell.
        ///     Started by the sharpDox Core.
        ///     </summary>
        ///     <param name="args">If sharpDox got started with any parameters, they will be redirected to the shell.</param>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der Einstiegspunkt für die Shell.
        ///     Diese Funktion wird von sharpDox gestartet.
        ///     </summary>
        ///     <param name="args">Falls sharpDox mit Parametern gestartet wurde, werden diese an die Shell weitergeleitet.</param>
        /// </de>
        void Start(string[] args);

        /// <default>
        ///     <summary>
        ///     Gets a value indicating whether the shell is a graphical user interface.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert einen Wert der angibt, ob die Shell grafisch ist.
        ///     </summary>
        /// </de>
        bool IsGui { get; }
    }
}
