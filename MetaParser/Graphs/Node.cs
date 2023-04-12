using System.Collections.Generic;
using System.Diagnostics;

namespace MetaParser.Graphs;

[DebuggerDisplay(@"[In: {Incoming.Count}] [Out: {Outgoing.Count}]", Name = @"{Id}")]
public sealed record Node<T> where T : notnull
{
    public readonly HashSet<T> Incoming;
    public readonly HashSet<T> Outgoing;

    public Node()
    {
        Incoming = new();
        Outgoing = new();
    }
}
