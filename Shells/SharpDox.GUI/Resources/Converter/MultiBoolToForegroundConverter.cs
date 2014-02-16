using System;
using System.Windows.Data;
using System.Windows.Media;

namespace SharpDox.GUI.Resources.Converter
{
    public class MultiBoolToForegroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var solidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF272624"));
            var isExcluded = (bool)values[0];
            var hasExcludedChild = (bool)values[1];

            if (isExcluded)
            {
                solidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF979797"));
            }
            else if (hasExcludedChild)
            {
                solidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0066CC"));
            }

            return solidColorBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
