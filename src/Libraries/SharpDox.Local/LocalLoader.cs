using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using SharpDox.Sdk.Local;

namespace SharpDox.Local
{
    public class LocalLoader
    {
        private const string LANGUAGEFOLDER = "lang";
        
        private readonly string _currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();

        private string _languageFolderPath;
        private ILocalStrings[] _localStrings;
        private string[] _localizationFiles;

        public void LoadLocalizations(ILocalStrings[] localStrings)
        {
            _localStrings = localStrings;
            var appRootFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(LocalCreator)).Location)).Parent.FullName;
            _languageFolderPath = Path.Combine(appRootFolder, LANGUAGEFOLDER);
            _localizationFiles = Directory.GetFiles(_languageFolderPath);

            ParseLocalizations();
        }

        private void ParseLocalizations()
        {
            foreach (var localStringsItem in _localStrings)
            {
                if (LocalizationExists(localStringsItem))
                {
                    ParseLocalization(localStringsItem);
                }
            }
        }

        private bool LocalizationExists(ILocalStrings localStrings)
        {
            return
                _localizationFiles.SingleOrDefault(
                    o => Path.GetFileName(o) == string.Format("{0}.{1}.sdlang", _currentLanguage, localStrings.DisplayName)) != null;
        }

        private void ParseLocalization(ILocalStrings localStrings)
        {
            var loadedStrings = ParseLocalizationFile(localStrings.DisplayName);
            foreach (var loadedString in loadedStrings)
            {
                if (LocalizationStringExists(localStrings, loadedString.Key))
                {
                    SetLocalizationString(localStrings, loadedString.Key, loadedString.Value);
                }
            }
        }

        private Dictionary<string, string> ParseLocalizationFile(string displayName)
        {
            var strings = new Dictionary<string, string>();

            var filePath = Path.Combine(_languageFolderPath,
                                        string.Format("{0}.{1}.sdlang", _currentLanguage, displayName));

            var lines = File.ReadAllLines(filePath);
            
            foreach (var line in lines)
            {
                var splittedLine = line.Split('=');
                if (splittedLine.Length == 2)
                {
                    strings.Add(splittedLine[0].Trim(), splittedLine[1].Trim());
                }
            }

            return strings;
        }

        private bool LocalizationStringExists(ILocalStrings localStrings, string propertyName)
        {
            return localStrings.GetType().GetProperty(propertyName) != null;
        }

        private void SetLocalizationString(ILocalStrings localStrings, string propertyName, string value)
        {
            localStrings.GetType().GetProperty(propertyName).SetValue(localStrings, value, null);
        }
    }
}
