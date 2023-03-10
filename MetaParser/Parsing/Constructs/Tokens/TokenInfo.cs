using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs.Consumers;
using MetaParser.Parsing.Constructs.Core;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Parsing.Constructs.Tokens;
using static DirectedGraph<GraphNodeKey>;

internal record TokenInfo
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

    public TokenInfo(string name, MetaParserContext context)
    {
        _registry = new WeakReference<MetaParserRegistry>(context.Registry);
        Name = name;
        NodeID = new GraphNodeKey(GraphNodeType.Token, context.Registry.GetNextTokenIndex());
    }
}
