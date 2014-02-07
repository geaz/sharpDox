using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDox.Sdk.Config.Attributes
{
    public class ConfigEditorAttribute : Attribute
    {
        public ConfigEditorAttribute(EditorType editorType)
        {
            Editor = editorType;
        }

        public EditorType Editor { get; set; }
    }
}
