using System;
using System.Linq;

namespace MetaParser.Syntax;

public abstract record GreenNode
{
    #region Fields
    public int Width { get; protected set; }
    public GreenNode[] Children { get; protected set; }
    #endregion

    #region Constructors
    public GreenNode()
    {
        Width = 1;
        Children = Array.Empty<GreenNode>();
    }

    public GreenNode(GreenNode[] children)
    {
        Children = children;
        Width = children.Sum(static child => child.Width);
    }
    #endregion

    #region Methods
    internal GreenNode Mutate(GreenNode[] newChildren)
    {
        return this with
        {
            Children = newChildren,
            Width = newChildren.Sum(static child => child.Width)
        };
    }

    public RedNode GetRed(int position, RedNode? parent) => new(this, position, parent);
    public GreenNodeBuilder CreateBuilder() => new(this);
    #endregion
}
