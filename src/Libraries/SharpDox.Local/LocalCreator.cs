using System.IO;
using System.Reflection;
using SharpDox.Sdk.Local;

namespace SharpDox.Local
{
    public class LocalCreator
    {
        private const string DEFAULTLANGUAGEFOLDER = "lang/defaults";

        private string _defaultLanguageFolder;

        public void CreateLocalizations(ILocalStrings[] localStrings)
        {
            EnsureDefaultLanguageFolder();
            foreach (var localString in localStrings)
            {
                CreateDefaultLocalFile(localString);
            }
        }

        private void EnsureDefaultLanguageFolder()
        {
            var appRootFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetAssembly(typeof(LocalCreator)).Location)).Parent.FullName;
            _defaultLanguageFolder = Path.Combine(appRootFolder, DEFAULTLANGUAGEFOLDER);
            if (!Directory.Exists(_defaultLanguageFolder))
            {
                Directory.CreateDirectory(_defaultLanguageFolder);
            }
        }

        private void CreateDefaultLocalFile(ILocalStrings localString)
        {
            var defaultLocalFile = GetFileContent(localString);
            File.WriteAllText(Path.Combine(_defaultLanguageFolder, "en." + localString.DisplayName + ".sdlang"), defaultLocalFile);
        }

        private string GetFileContent(ILocalStrings localString)
        {
            var defaultLocalFile = string.Empty;
            foreach (var property in localString.GetType().GetProperties())
            {
                if (property.Name != "DisplayName")
                {
                    defaultLocalFile += string.Format("{0} = {1}\n", property.Name, property.GetValue(localString, null));
                }
            }
            return defaultLocalFile;
        }
    }
}
