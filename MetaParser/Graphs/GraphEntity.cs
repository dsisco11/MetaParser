using MetaParser.Core;
using MetaParser.Exceptions;
using MetaParser.Graphs;
using MetaParser.Trees;

using System;
using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs;

internal abstract record GraphEntity : IGraphEntity, IComparable<GraphEntity>
{
    #region Fields
    private readonly EntityKey _key;
    private readonly WeakReference<EntityRegistry> _registry;
    #endregion

    #region Properties
    public NodeData GraphInfo { get; set; } = NodeData.Default;
    #endregion

    #region Accessors
    public KeyTreeNode<EntityKey> HierarchyNode => Registry.Tree[Key] ?? throw new MetaParserException($"Cannot find hierarchy node for '{Key}'");
    public EntityKey Key => _key;
    public EntityRegistry Registry
    {
        get
        {
            if (!_registry.TryGetTarget(out var registry))
            {
                throw new MetaParserException($"Cannot resolve registry for graph entity: {Key}");
            }

            return registry;
        }
    }
    
    #endregion

    #region Constructors
    public GraphEntity(NodeType nodeType, EntityRegistry registry)
    {
        _key = new EntityKey(nodeType, registry);
        _registry = new WeakReference<EntityRegistry>(registry);
    }
    #endregion

    public int CompareTo(GraphEntity other)
    {
        return _key.CompareTo(other._key);
    }

    public virtual IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry) { yield break; }

    public override string ToString() => _key.ToString();
}
