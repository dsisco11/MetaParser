using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;

using System;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal abstract record GraphableEntity : IGraphableEntity, IComparable<GraphableEntity>
{
    #region Fields
    private readonly EntityKey _key;
    private readonly WeakReference<MetaParserRegistry> _registry;
    #endregion

    #region Properties
    public ResolvedNode DependencyInfo { get; set; } = ResolvedNode.Default;
    #endregion

    #region Accessors
    public EntityKey NodeID => _key;
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
    public GraphableEntity(EntityKey key, MetaParserContext context)
    {
        _key = key;
        _registry = new WeakReference<MetaParserRegistry>(context.Registry);
    }
    #endregion

    public int CompareTo(GraphableEntity other)
    {
        return _key.CompareTo(other._key);
    }

    public abstract IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry);
}
