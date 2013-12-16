namespace SharpDox.Sdk.UI
{
    /// <default>
    ///     <summary>
    ///     Every Plugin which wants to add a settings page to the gui
    ///     has to implement this interface.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Plugins, welche eine Einstellungsseite in der GUI bereitstellen wollen,
    ///     müssen dieses Interface implementieren.
    ///     </summary>
    /// </de>
    public interface IPage
    {
        /// <default>
        ///     <summary>
        ///     Gets the title for this settings page.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Titel der Einstellungsseite.
        ///     </summary>
        /// </de>
        string PageName { get; }

        /// <default>
        ///     <summary>
        ///     Gets the width of the settings page.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Breite der Einstellungsseite.
        ///     </summary>
        /// </de>
        int Width { get; }

        /// <default>
        ///     <summary>
        ///     Gets the height of the settings page.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Höhe der EInstellungsseite.
        ///     </summary>
        /// </de>
        int Height { get; }
    }
}
