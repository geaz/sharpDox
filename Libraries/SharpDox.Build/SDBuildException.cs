using System;
using System.Runtime.Serialization;

namespace SharpDox.Build
{
    internal class SDBuildException : Exception
    {
        public SDBuildException(string message)
            : base(message)
        {
        }

        public SDBuildException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public SDBuildException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SDBuildException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected SDBuildException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}