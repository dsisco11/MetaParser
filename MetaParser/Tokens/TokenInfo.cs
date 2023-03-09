using MetaParser.Consumers;
using MetaParser.Graphs;

using System.Collections.Generic;

namespace MetaParser.Tokens;
using static DirectedGraph<GraphNodeKey>;

internal record TokenInfo
{
    #region Fields
    public readonly int Index;
    public readonly string Name;
    public readonly List<TokenConsumer> Consumers;
    public readonly GraphNodeKey NodeID;
    public ResolvedNode DependencyInfo { get; set; }
    #endregion

    public TokenInfo(int index, string name)
    {
        Index = index;
        Name = name;
        Consumers = new();
        NodeID = new GraphNodeKey(GraphNodeType.Token, Index);
    }
}
