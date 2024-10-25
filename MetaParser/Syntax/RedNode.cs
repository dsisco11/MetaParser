using System;
using System.Collections;
using System.Collections.Generic;

namespace MetaParser.Syntax;

public record RedNode : IEnumerable<RedNode>
{
    #region Fields
    internal readonly GreenNode Green;
    public readonly int Position;
    public readonly RedNode? Parent;
    #endregion

    #region Constructors
    internal RedNode(GreenNode green, int position = 0, RedNode? parent = null)
    {
        Green = green;
        Position = position;
        Parent = parent;
    }
    #endregion

    public IEnumerable<RedNode> Children
    {
        get
        {
            if (Green is GreenNode greenNode)
            {
                var pos = Position;
                foreach (var child in greenNode.Children)
                {
                    yield return child.GetRed(pos, this);
                    pos += child.Width;
                }
            }
        }
    }

    #region IEnumerator
    public IEnumerator<RedNode> GetEnumerator() => Children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}
