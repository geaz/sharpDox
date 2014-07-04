using System;

namespace SharpDox.Sdk.Config.Attributes
{
    public class ConfigEditorAttribute : Attribute
    {
        public ConfigEditorAttribute(EditorType editorType)
        {
            Editor = editorType;
        }

        public ConfigEditorAttribute(EditorType editorType, Type sourceListType)
        {
            Editor = editorType;
            SourceListType = sourceListType;
        }

        public ConfigEditorAttribute(EditorType editorType, string openFileFilter)
        {
            Editor = editorType;
            OpenFileFilter = openFileFilter;
        }

        public EditorType Editor { get; set; }
        public Type SourceListType { get; set; }
        public string OpenFileFilter { get; set; }
    }
}
