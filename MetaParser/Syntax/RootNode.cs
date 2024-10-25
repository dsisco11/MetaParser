namespace MetaParser.Syntax;

internal sealed record RootNode : GreenNode
{
    public RootNode() : base() { }
    public RootNode(GreenNode[] children) : base(children) { }
}