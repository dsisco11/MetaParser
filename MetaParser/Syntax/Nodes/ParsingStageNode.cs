namespace MetaParser.Syntax.Nodes;

internal record ParsingStageNode : ParentNode
{
    public readonly int Depth;

    public ParsingStageNode(int depth, GreenNode[] children) : base(children)
    {
        Depth = depth;
    }
}