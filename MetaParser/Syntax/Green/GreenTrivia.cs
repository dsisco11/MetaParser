using System;
using System.IO;

namespace MetaParser.Syntax.Green;

/// <summary>
/// Represents trivia (whitespace, comments, etc.) in the green tree.
/// Trivia is backed by ReadOnlyMemory&lt;char&gt; for efficient slicing without allocation.
/// </summary>
public sealed class GreenTrivia : GreenNode
{
    /// <summary>
    /// The trivia text, stored as ReadOnlyMemory for zero-copy slicing.
    /// </summary>
    private readonly ReadOnlyMemory<char> _text;
    
    /// <summary>
    /// Optional owned string for when we need to own the data.
    /// </summary>
    private readonly string? _ownedText;
    
    /// <summary>
    /// Gets the trivia text as a span (no allocation).
    /// </summary>
    public ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;
    
    /// <summary>
    /// Creates trivia backed by a memory slice (zero-copy).
    /// </summary>
    public GreenTrivia(ushort kind, ReadOnlyMemory<char> text)
        : base(kind, text.Length, GreenNodeFlags.IsTrivia)
    {
        _text = text;
        _ownedText = null;
    }
    
    /// <summary>
    /// Creates trivia backed by an owned string.
    /// </summary>
    public GreenTrivia(ushort kind, string text)
        : base(kind, text.Length, GreenNodeFlags.IsTrivia)
    {
        _text = default;
        _ownedText = text;
    }
    
    /// <summary>
    /// Trivia has no children.
    /// </summary>
    public override GreenNode? GetSlot(int index) => null;
    
    /// <summary>
    /// Writes the trivia text to the writer.
    /// </summary>
    public override void WriteTo(TextWriter writer)
    {
        if (_ownedText is not null)
        {
            writer.Write(_ownedText);
        }
        else
        {
#if NET8_0_OR_GREATER
            writer.Write(_text.Span);
#else
            writer.Write(_text.ToString());
#endif
        }
    }
    
    /// <summary>
    /// Creates an owned copy if backed by memory slice.
    /// </summary>
    public override GreenNode ToOwned()
    {
        if (_ownedText is not null)
            return this; // Already owned
        
        return new GreenTrivia(RawKind, _text.ToString());
    }
    
    /// <summary>
    /// Gets the text as a string (may allocate if memory-backed).
    /// </summary>
    public string GetText()
    {
        return _ownedText ?? _text.ToString();
    }
    
    protected override string FormatDebugString()
    {
        var text = GetText();
        var escaped = text.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
        if (escaped.Length > 20)
            escaped = escaped.Substring(0, 17) + "...";
        return $"Trivia[Kind={RawKind}, \"{escaped}\"]";
    }
}

/// <summary>
/// A list of trivia nodes, stored as a single green node.
/// Used when a token has multiple trivia items (e.g., whitespace + comment).
/// </summary>
public sealed class GreenTriviaList : GreenNode
{
    private readonly GreenTrivia[] _trivia;
    
    public GreenTriviaList(ushort kind, GreenTrivia[] trivia)
        : base(kind, ComputeWidth(trivia), GreenNodeFlags.IsTrivia | GreenNodeFlags.ContainsTrivia, (byte)Math.Min(trivia.Length, 255))
    {
        _trivia = trivia;
    }
    
    /// <summary>
    /// Gets the number of trivia items.
    /// </summary>
    public int Count => _trivia.Length;
    
    /// <summary>
    /// Gets the trivia at the specified index.
    /// </summary>
    public GreenTrivia this[int index] => _trivia[index];
    
    public override GreenNode? GetSlot(int index)
    {
        if (index >= 0 && index < _trivia.Length)
            return _trivia[index];
        return null;
    }
    
    public override void WriteTo(TextWriter writer)
    {
        foreach (var trivia in _trivia)
        {
            trivia.WriteTo(writer);
        }
    }
    
    public override GreenNode ToOwned()
    {
        var owned = new GreenTrivia[_trivia.Length];
        for (int i = 0; i < _trivia.Length; i++)
        {
            owned[i] = (GreenTrivia)_trivia[i].ToOwned();
        }
        return new GreenTriviaList(RawKind, owned);
    }
    
    private static int ComputeWidth(GreenTrivia[] trivia)
    {
        int width = 0;
        foreach (var t in trivia)
            width += t.FullWidth;
        return width;
    }
    
    protected override string FormatDebugString()
    {
        return $"TriviaList[Count={_trivia.Length}, Width={FullWidth}]";
    }
}
