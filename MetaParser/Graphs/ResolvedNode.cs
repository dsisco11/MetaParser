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
    public NodeDepth TreeDepth { get; private set; } = NodeDepth.Default;
    public NodeDepth NodeDepth { get; private set; } = NodeDepth.Default;
    public int Order { get; set; }
    public bool IsRecursive { get; set; }
    public ImmutableHashSet<ResolvedNode> Incoming { get; set; }
    public ImmutableHashSet<ResolvedNode> Outgoing { get; set; }
    #endregion

    #region Constructors
    private ResolvedNode()
    {
        Key = EntityKey.Default;
        Order = int.MaxValue;
        Incoming = ImmutableHashSet<ResolvedNode>.Empty;
        Outgoing = ImmutableHashSet<ResolvedNode>.Empty;
    }

    public ResolvedNode(EntityKey key)
    {
        Key = key;
    }
    #endregion

    public void Zero_Tree_Depth()
    {
        TreeDepth = new NodeDepth(int.MaxValue, int.MinValue);
    }

    public void Zero_Node_Depth()
    {
        NodeDepth = new NodeDepth(int.MaxValue, int.MinValue);
    }

    public void Set_Minimum_Depth(int value)
    {
        TreeDepth = TreeDepth with { Min = value };
        NodeDepth = NodeDepth with { Min = value };
    }

    public void Update_Depth(ResolvedNode ancestorNode)
    {
        TreeDepth = TreeDepth with
        {
            Min = Math.Min(TreeDepth.Min, ancestorNode.TreeDepth.Min + 1),
            Max = Math.Max(TreeDepth.Max, ancestorNode.TreeDepth.Max + 1)
        };

        if (Key.Type == ancestorNode.Key.Type)
        {
            NodeDepth = NodeDepth with
            {
                Min = Math.Min(NodeDepth.Min, ancestorNode.NodeDepth.Min + 1),
                Max = Math.Max(NodeDepth.Max, ancestorNode.NodeDepth.Max + 1)
            };
        }
    }

    public override string ToString()
    {
        return $"{Key} | Order: {Order} | IsRecursive: {IsRecursive} | TreeDepth: {TreeDepth} | NodeDepth: {NodeDepth}";
    }
}
