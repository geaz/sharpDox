using System.Globalization;
using SharpDox.Sdk.Config.Lists;

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
        }
    }
}
