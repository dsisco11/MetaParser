namespace MetaParser.Syntax;

public record struct SyntaxTree
{
    private readonly GreenNode root;
    public RedNode Root => root.GetRed(0, null);

    #region Constructors
    internal SyntaxTree(GreenNode rootGreen)
    {
        root = rootGreen;
    }

    public SyntaxTree() : this(new RootNode())
    {
    }

    internal SyntaxTree(GreenNode[] rootItems) : this(new RootNode(rootItems))
    {
    }
    #endregion

    public SyntaxTreeBuilder CreateBuilder() => new(root);
}