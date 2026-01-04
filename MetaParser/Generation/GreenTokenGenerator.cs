using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates GreenToken base class and trivia variants.
/// </summary>
internal static class GreenTokenGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("GreenToken.g.cs")
            .AddUsings("System", "System.IO");

        var code = file.Code;

        GenerateGreenTokenBase(code);
        code.AppendLine();
        GenerateGreenTokenWithNoTrivia(code);
        code.AppendLine();
        GenerateGreenTokenWithLeadingTrivia(code);
        code.AppendLine();
        GenerateGreenTokenWithTrailingTrivia(code);
        code.AppendLine();
        GenerateGreenTokenWithTrivia(code);
        code.AppendLine();
        GenerateGreenTokenExtensions(code);

        return file;
    }

    private static void GenerateGreenTokenBase(CodeBuilder code)
    {
        code.AppendSummary("Base class for all green tokens (leaf nodes).");
        code.AppendLine("internal abstract class GreenToken : GreenNode");
        code.OpenBlock();

        code.AppendLine("protected GreenToken(ushort kind, int fullWidth, GreenNodeFlags flags = GreenNodeFlags.None)");
        code.Indent();
        code.AppendLine(": base(kind, fullWidth, flags | GreenNodeFlags.IsToken, slotCount: 0) { }");
        code.Outdent();
        code.AppendLine();

        code.AppendSummaryLine("Gets the token text as a span.");
        code.AppendLine("public abstract ReadOnlySpan<char> Text { get; }");
        code.AppendLine();

        code.AppendSummaryLine("Gets the token text as a string.");
        code.AppendLine("public abstract string GetText();");
        code.AppendLine();

        code.AppendSummaryLine("Tokens have no child slots.");
        code.AppendLine("public sealed override GreenNode? GetSlot(int index) => null;");
        code.AppendLine();

        code.AppendSummaryLine("Creates a SyntaxToken wrapper for this green token.");
        code.AppendLine("internal override SyntaxNode CreateRed(SyntaxNode? parent, int position, int slotIndex)");
        code.Indent();
        code.AppendLine("=> new SyntaxToken(this, parent, position, slotIndex);");
        code.Outdent();

        code.CloseBlock();
    }

    private static void GenerateGreenTokenWithNoTrivia(CodeBuilder code)
    {
        code.AppendSummary("A token with no trivia (most common for keywords/operators).");
        code.AppendLine("internal sealed class GreenTokenWithNoTrivia : GreenToken");
        code.OpenBlock();

        code.AppendLine("private readonly ReadOnlyMemory<char> _text;");
        code.AppendLine("private readonly string? _ownedText;");
        code.AppendLine();

        code.AppendLine("public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;");
        code.AppendLine("public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;");
        code.AppendLine();

        // Memory constructor
        code.AppendSummaryLine("Creates a token backed by a memory slice.");
        code.AppendLine("public GreenTokenWithNoTrivia(ushort kind, ReadOnlyMemory<char> text) : base(kind, text.Length)");
        code.OpenBlock();
        code.AppendLine("_text = text;");
        code.AppendLine("_ownedText = null;");
        code.CloseBlock();
        code.AppendLine();

        // String constructor
        code.AppendSummaryLine("Creates a token backed by an owned string.");
        code.AppendLine("public GreenTokenWithNoTrivia(ushort kind, string text) : base(kind, text.Length)");
        code.OpenBlock();
        code.AppendLine("_text = default;");
        code.AppendLine("_ownedText = text;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string GetText() => _ownedText ?? _text.ToString();");
        code.AppendLine();

        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.Span);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override void WriteCoreTo(TextWriter writer) => WriteTo(writer);");
        code.AppendLine();

        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) return this;");
        code.AppendLine("return new GreenTokenWithNoTrivia(RawKind, _text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override string FormatDebugString()");
        code.OpenBlock();
        code.AppendLine("var text = GetText();");
        code.AppendLine("if (text.Length > 20) text = text.Substring(0, 17) + \"...\";");
        code.AppendLine("return $\"Token[Kind={RawKind}, \\\"{text}\\\"]\";");
        code.CloseBlock();

        code.CloseBlock();
    }

    private static void GenerateGreenTokenWithLeadingTrivia(CodeBuilder code)
    {
        code.AppendSummary("A token with only leading trivia.");
        code.AppendLine("internal sealed class GreenTokenWithLeadingTrivia : GreenToken");
        code.OpenBlock();

        code.AppendLine("private readonly GreenNode _leadingTrivia;");
        code.AppendLine("private readonly ReadOnlyMemory<char> _text;");
        code.AppendLine("private readonly string? _ownedText;");
        code.AppendLine();

        code.AppendLine("public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;");
        code.AppendLine("public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;");
        code.AppendLine("public override int LeadingTriviaWidth => _leadingTrivia.FullWidth;");
        code.AppendLine("public override GreenNode? LeadingTrivia => _leadingTrivia;");
        code.AppendLine();

        // Memory constructor
        code.AppendLine("public GreenTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text)");
        code.Indent();
        code.AppendLine(": base(kind, leadingTrivia.FullWidth + text.Length, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_leadingTrivia = leadingTrivia;");
        code.AppendLine("_text = text;");
        code.AppendLine("_ownedText = null;");
        code.CloseBlock();
        code.AppendLine();

        // String constructor
        code.AppendLine("public GreenTokenWithLeadingTrivia(ushort kind, GreenNode leadingTrivia, string text)");
        code.Indent();
        code.AppendLine(": base(kind, leadingTrivia.FullWidth + text.Length, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_leadingTrivia = leadingTrivia;");
        code.AppendLine("_text = default;");
        code.AppendLine("_ownedText = text;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string GetText() => _ownedText ?? _text.ToString();");
        code.AppendLine();

        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("_leadingTrivia.WriteTo(writer);");
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.Span);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override void WriteCoreTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null && _leadingTrivia.ToOwned() == _leadingTrivia) return this;");
        code.AppendLine("return new GreenTokenWithLeadingTrivia(RawKind, _leadingTrivia.ToOwned(), _ownedText ?? _text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override string FormatDebugString()");
        code.OpenBlock();
        code.AppendLine("var text = GetText();");
        code.AppendLine("if (text.Length > 20) text = text.Substring(0, 17) + \"...\";");
        code.AppendLine("return $\"Token[Kind={RawKind}, \\\"{text}\\\", Leading={LeadingTriviaWidth}]\";");
        code.CloseBlock();

        code.CloseBlock();
    }

    private static void GenerateGreenTokenWithTrailingTrivia(CodeBuilder code)
    {
        code.AppendSummary("A token with only trailing trivia.");
        code.AppendLine("internal sealed class GreenTokenWithTrailingTrivia : GreenToken");
        code.OpenBlock();

        code.AppendLine("private readonly ReadOnlyMemory<char> _text;");
        code.AppendLine("private readonly string? _ownedText;");
        code.AppendLine("private readonly GreenNode _trailingTrivia;");
        code.AppendLine();

        code.AppendLine("public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;");
        code.AppendLine("public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;");
        code.AppendLine("public override int TrailingTriviaWidth => _trailingTrivia.FullWidth;");
        code.AppendLine("public override GreenNode? TrailingTrivia => _trailingTrivia;");
        code.AppendLine();

        // Memory constructor
        code.AppendLine("public GreenTokenWithTrailingTrivia(ushort kind, ReadOnlyMemory<char> text, GreenNode trailingTrivia)");
        code.Indent();
        code.AppendLine(": base(kind, text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_text = text;");
        code.AppendLine("_ownedText = null;");
        code.AppendLine("_trailingTrivia = trailingTrivia;");
        code.CloseBlock();
        code.AppendLine();

        // String constructor
        code.AppendLine("public GreenTokenWithTrailingTrivia(ushort kind, string text, GreenNode trailingTrivia)");
        code.Indent();
        code.AppendLine(": base(kind, text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_text = default;");
        code.AppendLine("_ownedText = text;");
        code.AppendLine("_trailingTrivia = trailingTrivia;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string GetText() => _ownedText ?? _text.ToString();");
        code.AppendLine();

        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.Span);");
        code.AppendLine("_trailingTrivia.WriteTo(writer);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override void WriteCoreTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null && _trailingTrivia.ToOwned() == _trailingTrivia) return this;");
        code.AppendLine("return new GreenTokenWithTrailingTrivia(RawKind, _ownedText ?? _text.ToString(), _trailingTrivia.ToOwned());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override string FormatDebugString()");
        code.OpenBlock();
        code.AppendLine("var text = GetText();");
        code.AppendLine("if (text.Length > 20) text = text.Substring(0, 17) + \"...\";");
        code.AppendLine("return $\"Token[Kind={RawKind}, \\\"{text}\\\", Trailing={TrailingTriviaWidth}]\";");
        code.CloseBlock();

        code.CloseBlock();
    }

    private static void GenerateGreenTokenWithTrivia(CodeBuilder code)
    {
        code.AppendSummary("A token with both leading and trailing trivia.");
        code.AppendLine("internal sealed class GreenTokenWithTrivia : GreenToken");
        code.OpenBlock();

        code.AppendLine("private readonly GreenNode _leadingTrivia;");
        code.AppendLine("private readonly ReadOnlyMemory<char> _text;");
        code.AppendLine("private readonly string? _ownedText;");
        code.AppendLine("private readonly GreenNode _trailingTrivia;");
        code.AppendLine();

        code.AppendLine("public override ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;");
        code.AppendLine("public override int Width => _ownedText is not null ? _ownedText.Length : _text.Length;");
        code.AppendLine("public override int LeadingTriviaWidth => _leadingTrivia.FullWidth;");
        code.AppendLine("public override int TrailingTriviaWidth => _trailingTrivia.FullWidth;");
        code.AppendLine("public override GreenNode? LeadingTrivia => _leadingTrivia;");
        code.AppendLine("public override GreenNode? TrailingTrivia => _trailingTrivia;");
        code.AppendLine();

        // Memory constructor
        code.AppendLine("public GreenTokenWithTrivia(ushort kind, GreenNode leadingTrivia, ReadOnlyMemory<char> text, GreenNode trailingTrivia)");
        code.Indent();
        code.AppendLine(": base(kind, leadingTrivia.FullWidth + text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_leadingTrivia = leadingTrivia;");
        code.AppendLine("_text = text;");
        code.AppendLine("_ownedText = null;");
        code.AppendLine("_trailingTrivia = trailingTrivia;");
        code.CloseBlock();
        code.AppendLine();

        // String constructor
        code.AppendLine("public GreenTokenWithTrivia(ushort kind, GreenNode leadingTrivia, string text, GreenNode trailingTrivia)");
        code.Indent();
        code.AppendLine(": base(kind, leadingTrivia.FullWidth + text.Length + trailingTrivia.FullWidth, GreenNodeFlags.ContainsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_leadingTrivia = leadingTrivia;");
        code.AppendLine("_text = default;");
        code.AppendLine("_ownedText = text;");
        code.AppendLine("_trailingTrivia = trailingTrivia;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string GetText() => _ownedText ?? _text.ToString();");
        code.AppendLine();

        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("_leadingTrivia.WriteTo(writer);");
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.Span);");
        code.AppendLine("_trailingTrivia.WriteTo(writer);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override void WriteCoreTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null && _leadingTrivia.ToOwned() == _leadingTrivia && _trailingTrivia.ToOwned() == _trailingTrivia) return this;");
        code.AppendLine("return new GreenTokenWithTrivia(RawKind, _leadingTrivia.ToOwned(), _ownedText ?? _text.ToString(), _trailingTrivia.ToOwned());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("protected override string FormatDebugString()");
        code.OpenBlock();
        code.AppendLine("var text = GetText();");
        code.AppendLine("if (text.Length > 20) text = text.Substring(0, 17) + \"...\";");
        code.AppendLine("return $\"Token[Kind={RawKind}, \\\"{text}\\\", Leading={LeadingTriviaWidth}, Trailing={TrailingTriviaWidth}]\";");
        code.CloseBlock();

        code.CloseBlock();
    }

    private static void GenerateGreenTokenExtensions(CodeBuilder code)
    {
        code.AppendSummary("Extension methods for creating tokens with trivia.");
        code.AppendLine("internal static class GreenTokenExtensions");
        code.OpenBlock();

        code.AppendSummaryLine("Creates a new token with the specified leading trivia.");
        code.AppendLine("public static GreenToken WithLeadingTrivia(this GreenToken token, GreenNode? leadingTrivia)");
        code.OpenBlock();
        code.AppendLine("if (leadingTrivia is null || leadingTrivia.FullWidth == 0) return token;");
        code.AppendLine("var existingTrailing = token.TrailingTrivia;");
        code.AppendLine("var text = token.GetText();");
        code.AppendLine("if (existingTrailing is not null)");
        code.Indent();
        code.AppendLine("return new GreenTokenWithTrivia(token.RawKind, leadingTrivia, text, existingTrailing);");
        code.Outdent();
        code.AppendLine("return new GreenTokenWithLeadingTrivia(token.RawKind, leadingTrivia, text);");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummaryLine("Creates a new token with the specified trailing trivia.");
        code.AppendLine("public static GreenToken WithTrailingTrivia(this GreenToken token, GreenNode? trailingTrivia)");
        code.OpenBlock();
        code.AppendLine("if (trailingTrivia is null || trailingTrivia.FullWidth == 0) return token;");
        code.AppendLine("var existingLeading = token.LeadingTrivia;");
        code.AppendLine("var text = token.GetText();");
        code.AppendLine("if (existingLeading is not null)");
        code.Indent();
        code.AppendLine("return new GreenTokenWithTrivia(token.RawKind, existingLeading, text, trailingTrivia);");
        code.Outdent();
        code.AppendLine("return new GreenTokenWithTrailingTrivia(token.RawKind, text, trailingTrivia);");
        code.CloseBlock();

        code.CloseBlock();
    }
}
