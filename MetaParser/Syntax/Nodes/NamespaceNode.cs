namespace MetaParser.Syntax.Nodes;

internal record NamespaceNode : ParentNode
{
    public readonly string Name;

    public NamespaceNode(string name, GreenNode[] children) : base(children)
    {
        Name = name;
    }
}
