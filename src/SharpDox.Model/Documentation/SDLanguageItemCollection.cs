using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDox.Model.Documentation
{
    /// <default>
    ///     <summary>
    ///     Represents a collection of language specific elements.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Repräsentiert eine Kollektion von sprachspezifischen Elementen.
    ///     </summary>
    /// </de>
    public class SDLanguageItemCollection<T> : Dictionary<string, T>
    {        
        /// <default>
        ///     <summary>
        ///         Gets the element of the given language, if it exists.
        ///         Otherwise the default language element or <c>null</c>.
        ///     </summary>
        ///     <param name="language">The language of the element</param>
        ///     <returns>
        ///     The element of the given language, if it exists. 
        ///     Otherwise the default language element, if it exists. 
        ///     Otherwise <c>null</c>.
        ///     </returns>
        /// </default>
        /// <de>
        ///     <summary>
        ///         Liefert das Element für die angegebene Sprache zurück.
        ///         Falls diese nicht vorhanden ist wird das "default" Element oder <c>null</c> geliefert.
        ///     </summary>
        ///     <param name="language">Die Sprache des Elements</param>
        ///     <returns>
        ///     Liefert das Element für die angegebene Sprache zurück. 
        ///     Falls dieses nicht vorhanden ist, das "default" Element. 
        ///     Falls dieses auch nicht vorhanden ist wird <c>null</c> geliefert.
        ///     </returns>
        /// </de>
        public T GetElementOrDefault(string language)
        {
            T element;

            TryGetValue(language, out element);
            if (element == null)
            {
                TryGetValue("default", out element);
            }

            return element != null ? element : default(T);
        }
    }
}
