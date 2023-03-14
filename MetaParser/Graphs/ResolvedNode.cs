using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MetaParser.Graphs;

[DebuggerDisplay("{this.ToString()}", Name = "{Key}")]
internal record ResolvedNode
{
    #region Static
    public static readonly ResolvedNode Default = new ResolvedNode();
    #endregion

    #region Fields
    public readonly EntityKey Key;
    public NodeDepth TreeDepth { get; private set; }
    public NodeDepth NodeDepth { get; private set; }
    public int Order { get; set; }
    public bool IsRecursive { get; set; }
    public ImmutableHashSet<EntityKey> Incoming { get; set; }
    public ImmutableHashSet<EntityKey> Outgoing { get; set; }
    #endregion

    #region Constructors
    private ResolvedNode()
    {
        Key = EntityKey.Default;
        Incoming = ImmutableHashSet<EntityKey>.Empty;
        Outgoing = ImmutableHashSet<EntityKey>.Empty;
    }

    public ResolvedNode(EntityKey key, ImmutableHashSet<EntityKey> incoming, ImmutableHashSet<EntityKey> outgoing, int order, bool isRecursive, NodeDepth? treeDepth, NodeDepth? nodeDepth)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Incoming = incoming;
        Outgoing = outgoing;

        Order = order;
        IsRecursive = isRecursive;
        TreeDepth = treeDepth ?? NodeDepth.Zero;
        NodeDepth = nodeDepth ?? NodeDepth.Zero;
    }
    #endregion

    public override string ToString()
    {
        return $"{Key} | Order: {Order} | IsRecursive: {IsRecursive} | TreeDepth: {TreeDepth} | NodeDepth: {NodeDepth}";
    }
}
