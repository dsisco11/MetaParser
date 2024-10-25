using MetaParser.Visitors;

namespace MetaParser.Syntax.Nodes;

internal abstract record ParentNode : GreenNode
{
    public ParentNode(GreenNode[] children) : base(children) { }

    public override void Accept(RedNode red, IAstVisitor visitor)
    {
        visitor.Visit(red);
    }

}
