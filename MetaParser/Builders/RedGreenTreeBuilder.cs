using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;

using static MetaParser.Core.CodeCommon;

namespace MetaParser.Builders;

internal class RedGreenTreeBuilder : MetaCodeBuilder
{
    public static RedGreenTreeBuilder Instance = new RedGreenTreeBuilder();

    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        var idType = context.Config.IdType;

        writer.WriteLine("#nullable enable");
        writer.Write(@$"
using System.Collections;
using System.Collections.Generic;

public abstract record GreenNode
{{
    public {TokenEnum} Id {{ get; }}
    public int Width {{ get; }}

    protected GreenNode({TokenEnum} id, int width)
    {{
        Id = id;
        Width = width;
    }}

    protected GreenNode({idType} id, int width)
    {{
        Id = ({TokenEnum})id;
        Width = width;
    }}


    public abstract bool TryReplaceChild(GreenNode oldChild, GreenNode newChild, out GreenNode? result);
}}

public class RedNode : IEnumerable<RedNode>
{{
    public GreenNode Green {{ get; }}
    public int Position {{ get; set; }}
    public RedNode? Parent {{ get; set; }}

    public {TokenEnum} Id => Green.Id;
    public int Width => Green.Width;

    public RedNode(GreenNode green, int position = 0, RedNode? parent = null)
    {{
        Green = green;
        Position = position;
        Parent = parent;
    }}

    public int Length
    {{
        get
        {{
            if (Green is {SyntaxTokenNodeType} greenSyntaxToken)
            {{
                return greenSyntaxToken.Children.Length;
            }}

            return 0;
        }}
    }}

    public IEnumerable<RedNode> Children
    {{
        get
        {{
            if (Green is {SyntaxTokenNodeType} greenSyntaxToken)
            {{
                foreach (var child in greenSyntaxToken.Children)
                {{
                    yield return new RedNode(child, Position + child.Width, this);
                }}
            }}
        }}
    }}

    public bool TryReplaceChild(RedNode oldChild, RedNode newChild)
    {{
        if (oldChild is null || newChild is null) return false;
        if (Green.TryReplaceChild(oldChild.Green, newChild.Green, out _))
        {{
            newChild.Parent = this;
            return true;
        }}

        return false;
    }}

    #region IEnumerator
    public IEnumerator<RedNode> GetEnumerator() => Children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}}

public class RedGreenTree : IEnumerable<RedNode>
{{
    #region Properties
    public RedNode Root {{ get; private set; }}
    public int Length => Root.Length;
    #endregion

    public RedGreenTree(GreenNode rootGreen)
    {{
        Root = new RedNode(rootGreen);
    }}

    #region Methods
    public bool TryReplaceNode(RedNode nodeToReplace, GreenNode newGreenNode)
    {{
        if (nodeToReplace is null || newGreenNode is null) return false;

        RedNode newNode = new RedNode(newGreenNode);

        if (nodeToReplace.Parent is not null)
        {{
            if (nodeToReplace.Parent.TryReplaceChild(nodeToReplace, newNode))
            {{
                newNode.Parent = nodeToReplace.Parent;
                return true;
            }}
        }}
        else
        {{
            Root = newNode;
            return true;
        }}

        return false;
    }}
    #endregion

    #region IEnumerator
    public IEnumerator<RedNode> GetEnumerator() => Root.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}}

");

        writer.WriteLine("#nullable restore");
    }
}