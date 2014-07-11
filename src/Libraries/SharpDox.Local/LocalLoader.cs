using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using SharpDox.Sdk.Local;
using System;

namespace SharpDox.Local
{
    public class LocalLoader
    {
        private const string LANGUAGEFOLDER = "lang";
        
        private readonly string _currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();

        private string _languageFolderPath;
        private ILocalStrings[] _localStrings;
        private string[] _localizationFiles;

        public List<LocalStringsItem> LoadLocalizations(ILocalStrings[] localStrings)
        {
            _localStrings = localStrings;
            var appRootFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(LocalCreator)).Location)).Parent.FullName;
            _languageFolderPath = Path.Combine(appRootFolder, LANGUAGEFOLDER);
            _localizationFiles = Directory.GetFiles(_languageFolderPath);

            ParseCurrentLocalization();
            return GetAllLocalizations();
        }

        private void ParseCurrentLocalization()
        {
            foreach (var localStringsItem in _localStrings)
            {
                if (LocalizationExists(localStringsItem))
                {
                    ParseLocalization(localStringsItem, _currentLanguage);
                }
            }
        }

        private List<LocalStringsItem> GetAllLocalizations()
        {
            var localStringsCollection = new List<LocalStringsItem>();
            foreach (var localStringsItem in _localStrings)
            {
                localStringsCollection.Add(new LocalStringsItem("default", (ILocalStrings)Activator.CreateInstance(localStringsItem.GetType())));
            }

            foreach(var localizationFile in _localizationFiles)
            {
                var splittedFilename = Path.GetFileNameWithoutExtension(localizationFile).Split('.');
                var localStrings = _localStrings.SingleOrDefault(l => l.DisplayName == splittedFilename[1]);
                if(localStrings != null)
                {
                    var newLocalStrings = (ILocalStrings)Activator.CreateInstance(localStrings.GetType());
                    ParseLocalization(newLocalStrings, splittedFilename[0]);
                    localStringsCollection.Add(new LocalStringsItem(splittedFilename[0], newLocalStrings));
                }
            }
            return localStringsCollection;
        }

        private bool LocalizationExists(ILocalStrings localStrings)
        {
            return
                _localizationFiles.SingleOrDefault(
                    o => Path.GetFileName(o) == string.Format("{0}.{1}.sdlang", _currentLanguage, localStrings.DisplayName)) != null;
        }

        private void ParseLocalization(ILocalStrings localStrings, string language)
        {
            var loadedStrings = ParseLocalizationFile(localStrings.DisplayName, language);
            foreach (var loadedString in loadedStrings)
            {
                if (LocalizationStringExists(localStrings, loadedString.Key))
                {
                    SetLocalizationString(localStrings, loadedString.Key, loadedString.Value);
                }
            }
        }

        private Dictionary<string, string> ParseLocalizationFile(string displayName, string language)
        {
            var strings = new Dictionary<string, string>();

            var filePath = Path.Combine(_languageFolderPath,
                                        string.Format("{0}.{1}.sdlang", language, displayName));

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
