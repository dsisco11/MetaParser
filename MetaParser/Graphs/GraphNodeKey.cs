using System;
using System.Diagnostics;

namespace MetaParser.Graphs;

[DebuggerDisplay("{Type}<{Index}> Parent: [{Parent.Type}:{Parent.Index}]")]
internal record GraphNodeKey : IComparable<GraphNodeKey>
{
    public readonly GraphNodeKey? Parent;
    public readonly GraphNodeType Type;
    public readonly int Index;

    public GraphNodeKey(GraphNodeType type, int index, GraphNodeKey? parent = null)
    {
        Type = type;
        Index = index;
        Parent = parent;
    }

    public int CompareTo(GraphNodeKey other)
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
}
