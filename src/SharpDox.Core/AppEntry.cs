using System;
using System.Diagnostics;

namespace SharpDox.Core
{
    public class AppEntry
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var bootStrapper = new BootStrapper(args);
                bootStrapper.StartSharpDox();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
