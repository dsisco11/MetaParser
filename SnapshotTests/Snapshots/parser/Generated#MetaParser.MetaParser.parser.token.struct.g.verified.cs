//HintName: MetaParser.MetaParser.parser.token.struct.g.cs
namespace UnitTestParser;
#nullable enable


public sealed record LexerNode : GreenNode
{
    public global::System.ReadOnlyMemory<char> Value { get; }

    public LexerNode(ETokenType id, global::System.ReadOnlyMemory<char> value) : base(id, value.Length)
    {
        Value = value;
    }

    public LexerNode(byte id, global::System.ReadOnlyMemory<char> value) : base(id, value.Length)
    {
        Value = value;
    }

    public override bool TryReplaceChild(GreenNode oldChild, GreenNode newChild, out GreenNode? result)
    {
        throw new InvalidOperationException("Cannot replace a child of a lexer token");
    }
}

public sealed record SyntaxNode : GreenNode
{
    public GreenNode[] Children { get; }

    public SyntaxNode(byte id, GreenNode[] children) : base(id, children.Sum(child => child.Width))
    {
        Children = children;
    }

    public SyntaxNode(ETokenType id, GreenNode[] children) : base(id, children.Sum(child => child.Width))
    {
        Children = children;
    }

    public override bool TryReplaceChild(GreenNode oldChild, GreenNode newChild, out GreenNode? result)
    {
        if (oldChild is null || newChild is null)
        {
            result = null;
            return false;
        }

        var index = Array.IndexOf(Children, oldChild);

        if (index >= 0)
        {
            var newChildren = (GreenNode[])Children.Clone();
            newChildren[index] = newChild;

            result = new SyntaxNode(Id, newChildren);
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}

