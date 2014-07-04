using System;

namespace SharpDox.UML.Sequence.Elements
{
    internal class SequenceDiagramConnection : SequenceDiagramElement
    {
        public bool IsReturnConnection { get; set; }
        public Guid CallerId { get; set; }
        public Guid CalledId { get; set; }
        public string CalledMethodIdentifier { get; set; }
    }
}