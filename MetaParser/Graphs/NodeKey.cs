using System;
using System.Diagnostics;

namespace MetaParser.Graphs;

[DebuggerDisplay(@"{Type}<{Index}>{ParentString}")]
internal record NodeKey : IComparable<NodeKey>
{
    #region Statics
    public static readonly NodeKey Default = new(NodeType.None, 0);
    #endregion

    #region Fields
    public readonly NodeKey? Parent;
    public readonly NodeType Type;
    public readonly int Index;
    #endregion

    #region Properties
    #endregion

    #region Accessors
    private string ParentString => Parent is not null ? $"({Parent?.ToString()})" : string.Empty;

    #endregion

    #region Constructors
    public NodeKey(NodeType type, int index, NodeKey? parent = null)
    {
        Type = type;
        Index = index;
        Parent = parent;
    }
    #endregion

    public int CompareTo(NodeKey other)
    {
        // compare type and index
        int result = Type.CompareTo(other.Type);
        if (result == 0)
        {
            result = Index.CompareTo(other.Index);
        }

        // if we are still equal, compare parent
        if (result == 0 && Parent is not null && other.Parent is not null)
        {
            result = Parent.CompareTo(other.Parent);
        }

        return result;
    }

    public override string ToString()
    {
        return $"{Type}_{Index}";
    }
}
