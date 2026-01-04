using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates SyntaxTrivia and SyntaxTokenList red node wrappers.
/// </summary>
internal static class SyntaxTriviaGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("SyntaxTrivia.g.cs")
            .AddUsings("System");

        var code = file.Code;

        GenerateSyntaxTrivia(code);

        return file;
    }

    private static void GenerateSyntaxTrivia(CodeBuilder code)
    {
        code.AppendSummary("Represents trivia (whitespace, comments) in the syntax tree.");
        code.AppendLine("internal sealed class SyntaxTrivia : SyntaxNode");
        code.OpenBlock();

        // Constructor
        code.AppendSummary("Creates a new syntax trivia wrapping the specified green trivia.");
        code.AppendLine("internal SyntaxTrivia(GreenNode green, SyntaxNode? parent, int position, int slotIndex = -1)");
        code.Indent();
        code.AppendLine(": base(green, parent, position, slotIndex) { }");
        code.Outdent();
        code.AppendLine();

        // Text accessor
        code.AppendSummary("Gets the trivia text.");
        code.AppendLine("public string Text => Green switch");
        code.OpenBlock();
        code.AppendLine("GreenTrivia t => t.GetText(),");
        code.AppendLine("_ => Green.ToFullString()");
        code.CloseBlock(";");

        code.CloseBlock();
    }
}
