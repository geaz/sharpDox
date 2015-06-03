using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using SharpDox.Model;
using SharpDox.Model.Repository;

namespace SharpDox.Build
{
    public class SDTargetFxParser
    {
        private static readonly SDTargetFx UnknownTargetFx = new SDTargetFx
        {
            Identifier = "Unknown",
            Name = "Unknown"
        };

        public SDTargetFx GetTargetFx(string projectFile)
        {
            var fileContents = File.ReadAllText(projectFile);

            if (IsXamarinAndroid(fileContents))
            {
                return KnownTargetFxs.XamarinAndroid;
            }

            if (IsXamariniOS(fileContents))
            {
                return KnownTargetFxs.XamariniOS;
            }

            var document = XDocument.Parse(fileContents);

            var targetFrameworkIdentifier = string.Empty;
            var targetFrameworkVersion = string.Empty;
            var targetPlatformIdentifier = string.Empty;
            var targetPlatformVersion = string.Empty;
            var targetFrameworkProfile = string.Empty;

            ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkIdentifier", s => targetFrameworkIdentifier = s);
            ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkVersion", s => targetFrameworkVersion = s);
            ReadXPathElementValue(document, "/Project/PropertyGroup/TargetPlatformIdentifier", s => targetPlatformIdentifier = s);
            ReadXPathElementValue(document, "/Project/PropertyGroup/TargetPlatformVersion", s => targetPlatformVersion = s);
            ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkProfile", s => targetFrameworkProfile = s);

            return GetTargetFx(targetFrameworkIdentifier, targetFrameworkVersion, targetPlatformIdentifier, targetPlatformVersion, targetFrameworkProfile);
        }

        private void ReadXPathElementValue(XDocument doc, string xpath, Action<string> setter)
        {
            var mgr = new XmlNamespaceManager(new NameTable());
            mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

            xpath = xpath.Replace("/", "/x:");

            var element = doc.XPathSelectElement(xpath, mgr);
            if (element != null)
            {
                setter(element.Value);
            }
        }

        private SDTargetFx GetTargetFx(string targetFrameworkIdentifier, string targetFrameworkVersion,
            string targetPlatformIdentifier, string targetPlatformVersion, string targetFrameworkProfile)
        {
            // Note: PCL must be on top (since it also has v4.5)
            if (targetFrameworkProfile.ToLower().StartsWith("profile"))
            {
                return KnownTargetFxs.Pcl;
            }

            if (string.Equals(targetFrameworkVersion, "v4.0", StringComparison.OrdinalIgnoreCase))
            {
                return KnownTargetFxs.Net40;
            }

            if (string.Equals(targetFrameworkVersion, "v4.5", StringComparison.OrdinalIgnoreCase))
            {
                return KnownTargetFxs.Net45;
            }

            if (string.Equals(targetFrameworkVersion, "v4.6", StringComparison.OrdinalIgnoreCase))
            {
                return KnownTargetFxs.Net46;
            }

            if (string.Equals(targetFrameworkIdentifier, "silverlight", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(targetFrameworkVersion, "v5.0", StringComparison.OrdinalIgnoreCase))
                {
                    return KnownTargetFxs.Silverlight5;
                }
            }

            if (string.Equals(targetFrameworkIdentifier, "windowsphone", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(targetFrameworkVersion, "v8.0", StringComparison.OrdinalIgnoreCase))
                {
                    return KnownTargetFxs.WindowsPhone80;
                }

                if (string.Equals(targetFrameworkVersion, "v8.1", StringComparison.OrdinalIgnoreCase))
                {
                    return KnownTargetFxs.WindowsPhone81Silverlight;
                }
            }

            if (string.Equals(targetPlatformIdentifier, "uap", StringComparison.OrdinalIgnoreCase))
            {
                return KnownTargetFxs.Windows100;
            }

            if (string.Equals(targetPlatformIdentifier, "WindowsPhoneApp", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(targetPlatformVersion, "8.1", StringComparison.OrdinalIgnoreCase))
                {
                    return KnownTargetFxs.WindowsPhone81Runtime;
                }
            }

            if (string.Equals(targetPlatformVersion, "8.1", StringComparison.OrdinalIgnoreCase))
            {
                return KnownTargetFxs.Windows81;
            }

            return UnknownTargetFx;
        }

        private bool IsXamarinAndroid(string projectFileContents)
        {
            return projectFileContents.ToLower().Contains("xamarin.android.csharp.targets");
        }

        private bool IsXamariniOS(string projectFileContents)
        {
            return projectFileContents.ToLower().Contains("xamarin.ios.csharp.targets");
        }
    }
}