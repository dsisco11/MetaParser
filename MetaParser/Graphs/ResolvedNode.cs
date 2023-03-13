using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Graphs;

[DebuggerDisplay("{this.ToString()}", Name = "{Key}")]
internal record ResolvedNode
{
    #region Static
    public static readonly ResolvedNode Default = new ResolvedNode();
    static readonly int MaxTypeIndex = 1+Enum.GetValues(typeof(NodeType)).Cast<int>().Max();
    #endregion

    #region Fields
    public readonly NodeKey Key;
    [DebuggerDisplay("{DebugDepth}")]
    public readonly NodeDepth[] Depth = Array.Empty<NodeDepth>();
    public int Order { get; set; }
    public bool IsRecursive { get; set; }
    public ImmutableHashSet<ResolvedNode> Incoming { get; set; }
    public ImmutableHashSet<ResolvedNode> Outgoing { get; set; }
    #endregion

    #region Accessors
    private string DebugDepth => string.Join(", ", Depth.Select(static (d, i) => $"{Enum.GetName(typeof(NodeType), i).ToUpperInvariant()[0]}: [{d.Min}, {d.Max}]"));
    #endregion

    #region Costructors
    private ResolvedNode()
    {
        Key = NodeKey.Default;
        Order = int.MaxValue;
        Incoming = ImmutableHashSet<ResolvedNode>.Empty;
        Outgoing = ImmutableHashSet<ResolvedNode>.Empty;
    }

    public ResolvedNode(NodeKey key)
    {
        Key = key;
        Depth = new NodeDepth[MaxTypeIndex];
    }
    #endregion


    public void Zero_Depth(NodeType nodeType)
    {
        int type = (int)nodeType;
        Depth[type] = new NodeDepth(int.MaxValue, int.MinValue);
    }

    public void Set_Minimum_Depth(int value)
    {
        for (int i = 0; i < Depth.Length; i++)
        {
            Depth[i] = Depth[i] with { Min = value };
        }
    }

    public void Update_Depth(ResolvedNode ancestorNode)
    {
        Accumulate_Depth(NodeType.None, ancestorNode);
        Accumulate_Depth(ancestorNode.Key.Type, ancestorNode);
    }

    private void Accumulate_Depth(NodeType nodeType, ResolvedNode ancestorNode)
    {
        int type = (int)nodeType;
        var depth = Depth[type];
        var ancestorDepth = ancestorNode.Depth[type];
        Depth[type] = depth with {
            Min = Math.Min(depth.Min, ancestorDepth.Min + 1),
            Max = Math.Max(depth.Max, ancestorDepth.Max + 1)
        };
    }

    public override string ToString()
    {
        return $"{Key} | Order[{Order}] | IsRecursive ({IsRecursive}) | Depth[{DebugDepth}]";
    }
}
