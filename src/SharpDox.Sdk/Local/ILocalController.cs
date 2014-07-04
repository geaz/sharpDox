using System;

namespace SharpDox.Sdk.Local
{
    public interface ILocalController
    {
        T GetLocalStrings<T>();
        string GetLocalString(Type localType, string stringName);
    }
}
