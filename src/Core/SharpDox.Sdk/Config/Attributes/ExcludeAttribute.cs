using System;

namespace SharpDox.Sdk.Config.Attributes
{
    /// <default>
    ///     <summary>
    ///     Configuration items marked by this attribute 
    ///     will not be part of the configuration file.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Konfigurationselemente die mit diesem Attribut gekennzeichnet wurden
    ///     werden kein Teil der Konfigurationsdatei die von sharpDox gespeichert wird.
    ///     </summary>
    /// </de>
    public class ExcludeAttribute : Attribute { }
}
