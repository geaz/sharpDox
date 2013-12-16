using SharpDox.Sdk.Local;

namespace SharpDox.Local
{
    public class LocalController
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
                    localStrings = (T) localString;
                }
            }

            return localStrings;
        }
    }
}
