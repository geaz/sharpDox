using System.Collections.Generic;

namespace SharpDox.Sdk.Config.Lists
{
    public class CheckBoxNode
    {
        public CheckBoxNode(string name)
        {
            IsChecked = false;
            Name = name;
        }

        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    public class CheckBoxList : List<CheckBoxNode>
    {
        public void Add(string name)
        {
            Add(new CheckBoxNode(name));
        }
    }
}
