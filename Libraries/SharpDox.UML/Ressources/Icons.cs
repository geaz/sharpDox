using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace SharpDox.UML.Ressources
{
    internal static class Icons
    {
        private static readonly Object _lockObj = new Object();

        public static BitmapImage GetIcon(string entityType, string accessibility)
        {
            BitmapImage bitmap = null;
            switch (accessibility.ToLower())
            {
                case "friend":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Friend.png", entityType)));
                    break;
                case "private":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Private.png", entityType)));
                    break;
                case "protected":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Protected.png", entityType)));
                    break;
                case "public":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Public.png", entityType)));
                    break;
                case "sealed":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Sealed.png", entityType)));
                    break;
                case "internal":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Sealed.png", entityType)));
                    break;
                case "shortcut":
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Shortcut.png", entityType)));
                    break;
                default:
                    bitmap = new BitmapImage(new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Public.png", entityType)));
                    break;
            }

            return bitmap;
        }

        public static string GetBase64Icon(string entityType, string accessibility)
        {
            Uri uri = null;

            switch (accessibility.ToLower())
            {
                case "friend":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Friend.png", entityType));
                    break;
                case "private":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Private.png", entityType));
                    break;
                case "protected":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Protected.png", entityType));
                    break;
                case "public":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Public.png", entityType));
                    break;
                case "sealed":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Sealed.png", entityType));
                    break;
                case "internal":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Sealed.png", entityType));
                    break;
                case "shortcut":
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Shortcut.png", entityType));
                    break;
                default:
                    uri = new Uri(string.Format("pack://application:,,,/SharpDox.UML;component/Ressources/Icons/{0}_Public.png", entityType));
                    break;
            }

            StreamResourceInfo sri = null;
            lock (_lockObj)
            {
                sri = Application.GetResourceStream(uri);
            }

            var base64 = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                sri.Stream.CopyTo(memoryStream);
                var result = memoryStream.ToArray();
                base64 = Convert.ToBase64String(result);
            }

            return base64;
        }
    }
}
