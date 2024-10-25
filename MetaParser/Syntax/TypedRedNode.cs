using MetaParser.Syntax;

namespace MetaParser.Trees;

public record TypedRedNode<T> : RedNode
    where T : GreenNode
{
    #region Accessors
    public T Value => (T)Green;
    #endregion

    #region Constructors
    internal TypedRedNode(RedNode other) : base(other.Green, other.Position, other.Parent)
    {
    }
    #endregion
}