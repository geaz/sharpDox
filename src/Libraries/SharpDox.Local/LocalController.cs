using System;
using System.Linq;
using SharpDox.Sdk.Local;
using System.Collections.Generic;
using System.Threading;

namespace SharpDox.Local
{
    public class LocalController : ILocalController
    {
        private readonly List<LocalStringsItem> _localStrings;

        public LocalController(ILocalStrings[] localStrings)
        {
            new LocalCreator().CreateLocalizations(localStrings);
            _localStrings = new LocalLoader().LoadLocalizations(localStrings);
        }

        public T GetLocalStrings<T>()
        {
            return GetLocalStringsOrDefault<T>(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower());
        }

        public T GetLocalStringsOrDefault<T>(string language)
        {
            var localStrings = _localStrings.SingleOrDefault(l => l.Language == language && l.LocalStrings is T);
            if (localStrings == null)
            {
                localStrings = _localStrings.SingleOrDefault(l => l.Language == "default" && l.LocalStrings is T);
            }
            return localStrings != null ? (T)localStrings.LocalStrings : default(T);
        }

        public string GetLocalString(Type localType, string stringName)
        {
            var localString = string.Empty;

            var currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            var localStringItem = _localStrings.SingleOrDefault(l => l.Language == currentLanguage && l.LocalStrings.GetType() == localType);
            if (localStringItem != null)
            {
                var value = localStringItem.LocalStrings.GetType().GetProperty(stringName).GetValue(localStringItem.LocalStrings, null);
                localString = value != null ? value.ToString() : string.Empty;
            }

            return localString;
        }
    }
}
