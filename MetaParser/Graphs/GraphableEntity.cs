using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;

using System;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal abstract record GraphableEntity : IGraphableEntity, IComparable<GraphableEntity>
{
    #region Fields
    private readonly NodeKey _nodeID;
    private readonly WeakReference<MetaParserRegistry> _registry;
    #endregion

    #region Properties
    public ResolvedNode DependencyInfo { get; set; } = ResolvedNode.Default;
    #endregion

    #region Accessors
    public NodeKey NodeID => _nodeID;
    public MetaParserRegistry Registry
    {
        get
        {
            if (!_registry.TryGetTarget(out var registry))
            {
                throw new MetaParserException($"Cannot resolve registry for graphable entity: {NodeID}");
            }

            return registry;
        }
    }
    
    #endregion

    #region Constructors
    public GraphableEntity(NodeKey key, MetaParserContext context)
    {
        _nodeID = key;
        _registry = new WeakReference<MetaParserRegistry>(context.Registry);
    }
    #endregion

    public int CompareTo(GraphableEntity other)
    {
        return _nodeID.CompareTo(other._nodeID);
    }

    public abstract IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry);
}
