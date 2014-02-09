using System;
using System.Collections.Generic;

namespace SharpDox.Sdk.Config.Lists
{
    public class CheckBoxNode
    {
        public CheckBoxNode(bool isChecked, string name)
        {
            IsChecked = isChecked;
            Name = name;
        }

        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    public class CheckBoxList : List<CheckBoxNode>
    {
        private readonly bool _defaultIsChecked;

        public CheckBoxList(bool defaultIsChecked)
        {
            _defaultIsChecked = defaultIsChecked;
        }

        public void Add(string name)
        {
            Add(new CheckBoxNode(_defaultIsChecked, name));
        }
    }
}
