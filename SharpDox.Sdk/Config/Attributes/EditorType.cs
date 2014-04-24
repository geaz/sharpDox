namespace SharpDox.Sdk.Config.Attributes
{
    /// <default>
    ///     <summary>
    ///     All possible editor types (besides the default ones for strings & booleans).
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Alle möglichen Editortypen für die GUI (außer den standard Controls für strings & booleans).
    ///     </summary>
    /// </de>
    public enum EditorType
    {
        /// <default>
        ///     <summary>
        ///     A color picker control to select a hex value.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Colorpicker, um einen Hex-Farbwert auszuwählen.
        ///     </summary>
        /// </de>
        Colorpicker,

        /// <default>
        ///     <summary>
        ///     A system folder browser to select a folder.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Dateisystem-Browser, um einen Ordner auszuwählen.
        ///     </summary>
        /// </de>
        Folderpicker,

        /// <default>
        ///     <summary>
        ///     A system file browser to select a file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Dateisystem-Browser, um eine Datei auszuwählen.
        ///     </summary>
        /// </de>
        Filepicker,

        /// <default>
        ///     <summary>
        ///     A combobox control.
        ///     </summary>
        ///     <remarks>
        ///     If you use this editortype, you have to define a sourcelist type.
        ///     </remarks>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Combobox-Control.
        ///     </summary>
        ///     <remarks>
        ///     Falls dieser Editortyp genutzt werden soll muss eine Sourcelist angeben werden.
        ///     </remarks>
        /// </de>
        ComboBox,

        /// <default>
        ///     <summary>
        ///     A combobox control with checkboxes to make multiple selections.
        ///     </summary>
        ///     <remarks>
        ///     If you use this editortype, you have to define a sourcelist type.
        ///     </remarks>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Combobox-Control mit Checkboxen, um Mehrfachauswahlen zu ermöglichen.
        ///     </summary>
        ///     <remarks>
        ///     Falls dieser Editortyp genutzt werden soll muss eine Sourcelist angeben werden.
        ///     </remarks>
        /// </de>
        CheckBoxList,

        /// <default>
        ///     <summary>
        ///     A markdown control to help creating markdown text.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein Markdown-Control, das bei der Erstellung von Markdown-Syntax unterstützt.
        ///     </summary>
        /// </de>
        Markdown
    }
}
