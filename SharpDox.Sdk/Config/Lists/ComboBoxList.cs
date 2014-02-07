using System;
using System.Collections.Generic;

namespace SharpDox.Sdk.Config.Lists
{
    public abstract class ComboBoxList : List<Tuple<object, string>>
    {
        public void Add(object value, string name)
        {
            Add(new Tuple<object, string>(value, name));
        }
    }
}
