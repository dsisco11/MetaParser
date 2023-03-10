using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs;
using static DirectedGraph<GraphNodeKey>;

internal record TokenInfo : IComparable<TokenInfo>
{
    #region Fields
    public readonly string Name;
    public readonly GraphNodeKey NodeID;
    private readonly WeakReference<MetaParserRegistry> _registry;
    #endregion

    #region Dependency Info
    public ResolvedNode? DependencyInfo { get; set; }
    #endregion

    #region Accessors
    public int Index => NodeID.Index;

    public IEnumerable<Consumer> GetConsumers()
    {
        if (_registry.TryGetTarget(out MetaParserRegistry registry))
        {
            return DependencyInfo!.Incoming
                .Where(c => c.Key.Type == GraphNodeType.Consumer && c.Key.Parent == NodeID)
                .Select(c => registry.Consumers[c.Key]);
        }
        else
        {
            throw new MetaParserException($"Cannot resolve consumers for token without registry: {NodeID}");
        }
    }
    #endregion

    #region Constructors
    public TokenInfo(string name, MetaParserContext context)
    {
        _registry = new WeakReference<MetaParserRegistry>(context.Registry);
        Name = name;
        NodeID = new GraphNodeKey(GraphNodeType.Token, context.Registry.GetNextTokenIndex());
    }
    #endregion

    #region IComparable
    public int CompareTo(TokenInfo other)
    {
        return NodeID.Index.CompareTo(other.NodeID.Index);
    }
    #endregion
}
