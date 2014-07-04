using System.Windows.Media;
using System.Windows;

namespace SharpDox.UML
{
    internal static class Fonts
    {
        public static Typeface FontNormal = new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        public static Typeface FontBold = new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
        public static Typeface FontLight = new Typeface(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Light, FontStretches.Normal);
        public static Typeface FontItalic = new Typeface(new FontFamily("Verdana"), FontStyles.Italic, FontWeights.Normal, FontStretches.Normal);
    }
}
