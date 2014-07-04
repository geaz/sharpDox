using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SharpDox.ConsoleHelper
{
    /// <summary>
    /// Class to get a dictionary of all start parameters of a console application.
    /// Valid Parameters are:
    /// {-,/,--}param{ ,=,:}((",')value(",'))
    /// 
    /// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
    /// </summary>
    public class ConsoleArguments : StringDictionary
    {
        private readonly Regex _nameValueRegex = new Regex(@"^([/-]|--){1}(?<name>\w+)([:=])?(?<value>.+)?$", RegexOptions.Compiled);

        public ConsoleArguments(IEnumerable<string> args)
        {
            var lastName = string.Empty;
            char[] trimChars = { '"', '\'' };

            foreach (string arg in args)
            {
                var match = _nameValueRegex.Match(arg);
                if (!match.Success)
                {
                    if (!string.IsNullOrEmpty(lastName))
                    {
                        this[lastName] = arg.Trim(trimChars);
                    }
                }
                else
                {
                    lastName = match.Groups["name"].Value;
                    Add(lastName, match.Groups["value"].Value.Trim(trimChars));
                }
            }
        }
    }
}