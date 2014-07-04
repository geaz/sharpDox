using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SharpDox.UML.Extensions
{
    internal static class StringToWidth
    {
        public static double GetWidth(this string text, int fontSize, Typeface typeFace)
        {
            var formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, fontSize, Brushes.Black);
            return formattedText.Width;
        }
    }
}
