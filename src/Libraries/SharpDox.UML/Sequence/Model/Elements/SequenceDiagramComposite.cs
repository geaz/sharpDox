using System;
using System.Collections.Generic;

namespace SharpDox.UML.Sequence.Elements
{
    internal class SequenceDiagramComposite : SequenceDiagramElement
    {
        public SequenceDiagramComposite()
        {
            Content = new List<SequenceDiagramElement>();
        }

        public SequenceDiagramConnection AddConnection(Guid callerId, Guid calledId, string text, string calledMethodIdentifier, bool isReturnConnection = false)
        {
            var connection = new SequenceDiagramConnection
            {
                CallerId = callerId,
                CalledId = calledId,
                CalledMethodIdentifier = calledMethodIdentifier,
                Text = text,
                IsReturnConnection = isReturnConnection
            };
            Content.Add(connection);

            return connection;
        }

        public SequenceDiagramComposite AddBlock(string text)
        {
            var block = new SequenceDiagramComposite
            {
                Text = text
            };
            Content.Add(block);

            return block;
        }

        public List<SequenceDiagramElement> Content { get; set; }
    }
}
