namespace MetaParser.Syntax.Nodes;

internal record ClassNode : ParentNode
{
    public readonly string Name;

    public ClassNode(string name, GreenNode[] children) : base(children)
    {
        Name = name;
    }
}
