using System;
using System.ComponentModel;

namespace SharpDox.Sdk.Config
{
    /// <default>
    ///     <summary>
    ///     Every plugin which needs new configuration items can implement this interface.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Plugins, die Konfigurationseinstellungen benötigen, können dieses Interface implementieren.
    ///     </summary>
    /// </de>
    public interface IConfigSection : INotifyPropertyChanged
    {
        /// <default>
        ///     <summary>
        ///     A unique global identifier. Will be used to save the 
        ///     configuration into the sharpDox config file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Ein eindeutiger Identifikator. Wird genutzt um die Konfiguration
        ///     in der sharpDox Konfigurationsdatei zuspeichern.
        ///     </summary>
        /// </de>
        Guid Guid { get; }
    }
}
