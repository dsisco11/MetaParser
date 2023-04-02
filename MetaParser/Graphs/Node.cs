using System.Collections.Generic;
using System.Diagnostics;

namespace MetaParser.Graphs;

internal partial class DirectedGraph
{
    #region Records
    [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    public readonly record struct Node
    {
        public readonly HashSet<EntityKey> Incoming = new();
        public readonly HashSet<EntityKey> Outgoing = new();

        public Node()
        {
        }
    }
    #endregion
}