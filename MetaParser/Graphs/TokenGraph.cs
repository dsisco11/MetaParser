using System.Collections.Generic;

namespace MetaParser.Graphs;

internal class TokenGraph : DirectedGraph<NodeKey>
{
    public TokenGraph(TokenGraph other) : base(other.Nodes.Keys)
    {
        foreach (var node in other.Nodes)
        { 
            Nodes[node.Key].Incoming.UnionWith(node.Value.Incoming);
            Nodes[node.Key].Outgoing.UnionWith(node.Value.Outgoing);
        }
    }

    public TokenGraph(IEnumerable<NodeKey> items) : base(items)
    {
    }
}
