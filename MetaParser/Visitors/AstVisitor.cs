using MetaParser.Syntax;

namespace MetaParser.Visitors;

internal abstract class AstVisitor : IAstVisitor
{
    public SyntaxTree Tree { get; protected set; }

    public AstVisitor(SyntaxTree tree)
    {
        Tree = tree;
    }

    public abstract void Visit(RedNode node);
}

