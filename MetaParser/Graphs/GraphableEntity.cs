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
    private readonly WeakReference<TokenRegistry> _registry;
    #endregion

    #region Properties
    public ResolvedNode DependencyInfo { get; set; } = ResolvedNode.Default;
    #endregion

    #region Accessors
    public EntityKey Key => _key;
    public TokenRegistry Registry
    {
        get
        {
            if (!_registry.TryGetTarget(out var registry))
            {
                throw new MetaParserException($"Cannot resolve registry for graphable entity: {Key}");
            }

            return registry;
        }
    }
    
    #endregion

    #region Constructors
    public GraphableEntity(EntityKey key, ParserContext context)
    {
        _key = key;
        _registry = new WeakReference<TokenRegistry>(context.Registry);
    }
    #endregion

    public int CompareTo(GraphableEntity other)
    {
        return _key.CompareTo(other._key);
    }

    public abstract IEnumerable<EntityLink> ResolveLinks(TokenRegistry Registry);

    public override string ToString()
    {
        return _key.ToString();
    }
}
