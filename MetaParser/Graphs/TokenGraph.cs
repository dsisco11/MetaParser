using System.Collections.Generic;

namespace MetaParser.Graphs;

internal class TokenGraph : DirectedGraph<NodeKey>
{
    public TokenGraph(IEnumerable<NodeKey> items) : base(items)
    {
    }
}
