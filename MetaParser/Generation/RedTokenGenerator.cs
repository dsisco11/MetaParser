using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the RedToken class that wraps GreenToken with position information.
/// </summary>
internal static class RedTokenGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("SyntaxToken.g.cs")
            .AddUsings("System");

        var code = file.Code;

        GenerateSyntaxTokenClass(code);
        code.AppendLine();
        GenerateSyntaxTokenList(code);

        return file;
    }

    private static void GenerateSyntaxTokenClass(CodeBuilder code)
    {
        code.AppendSummary(@"Represents a token in the syntax tree.

SyntaxToken wraps a GreenToken and provides:
- Parent reference for tree navigation
- Absolute position in the source text
- Access to leading and trailing trivia");
        code.AppendLine("internal sealed class SyntaxToken : SyntaxNode");
        code.OpenBlock();

        // Constructor
        code.AppendSummary("Creates a new syntax token wrapping the specified green token.");
        code.AppendLine("internal SyntaxToken(GreenToken green, SyntaxNode? parent, int position, int slotIndex = -1)");
        code.Indent();
        code.AppendLine(": base(green, parent, position, slotIndex)");
        code.Outdent();
        code.OpenBlock();
        code.CloseBlock();
        code.AppendLine();

        // Typed accessor for underlying green token
        code.AppendSummary("Gets the underlying green token.");
        code.AppendLine("public new GreenToken Green => (GreenToken)base.Green;");
        code.AppendLine();

        // Token kind
        code.AppendSummary("Gets the token kind.");
        code.AppendLine("public TokenKind Kind => (TokenKind)RawKind;");
        code.AppendLine();

        // Text accessors
        code.AppendSummary("Gets the text of this token (excluding trivia).");
        code.AppendLine("public string Text => Green.GetText();");
        code.AppendLine();

        code.AppendSummary("Gets the value text of this token (may differ for escape sequences).");
        code.AppendLine("public string ValueText => Green.GetText();");
        code.AppendLine();

        // Trivia properties
        code.AppendSummary("Gets the width of leading trivia.");
        code.AppendLine("public int LeadingTriviaWidth => Green.LeadingTriviaWidth;");
        code.AppendLine();

        code.AppendSummary("Gets the width of trailing trivia.");
        code.AppendLine("public int TrailingTriviaWidth => Green.TrailingTriviaWidth;");
        code.AppendLine();

        code.AppendSummary("Gets whether this token has any leading trivia.");
        code.AppendLine("public bool HasLeadingTrivia => Green.LeadingTriviaWidth > 0;");
        code.AppendLine();

        code.AppendSummary("Gets whether this token has any trailing trivia.");
        code.AppendLine("public bool HasTrailingTrivia => Green.TrailingTriviaWidth > 0;");
        code.AppendLine();

        // Span properties (more specific for tokens)
        code.AppendSummary("Gets the span of the token text (excluding trivia).");
        code.AppendLine("public new TextSpan Span => new TextSpan(Position + LeadingTriviaWidth, Green.Width);");
        code.AppendLine();

        // Check if missing/synthesized
        code.AppendSummary("Gets whether this is a missing token (synthesized by parser).");
        code.AppendLine("public bool IsMissing => Green.Width == 0 && Kind != TokenKind.EndOfFile;");

        code.CloseBlock();
    }

    private static void GenerateSyntaxTokenList(CodeBuilder code)
    {
        code.AppendSummary("Represents a list of tokens in the syntax tree (typically the root).");
        code.AppendLine("internal sealed class SyntaxTokenList : SyntaxNode");
        code.OpenBlock();

        // Constructor
        code.AppendSummary("Creates a new syntax token list wrapping the specified green token list.");
        code.AppendLine("internal SyntaxTokenList(GreenTokenList green, SyntaxNode? parent, int position, int slotIndex = -1)");
        code.Indent();
        code.AppendLine(": base(green, parent, position, slotIndex) { }");
        code.Outdent();
        code.AppendLine();

        // Typed Green accessor
        code.AppendSummary("Gets the underlying green token list.");
        code.AppendLine("public new GreenTokenList Green => (GreenTokenList)base.Green;");
        code.AppendLine();

        // Count
        code.AppendSummary("Gets the number of tokens.");
        code.AppendLine("public int Count => Green.Count;");
        code.AppendLine();

        // Indexer
        code.AppendSummary("Gets the token at the specified index.");
        code.AppendLine("public SyntaxToken this[int index]");
        code.OpenBlock();
        code.AppendLine("get");
        code.OpenBlock();
        code.AppendLine("var greenToken = Green[index];");
        code.AppendLine("int pos = Position;");
        code.AppendLine("for (int i = 0; i < index; i++) pos += Green[i].FullWidth;");
        code.AppendLine("return new SyntaxToken(greenToken, this, pos, index);");
        code.CloseBlock();
        code.CloseBlock();

        code.CloseBlock();
    }
}
