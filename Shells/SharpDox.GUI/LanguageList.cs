using System;
using System.Collections.Generic;
using System.Globalization;

namespace SharpDox.GUI
{
    public class LanguageItem : IComparable<LanguageItem>
    {
        public string DisplayName { get; set; }
        public string TwoLetterCode { get; set; }

        public int CompareTo(LanguageItem other)
        {
            return DisplayName.CompareTo(other.DisplayName);
        }
    }

    internal class LanguageList : List<LanguageItem>
    {
        public LanguageList()
        {
            foreach (var language in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                Add(new LanguageItem { DisplayName = language.DisplayName, TwoLetterCode = language.TwoLetterISOLanguageName});
            }
            Sort();
        }
    }
}
