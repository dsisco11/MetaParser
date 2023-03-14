namespace MetaParser.Graphs;

internal record struct NodeDepth(int Min = 0, int Max = 0)
{
    public static readonly NodeDepth Default = new(0, 0);
    public override string ToString()
    {
        return $"[{Min}, {Max}]";
    }
}