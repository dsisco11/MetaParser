using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates GreenTrivia and GreenTriviaList classes.
/// </summary>
internal static class GreenTriviaGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("GreenTrivia.g.cs")
            .AddUsings("System", "System.IO");

        var code = file.Code;

        GenerateGreenTrivia(code);
        code.AppendLine();
        GenerateGreenTriviaList(code);
        code.AppendLine();
        GenerateGreenTokenList(code);

        return file;
    }

    private static void GenerateGreenTrivia(CodeBuilder code)
    {
        code.AppendSummary("Represents trivia (whitespace, comments) backed by ReadOnlyMemory.");
        code.AppendLine("internal sealed class GreenTrivia : GreenNode");
        code.OpenBlock();

        // Fields
        code.AppendLine("private readonly ReadOnlyMemory<char> _text;");
        code.AppendLine("private readonly string? _ownedText;");
        code.AppendLine();

        // Text property
        code.AppendSummaryLine("Gets the trivia text as a span.");
        code.AppendLine("public ReadOnlySpan<char> Text => _ownedText is not null ? _ownedText.AsSpan() : _text.Span;");
        code.AppendLine();

        // Constructors
        code.AppendSummaryLine("Creates trivia backed by a memory slice (zero-copy).");
        code.AppendLine("public GreenTrivia(ushort kind, ReadOnlyMemory<char> text)");
        code.Indent();
        code.AppendLine(": base(kind, text.Length, GreenNodeFlags.IsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_text = text;");
        code.AppendLine("_ownedText = null;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummaryLine("Creates trivia backed by an owned string.");
        code.AppendLine("public GreenTrivia(ushort kind, string text)");
        code.Indent();
        code.AppendLine(": base(kind, text.Length, GreenNodeFlags.IsTrivia)");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_text = default;");
        code.AppendLine("_ownedText = text;");
        code.CloseBlock();
        code.AppendLine();

        // GetSlot
        code.AppendSummaryLine("Trivia has no children.");
        code.AppendLine("public override GreenNode? GetSlot(int index) => null;");
        code.AppendLine();

        // WriteTo
        code.AppendSummaryLine("Writes the trivia text.");
        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) writer.Write(_ownedText);");
        code.AppendLine("else writer.Write(_text.Span);");
        code.CloseBlock();
        code.AppendLine();

        // ToOwned
        code.AppendSummaryLine("Creates an owned copy if memory-backed.");
        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("if (_ownedText is not null) return this;");
        code.AppendLine("return new GreenTrivia(RawKind, _text.ToString());");
        code.CloseBlock();
        code.AppendLine();

        // GetText
        code.AppendSummaryLine("Gets the text as a string.");
        code.AppendLine("public string GetText() => _ownedText ?? _text.ToString();");
        code.AppendLine();

        // FormatDebugString
        code.AppendLine("protected override string FormatDebugString()");
        code.OpenBlock();
        code.AppendLine("var text = GetText();");
        code.AppendLine("var escaped = text.Replace(\"\\r\", \"\\\\r\").Replace(\"\\n\", \"\\\\n\").Replace(\"\\t\", \"\\\\t\");");
        code.AppendLine("if (escaped.Length > 20) escaped = escaped.Substring(0, 17) + \"...\";");
        code.AppendLine("return $\"Trivia[Kind={RawKind}, \\\"{escaped}\\\"]\";");
        code.CloseBlock();

        code.CloseBlock();
    }

    private static void GenerateGreenTriviaList(CodeBuilder code)
    {
        code.AppendSummary("A list of trivia nodes stored as a single green node.");
        code.AppendLine("internal sealed class GreenTriviaList : GreenNode");
        code.OpenBlock();

        code.AppendLine("private readonly GreenTrivia[] _trivia;");
        code.AppendLine();

        // Constructor
        code.AppendLine("public GreenTriviaList(ushort kind, GreenTrivia[] trivia)");
        code.Indent();
        code.AppendLine(": base(kind, ComputeWidth(trivia), GreenNodeFlags.IsTrivia | GreenNodeFlags.ContainsTrivia, (byte)Math.Min(trivia.Length, 255))");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_trivia = trivia;");
        code.CloseBlock();
        code.AppendLine();

        // Count
        code.AppendSummaryLine("Gets the number of trivia items.");
        code.AppendLine("public int Count => _trivia.Length;");
        code.AppendLine();

        // Indexer
        code.AppendSummaryLine("Gets the trivia at the specified index.");
        code.AppendLine("public GreenTrivia this[int index] => _trivia[index];");
        code.AppendLine();

        // GetSlot
        code.AppendLine("public override GreenNode? GetSlot(int index)");
        code.OpenBlock();
        code.AppendLine("if (index >= 0 && index < _trivia.Length) return _trivia[index];");
        code.AppendLine("return null;");
        code.CloseBlock();
        code.AppendLine();

        // WriteTo
        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("foreach (var t in _trivia) t.WriteTo(writer);");
        code.CloseBlock();
        code.AppendLine();

        // ToOwned
        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("var owned = new GreenTrivia[_trivia.Length];");
        code.AppendLine("for (int i = 0; i < _trivia.Length; i++) owned[i] = (GreenTrivia)_trivia[i].ToOwned();");
        code.AppendLine("return new GreenTriviaList(RawKind, owned);");
        code.CloseBlock();
        code.AppendLine();

        // ComputeWidth
        code.AppendLine("private static int ComputeWidth(GreenTrivia[] trivia)");
        code.OpenBlock();
        code.AppendLine("int width = 0;");
        code.AppendLine("foreach (var t in trivia) width += t.FullWidth;");
        code.AppendLine("return width;");
        code.CloseBlock();
        code.AppendLine();

        // FormatDebugString
        code.AppendLine("protected override string FormatDebugString() => $\"TriviaList[Count={_trivia.Length}, Width={FullWidth}]\";");

        code.CloseBlock();
    }

    private static void GenerateGreenTokenList(CodeBuilder code)
    {
        code.AppendSummary("A list of tokens stored as a single green node (used as root for token streams).");
        code.AppendLine("internal sealed class GreenTokenList : GreenNode");
        code.OpenBlock();

        code.AppendLine("private readonly GreenToken[] _tokens;");
        code.AppendLine();

        // Constructor
        code.AppendLine("public GreenTokenList(GreenToken[] tokens)");
        code.Indent();
        code.AppendLine(": base(0, ComputeWidth(tokens), GreenNodeFlags.None, (byte)Math.Min(tokens.Length, 255))");
        code.Outdent();
        code.OpenBlock();
        code.AppendLine("_tokens = tokens;");
        code.CloseBlock();
        code.AppendLine();

        // Count
        code.AppendSummaryLine("Gets the number of tokens.");
        code.AppendLine("public int Count => _tokens.Length;");
        code.AppendLine();

        // Indexer
        code.AppendSummaryLine("Gets the token at the specified index.");
        code.AppendLine("public GreenToken this[int index] => _tokens[index];");
        code.AppendLine();

        // GetSlot
        code.AppendLine("public override GreenNode? GetSlot(int index)");
        code.OpenBlock();
        code.AppendLine("if (index >= 0 && index < _tokens.Length) return _tokens[index];");
        code.AppendLine("return null;");
        code.CloseBlock();
        code.AppendLine();

        // WriteTo
        code.AppendLine("public override void WriteTo(TextWriter writer)");
        code.OpenBlock();
        code.AppendLine("foreach (var t in _tokens) t.WriteTo(writer);");
        code.CloseBlock();
        code.AppendLine();

        // ToOwned
        code.AppendLine("public override GreenNode ToOwned()");
        code.OpenBlock();
        code.AppendLine("var owned = new GreenToken[_tokens.Length];");
        code.AppendLine("for (int i = 0; i < _tokens.Length; i++) owned[i] = (GreenToken)_tokens[i].ToOwned();");
        code.AppendLine("return new GreenTokenList(owned);");
        code.CloseBlock();
        code.AppendLine();

        // ComputeWidth
        code.AppendLine("private static int ComputeWidth(GreenToken[] tokens)");
        code.OpenBlock();
        code.AppendLine("int width = 0;");
        code.AppendLine("foreach (var t in tokens) width += t.FullWidth;");
        code.AppendLine("return width;");
        code.CloseBlock();
        code.AppendLine();

        // FormatDebugString
        code.AppendLine("protected override string FormatDebugString() => $\"TokenList[Count={_tokens.Length}, Width={FullWidth}]\";");

        code.CloseBlock();
    }
}
