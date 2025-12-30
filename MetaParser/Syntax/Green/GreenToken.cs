using System;
using System.IO;

namespace MetaParser.Syntax.Green;

/// <summary>
/// Base class for all green tokens.
/// Tokens are leaf nodes that contain actual text from the source.
/// </summary>
public abstract class GreenToken : GreenNode
{
    /// <summary>
    /// Creates a new green token.
    /// </summary>
    protected GreenToken(ushort kind, int fullWidth, GreenNodeFlags flags = GreenNodeFlags.None)
        : base(kind, fullWidth, flags | GreenNodeFlags.IsToken, slotCount: 0)
    {
    }
    
    /// <summary>
    /// Gets the token text (excluding trivia).
    /// </summary>
    public abstract ReadOnlySpan<char> Text { get; }
    
    /// <summary>
    /// Gets the token text as a string (may allocate).
    /// </summary>
    public abstract string GetText();
    
    /// <summary>
    /// Tokens have no child slots.
    /// </summary>
    public sealed override GreenNode? GetSlot(int index) => null;
}

/// <summary>
/// A token with no trivia (most common case for keywords/operators).
/// Optimized for minimal memory usage.
/// </summary>
public sealed class GreenTokenWithNoTrivia : GreenToken
{
    private readonly ReadOnlyMemory<char> _text;
    private readonly string? _ownedText;
    
    public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;
    
    public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;
    
    /// <summary>
    /// Creates a token backed by a memory slice (zero-copy).
    /// </summary>
    public GreenTokenWithNoTrivia(ushort kind, ReadOnlyMemory<char> text)
        : base(kind, text.Length)
    {
        _text = text;
        _ownedText = null;
    }
    
    /// <summary>
    /// Creates a token backed by an owned string.
    /// </summary>
    public GreenTokenWithNoTrivia(ushort kind, string text)
        : base(kind, text.Length)
    {
        _text = default;
        _ownedText = text;
    }
    
    public override string GetText() => _ownedText ?? _text.ToString();
    
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
    
    protected override void WriteCoreTo(TextWriter writer) => WriteTo(writer);
    
    public override GreenNode ToOwned()
    {
        if (_ownedText is not null)
            return this;
        return new GreenTokenWithNoTrivia(RawKind, _text.ToString());
    }
    
    protected override string FormatDebugString()
    {
        var text = GetText();
        if (text.Length > 20)
            text = text.Substring(0, 17) + "...";
        return $"Token[Kind={RawKind}, \"{text}\"]";
    }
}

/// <summary>
/// A token with only leading trivia.
/// </summary>
public sealed class GreenTokenWithLeadingTrivia : GreenToken
{
    private readonly GreenNode _leadingTrivia;
    private readonly ReadOnlyMemory<char> _text;
    private readonly string? _ownedText;
    
    public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;
    public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;
    public override int LeadingTriviaWidth => _leadingTrivia.FullWidth;
    public override GreenNode? LeadingTrivia => _leadingTrivia;
    
    public GreenTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text)
        : base(kind, leadingTrivia.FullWidth + text.Length, GreenNodeFlags.ContainsTrivia)
    {
        _leadingTrivia = leadingTrivia;
        _text = text;
        _ownedText = null;
    }
    
    public GreenTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, string text)
        : base(kind, leadingTrivia.FullWidth + text.Length, GreenNodeFlags.ContainsTrivia)
    {
        _leadingTrivia = leadingTrivia;
        _text = default;
        _ownedText = text;
    }
    
    public override string GetText() => _ownedText ?? _text.ToString();
    
    public override void WriteTo(TextWriter writer)
    {
        _leadingTrivia.WriteTo(writer);
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
    
    protected override void WriteCoreTo(TextWriter writer)
    {
        if (_ownedText is not null)
            writer.Write(_ownedText);
        else
            writer.Write(_text.ToString());
    }
    
    public override GreenNode ToOwned()
    {
        if (_ownedText is not null && _leadingTrivia.ToOwned() == _leadingTrivia)
            return this;
        return new GreenTokenWithLeadingTrivia(RawKind, _leadingTrivia.ToOwned(), _ownedText ?? _text.ToString());
    }
    
    protected override string FormatDebugString()
    {
        var text = GetText();
        if (text.Length > 20)
            text = text.Substring(0, 17) + "...";
        return $"Token[Kind={RawKind}, \"{text}\", Leading={LeadingTriviaWidth}]";
    }
}

/// <summary>
/// A token with only trailing trivia.
/// </summary>
public sealed class GreenTokenWithTrailingTrivia : GreenToken
{
    private readonly ReadOnlyMemory<char> _text;
    private readonly string? _ownedText;
    private readonly GreenNode _trailingTrivia;
    
    public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;
    public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;
    public override int TrailingTriviaWidth => _trailingTrivia.FullWidth;
    public override GreenNode? TrailingTrivia => _trailingTrivia;
    
    public GreenTokenWithTrailingTrivia(ushort kind, ReadOnlyMemory<char> text, GreenNode trailingTrivia)
        : base(kind, text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)
    {
        _text = text;
        _ownedText = null;
        _trailingTrivia = trailingTrivia;
    }
    
    public GreenTokenWithTrailingTrivia(ushort kind, string text, GreenNode trailingTrivia)
        : base(kind, text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)
    {
        _text = default;
        _ownedText = text;
        _trailingTrivia = trailingTrivia;
    }
    
    public override string GetText() => _ownedText ?? _text.ToString();
    
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
        _trailingTrivia.WriteTo(writer);
    }
    
    protected override void WriteCoreTo(TextWriter writer)
    {
        if (_ownedText is not null)
            writer.Write(_ownedText);
        else
            writer.Write(_text.ToString());
    }
    
    public override GreenNode ToOwned()
    {
        if (_ownedText is not null && _trailingTrivia.ToOwned() == _trailingTrivia)
            return this;
        return new GreenTokenWithTrailingTrivia(RawKind, _ownedText ?? _text.ToString(), _trailingTrivia.ToOwned());
    }
    
    protected override string FormatDebugString()
    {
        var text = GetText();
        if (text.Length > 20)
            text = text.Substring(0, 17) + "...";
        return $"Token[Kind={RawKind}, \"{text}\", Trailing={TrailingTriviaWidth}]";
    }
}

/// <summary>
/// A token with both leading and trailing trivia.
/// </summary>
public sealed class GreenTokenWithTrivia : GreenToken
{
    private readonly GreenNode _leadingTrivia;
    private readonly ReadOnlyMemory<char> _text;
    private readonly string? _ownedText;
    private readonly GreenNode _trailingTrivia;
    
    public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;
    public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;
    public override int LeadingTriviaWidth => _leadingTrivia.FullWidth;
    public override int TrailingTriviaWidth => _trailingTrivia.FullWidth;
    public override GreenNode? LeadingTrivia => _leadingTrivia;
    public override GreenNode? TrailingTrivia => _trailingTrivia;
    
    public GreenTokenWithTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text, GreenNode trailingTrivia)
        : base(kind, leadingTrivia.FullWidth + text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)
    {
        _leadingTrivia = leadingTrivia;
        _text = text;
        _ownedText = null;
        _trailingTrivia = trailingTrivia;
    }
    
    public GreenTokenWithTrivia(ushort kind, GreenNode leadingTrivia, string text, GreenNode trailingTrivia)
        : base(kind, leadingTrivia.FullWidth + text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)
    {
        _leadingTrivia = leadingTrivia;
        _text = default;
        _ownedText = text;
        _trailingTrivia = trailingTrivia;
    }
    
    public override string GetText() => _ownedText ?? _text.ToString();
    
    public override void WriteTo(TextWriter writer)
    {
        _leadingTrivia.WriteTo(writer);
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
        _trailingTrivia.WriteTo(writer);
    }
    
    protected override void WriteCoreTo(TextWriter writer)
    {
        if (_ownedText is not null)
            writer.Write(_ownedText);
        else
            writer.Write(_text.ToString());
    }
    
    public override GreenNode ToOwned()
    {
        if (_ownedText is not null && _leadingTrivia.ToOwned() == _leadingTrivia && _trailingTrivia.ToOwned() == _trailingTrivia)
            return this;
        return new GreenTokenWithTrivia(RawKind, _leadingTrivia.ToOwned(), _ownedText ?? _text.ToString(), _trailingTrivia.ToOwned());
    }
    
    protected override string FormatDebugString()
    {
        var text = GetText();
        if (text.Length > 20)
            text = text.Substring(0, 17) + "...";
        return $"Token[Kind={RawKind}, \"{text}\", Leading={LeadingTriviaWidth}, Trailing={TrailingTriviaWidth}]";
    }
}

/// <summary>
/// Extension methods for creating tokens with trivia.
/// </summary>
public static class GreenTokenExtensions
{
    /// <summary>
    /// Creates a new token with the specified leading trivia.
    /// </summary>
    public static GreenToken WithLeadingTrivia(this GreenToken token, GreenNode? leadingTrivia)
    {
        if (leadingTrivia is null || leadingTrivia.FullWidth == 0)
            return token;
        
        var existingTrailing = token.TrailingTrivia;
        var text = token.GetText();
        
        if (existingTrailing is not null)
            return new GreenTokenWithTrivia(token.RawKind, leadingTrivia, text, existingTrailing);
        else
            return new GreenTokenWithLeadingTrivia(token.RawKind, leadingTrivia, text);
    }
    
    /// <summary>
    /// Creates a new token with the specified trailing trivia.
    /// </summary>
    public static GreenToken WithTrailingTrivia(this GreenToken token, GreenNode? trailingTrivia)
    {
        if (trailingTrivia is null || trailingTrivia.FullWidth == 0)
            return token;
        
        var existingLeading = token.LeadingTrivia;
        var text = token.GetText();
        
        if (existingLeading is not null)
            return new GreenTokenWithTrivia(token.RawKind, existingLeading, text, trailingTrivia);
        else
            return new GreenTokenWithTrailingTrivia(token.RawKind, text, trailingTrivia);
    }
}
