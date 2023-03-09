namespace MetaParser.Graphs;
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
