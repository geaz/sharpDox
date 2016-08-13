using System;

namespace SharpDox.Sdk.Config.Attributes
{
    /// <default>
    ///     <summary>
    ///     If a configuration item should be visible in the gui,
    ///     it needs to be marked by this attribute to get a display name.
    ///     </summary>
    ///     <example>
    ///         <code>
    ///             [Required]
    ///             [Name(typeof(CoreStrings), "ProjectName")]
    ///             public string ProjectName
    ///             {
    ///                 get { /* ... */ }
    ///                 set { /* ... */ }
    ///             }
    ///         </code>
    ///     </example>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Falls ein Konfigurationselement in der GUI sichtbar sein soll,
    ///     muss es mit diesem Attribut gekennzeichnet sein. Damit bekommt
    ///     es einen Anzeigenamen für das Propertygrid.
    ///     </summary>
    ///     <example>
    ///         <code>
    ///             [Required]
    ///             [Name(typeof(CoreStrings), "ProjectName")]
    ///             public string ProjectName
    ///             {
    ///                 get { /* ... */ }
    ///                 set { /* ... */ }
    ///             }
    ///         </code>
    ///     </example>
    /// </de>
    public class NameAttribute : Attribute
    {
        /// <default>
        ///     <summary>
        ///     The name attribute for a configuration item in a custom 
        ///     configuration section.
        ///     </summary>
        ///     <param name="localType">The type of the custom strings to get the title.</param>
        ///     <param name="displayName">Name of the property to get the title.</param>     
        ///     <seealso cref="SharpDox.Sdk.Config.IConfigSection"/>
        ///     <seealso cref="SharpDox.Sdk.Local.ILocalStrings"/>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Das Namensattribut für ein Konfigurationselement in einer
        ///     benutzerdefinierten Konfigurationssektion.
        ///     </summary>
        ///     <param name="localType">Der Typ der benutzerdefinierten Strings von dem der Titel geholt werden soll.</param>
        ///     <param name="displayName">Der Name der Eigenschaft von der der Titel geholt werden soll.</param>
        ///     <seealso cref="SharpDox.Sdk.Config.IConfigSection"/>
        ///     <seealso cref="SharpDox.Sdk.Local.ILocalStrings"/>
        /// </de>
        public NameAttribute(Type localType, string displayName)
        {
            LocalType = localType;
            DisplayName = displayName;
        }

        /// <default>
        ///     <summary>
        ///     The type of the custom strings to get the title.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der Typ der benutzerdefinierten Strings von dem der Titel geholt werden soll.
        ///     </summary>
        /// </de>
        public Type LocalType { get; set; }

        /// <default>
        ///     <summary>
        ///     Name of the property to get the title.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Der Name der Eigenschaft von der der Titel geholt werden soll.
        ///     </summary>
        /// </de>
        public string DisplayName { get; set; }
    }
}
