namespace SharpDox.Sdk.Local
{
    /// <default>
    ///     <summary>
    ///     Every plugin which wants to use localized
    ///     strings has to implement this interface.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Jedes Plugin, das lokalisierte Strings nutzen
    ///     möchte, muss dieses Interface implementieren.
    ///     </summary>
    /// </de>
    public interface ILocalStrings
    {
        /// <default>
        ///     <summary>
        ///     Gets the name to use for the file.
        ///     </summary>
        ///     <example>
        ///     en.[DisplayName].slang
        ///     </example>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namen für die Datei.
        ///     </summary>
        ///     <example>
        ///     de.[DisplayName].slang
        ///     </example>
        /// </de>
        string DisplayName { get; }
    }
}
