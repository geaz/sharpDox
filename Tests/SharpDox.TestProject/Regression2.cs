using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDox.TestProject
{
    public class Regression2
    {
        public byte[] ReturnsByteArray()
        {
            var bytes = ReturnsByteArrayToo();
            bytes = ReturnsByteArrayToo();
            return bytes;
        }

        public byte[] ReturnsByteArrayToo()
        {
            var byteArray = new byte[10];
            return byteArray;
        }
    }
}
