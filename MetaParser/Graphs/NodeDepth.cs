namespace MetaParser.Graphs;

internal record struct NodeDepth(int Min, int Max)
{
    public static readonly NodeDepth Zero = new(0, 0);
    public override string ToString()
    {
        return $"[{Min}, {Max}]";
    }
}