using System;

namespace SharpDox.TestProject
{
    /// <summary>
    /// Description of IInheritDoc.
    /// </summary>
    public interface IInheritDoc
    {
        /// <summary>
        /// Description of Method1
        /// </summary>
        void Method1();
    }

    public class InheritDoc : IInheritDoc
    {
        /// <inheritdoc />
        public void Method1()
        {
            throw new NotImplementedException();
        }
    }
}
