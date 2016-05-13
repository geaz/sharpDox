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
