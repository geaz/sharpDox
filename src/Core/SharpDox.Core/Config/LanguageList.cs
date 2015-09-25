using System.Globalization;
using SharpDox.Sdk.Config.Lists;
using System;

namespace SharpDox.Core.Config
{
    public class LanguageList : ComboBoxList
    {
        public LanguageList()
        {
            foreach (var language in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                Add(language.TwoLetterISOLanguageName, language.DisplayName);
            }

            Sort(delegate (Tuple<object, string> one, Tuple<object, string> two)
            {
                return one.Item2.CompareTo(two.Item2);
            });
        }
    }
}
