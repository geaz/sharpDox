using System;

namespace SharpDox.UML.Sequence.Elements
{
    internal class SequenceDiagramElement
    {
        public SequenceDiagramElement()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; private set; }
        public string Text { get; set; }
    }
}
