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
    }
}
