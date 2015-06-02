using System;
using System.Globalization;
using System.Windows.Data;
using SharpDox.Sdk;

namespace SharpDox.GUI.Converters
{
    public class SDPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as SDPath;
            if (path == null)
            {
                return null;
            }

            return path.FullPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fullPath = value as string;
            if (string.IsNullOrEmpty(fullPath))
            {
                return null;
            }

            return new SDPath(fullPath);
        }
    }
}