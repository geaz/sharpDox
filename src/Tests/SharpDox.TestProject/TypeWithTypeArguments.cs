using System.Collections.Generic;

namespace SharpDox.TestProject
{
    public class TypeWithTypeArguments<T> where T : Regression1
    {
        public void DoIt()
        {
            
        }

        public void DoMore(int t)
        {
            
        }

        public void DoEvenMore<TV>(T t, TV l) where TV: Regression2
        {
            
        }

        public dynamic ReturnDynamic()
        {
            return null;
        }

        public string[][] MoreDimensionArrayReturn()
        {
            return null;
        }

        public unsafe int*[][] MoreDimensionPointerArrayReturn()
        {
            return null;
        }

        public List<string> TypeParameters(List<string> test)
        {
            return new List<string>();
        }
    }
}
