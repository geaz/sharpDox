using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SharpDox.UML.Ressources
{
    internal static class Icons
    {
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
            string ressourceName;
            switch (accessibility.ToLower())
            {
                case "friend":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Friend.png", entityType);
                    break;
                case "private":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Private.png", entityType);
                    break;
                case "protected":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Protected.png", entityType);
                    break;
                case "public":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Public.png", entityType);
                    break;
                case "sealed":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Sealed.png", entityType);
                    break;
                case "internal":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Sealed.png", entityType);
                    break;
                case "shortcut":
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Shortcut.png", entityType);
                    break;
                default:
                    ressourceName = string.Format("SharpDox.UML.Ressources.Icons.Embedded.{0}_Public.png", entityType);
                    break;
            }
            
            var base64 = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                typeof(Icons).Assembly.GetManifestResourceStream(ressourceName).CopyTo(memoryStream);
                var result = memoryStream.ToArray();
                base64 = Convert.ToBase64String(result);
            }

            return base64;
        }
    }
}
