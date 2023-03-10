using System.Diagnostics;

namespace MetaParser.Graphs;

[DebuggerDisplay("{Type}<{Index}> Parent:{Parent}")]
internal record GraphNodeKey
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
}
