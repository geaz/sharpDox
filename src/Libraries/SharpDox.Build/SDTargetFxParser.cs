using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using SharpDox.Model;
using SharpDox.Model.Repository;

namespace SharpDox.Build
{
    public class SDTargetFxParser
    {
        public SDTargetFx GetTargetFx(string projectFile)
        {
            var targetFx = KnownTargetFxs.Unknown;
            var fileContents = File.ReadAllText(projectFile);

            if (IsXamarinAndroid(fileContents))
            {
                targetFx = KnownTargetFxs.XamarinAndroid;
            }
            else if (IsXamariniOS(fileContents))
            {
                targetFx = KnownTargetFxs.XamariniOS;
            }
            else
            {
                var document = XDocument.Parse(fileContents);
                var targetFrameworkIdentifier = ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkIdentifier");
                var targetFrameworkVersion = ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkVersion");
                var targetPlatformIdentifier = ReadXPathElementValue(document, "/Project/PropertyGroup/TargetPlatformIdentifier");
                var targetPlatformVersion = ReadXPathElementValue(document, "/Project/PropertyGroup/TargetPlatformVersion");
                var targetFrameworkProfile = ReadXPathElementValue(document, "/Project/PropertyGroup/TargetFrameworkProfile");

                targetFx = GetTargetFx(targetFrameworkIdentifier, targetFrameworkVersion, targetPlatformIdentifier, targetPlatformVersion, targetFrameworkProfile);
            }

            return targetFx;
        }

        private string ReadXPathElementValue(XDocument doc, string xpath)
        {
            var value = string.Empty;

            var mgr = new XmlNamespaceManager(new NameTable());
            mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

            var element = doc.XPathSelectElement(xpath.Replace("/", "/x:"), mgr);
            if (element != null)
            {
                value = element.Value;
            }

            return value;
        }

        private bool IsXamarinAndroid(string projectFileContents)
        {
            return projectFileContents.ToLower().Contains("xamarin.android.csharp.targets");
        }

        private bool IsXamariniOS(string projectFileContents)
        {
            return projectFileContents.ToLower().Contains("xamarin.ios.csharp.targets");
        }

        private SDTargetFx GetTargetFx(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetPlatformIdentifier, string targetPlatformVersion, string targetFrameworkProfile)
        {
            var targetFx = KnownTargetFxs.Unknown;

            // Note: PCL must be on top (since it also has v4.5)
            if (targetFrameworkProfile.ToLower().StartsWith("profile"))
            {
                targetFx = KnownTargetFxs.Pcl;
            }
            else if (string.Equals(targetFrameworkVersion, "v3.0", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Net30;
            }
            else if (string.Equals(targetFrameworkVersion, "v3.5", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Net35;
            }
            else if (string.Equals(targetFrameworkVersion, "v4.0", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Net40;
            }
            else if (string.Equals(targetFrameworkVersion, "v4.5", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Net45;
            }
            else if (string.Equals(targetFrameworkVersion, "v4.6", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Net46;
            }
            else if (string.Equals(targetFrameworkIdentifier, "silverlight", StringComparison.OrdinalIgnoreCase) && string.Equals(targetFrameworkVersion, "v5.0", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Silverlight5;
            }
            else if (string.Equals(targetFrameworkIdentifier, "windowsphone", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(targetFrameworkVersion, "v8.0", StringComparison.OrdinalIgnoreCase))
                {
                    targetFx = KnownTargetFxs.WindowsPhone80;
                }
                else if (string.Equals(targetFrameworkVersion, "v8.1", StringComparison.OrdinalIgnoreCase))
                {
                    targetFx = KnownTargetFxs.WindowsPhone81Silverlight;
                }
            }
            else if (string.Equals(targetPlatformIdentifier, "uap", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Windows100;
            }
            else if (string.Equals(targetPlatformIdentifier, "WindowsPhoneApp", StringComparison.OrdinalIgnoreCase) && string.Equals(targetPlatformVersion, "8.1", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.WindowsPhone81Runtime;
            }
            else if (string.Equals(targetPlatformVersion, "8.1", StringComparison.OrdinalIgnoreCase))
            {
                targetFx = KnownTargetFxs.Windows81;
            }

            return targetFx;
        }
    }
}