using System;
using SharpDox.Sdk.Local;

namespace SharpDox.Local
{
    public class LocalController : ILocalController
    {
        private readonly ILocalStrings[] _localStrings;

        public LocalController(ILocalStrings[] localStrings)
        {
            _localStrings = localStrings;

            new LocalCreator().CreateLocalizations(localStrings);
            new LocalLoader().LoadLocalizations(localStrings);
        }

        public T GetLocalStrings<T>()
        {
            var localStrings = default(T);

            foreach (var localString in _localStrings)
            {
                if (localString is T)
                {
                    localStrings = (T)localString;
                }
            }

            return localStrings;
        }

        public string GetLocalString(Type localType, string stringName)
        {
            var localString = string.Empty;

            foreach (var localStrings in _localStrings)
            {
                if (localStrings.GetType() == localType)
                {
                    var value = localStrings.GetType().GetProperty(stringName).GetValue(localStrings, null);
                    localString = value != null ? value.ToString() : string.Empty;
                }
            }

            return localString;
        }
    }
}
