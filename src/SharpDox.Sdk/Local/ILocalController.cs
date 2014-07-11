using System;

namespace SharpDox.Sdk.Local
{
    public interface ILocalController
    {
        T GetLocalStrings<T>();
        T GetLocalStringsOrDefault<T>(string language);
        string GetLocalString(Type localType, string stringName);
    }
}
