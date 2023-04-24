//HintName: MetaParser.MetaParser.parser.RedGreenTree.g.cs
namespace UnitTestParser;
#nullable enable

using System.Collections;
using System.Collections.Generic;

public abstract record GreenNode
{
    public ETokenType Id { get; }
    public int Width { get; }

    protected GreenNode(ETokenType id, int width)
    {
        Id = id;
        Width = width;
    }

    protected GreenNode(byte id, int width)
    {
        Id = (ETokenType)id;
        Width = width;
    }


    public abstract bool TryReplaceChild(GreenNode oldChild, GreenNode newChild, out GreenNode? result);
}

public class RedNode : IEnumerable<RedNode>
{
    public GreenNode Green { get; }
    public int Position { get; set; }
    public RedNode? Parent { get; set; }

    public ETokenType Id => Green.Id;
    public int Width => Green.Width;

    public RedNode(GreenNode green, int position = 0, RedNode? parent = null)
    {
        Green = green;
        Position = position;
        Parent = parent;
    }

    public int Length
    {
        get
        {
            if (Green is SyntaxNode greenSyntaxToken)
            {
                return greenSyntaxToken.Children.Length;
            }

            return 0;
        }
    }

    public IEnumerable<RedNode> Children
    {
        get
        {
            if (Green is SyntaxNode greenSyntaxToken)
            {
                foreach (var child in greenSyntaxToken.Children)
                {
                    yield return new RedNode(child, Position + child.Width, this);
                }
            }
        }
    }

    public bool TryReplaceChild(RedNode oldChild, RedNode newChild)
    {
        if (oldChild is null || newChild is null) return false;
        if (Green.TryReplaceChild(oldChild.Green, newChild.Green, out _))
        {
            newChild.Parent = this;
            return true;
        }

        return false;
    }

    #region IEnumerator
    public IEnumerator<RedNode> GetEnumerator() => Children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}

public class RedGreenTree : IEnumerable<RedNode>
{
    #region Properties
    public RedNode Root { get; private set; }
    public int Length => Root.Length;
    #endregion

    public RedGreenTree(GreenNode rootGreen)
    {
        Root = new RedNode(rootGreen);
    }

    #region Methods
    public bool TryReplaceNode(RedNode nodeToReplace, GreenNode newGreenNode)
    {
        if (nodeToReplace is null || newGreenNode is null) return false;

        RedNode newNode = new RedNode(newGreenNode);

        if (nodeToReplace.Parent is not null)
        {
            if (nodeToReplace.Parent.TryReplaceChild(nodeToReplace, newNode))
            {
                newNode.Parent = nodeToReplace.Parent;
                return true;
            }
        }
        else
        {
            Root = newNode;
            return true;
        }

        return false;
    }
    #endregion

    #region IEnumerator
    public IEnumerator<RedNode> GetEnumerator() => Root.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}

#nullable restore
