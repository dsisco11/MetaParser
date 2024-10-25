namespace MetaParser.Syntax.Nodes;

internal record ParserDefinitionNode : ParentNode
{
    public readonly string Name;
    public readonly string Namespace;

    public ParserDefinitionNode(string name, string @namespace, GreenNode[] children) : base(children)
    {
        Name = name;
        Namespace = @namespace;
    }
}
