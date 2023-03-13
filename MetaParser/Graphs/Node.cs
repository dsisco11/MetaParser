using System.Collections.Generic;
using System.Diagnostics;

namespace MetaParser.Graphs;

internal partial class DirectedGraph
{
    #region Records
    [DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
    public record Node
    {
        public readonly HashSet<NodeKey> Incoming = new();
        public readonly HashSet<NodeKey> Outgoing = new();
    }
    #endregion
}