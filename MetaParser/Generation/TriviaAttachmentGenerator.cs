using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the TriviaAttacher class that implements Roslyn-style trivia attachment.
/// 
/// Roslyn trivia rules:
/// - Trivia at the start of a line attaches as LEADING trivia to the next token
/// - Trivia on the same line as a token attaches as TRAILING trivia to that token
/// - When a newline is encountered, remaining trivia before it becomes trailing, newline starts leading
/// </summary>
internal static class TriviaAttachmentGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("TriviaAttacher.g.cs")
            .AddUsings("System", "System.Collections.Generic");

        var code = file.Code;

        GenerateTriviaAttacher(code, schema);

        return file;
    }

    private static void GenerateTriviaAttacher(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary(@"Attaches trivia to tokens using Roslyn-style rules.

Roslyn trivia attachment rules:
- Leading trivia: All trivia after a newline (or start of file) until the next non-trivia token
- Trailing trivia: All trivia on the same line as a token, up to and including the newline

Example: '  x + y  \\n  z'
- '  ' (leading) + 'x' + '' (trailing)
- ' ' (leading) + '+' + '' (trailing)  
- ' ' (leading) + 'y' + '  \\n' (trailing)
- '  ' (leading) + 'z' + '' (trailing)");
        code.AppendLine($"internal sealed class TriviaAttacher");
        code.OpenBlock();

        code.AppendLine("private readonly GreenTokenFactory _factory;");
        code.AppendLine();

        // Constructor
        code.AppendLine("public TriviaAttacher(GreenTokenFactory factory)");
        code.OpenBlock();
        code.AppendLine("_factory = factory;");
        code.CloseBlock();
        code.AppendLine();

        // AttachTrivia method
        GenerateAttachTriviaMethod(code);

        // Helper to create trivia node from list
        code.AppendLine();
        GenerateCreateTriviaNodeMethod(code);

        code.CloseBlock();
    }

    private static void GenerateAttachTriviaMethod(CodeBuilder code)
    {
        code.AppendSummary("Attaches trivia to tokens following Roslyn conventions.");
        code.AppendParam("rawTokens", "The raw token stream including trivia tokens.");
        code.AppendReturns("Tokens with trivia attached as leading/trailing.");
        code.AppendLine("public IEnumerable<GreenToken> AttachTrivia(IEnumerable<GreenToken> rawTokens)");
        code.OpenBlock();

        code.AppendLine("var leadingTrivia = new List<GreenTrivia>();");
        code.AppendLine("var trailingTrivia = new List<GreenTrivia>();");
        code.AppendLine("GreenToken? currentToken = null;");
        code.AppendLine("bool atLineStart = true; // Start of file is like after a newline");
        code.AppendLine();

        code.AppendLine("foreach (var token in rawTokens)");
        code.OpenBlock();
        code.AppendLine("var kind = (TokenKind)token.RawKind;");
        code.AppendLine();

        // Handle trivia tokens
        code.AppendLine("if (kind.IsTrivia())");
        code.OpenBlock();
        code.AppendLine("var trivia = _factory.CreateTrivia(token.RawKind, token.GetText());");
        code.AppendLine();
        code.AppendLine("if (atLineStart)");
        code.OpenBlock();
        code.AppendLine("// At line start, all trivia is leading");
        code.AppendLine("leadingTrivia.Add(trivia);");
        code.CloseBlock();
        code.AppendLine("else");
        code.OpenBlock();
        code.AppendLine("// Same line as previous token, this is trailing");
        code.AppendLine("trailingTrivia.Add(trivia);");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("// Check if this trivia ends the line");
        code.AppendLine("if (kind == TokenKind.EndOfLine)");
        code.OpenBlock();
        code.AppendLine("atLineStart = true;");
        code.AppendLine("// Newline is trailing trivia for current token, next trivia is leading");
        code.AppendLine("if (currentToken is not null && trailingTrivia.Count > 0)");
        code.OpenBlock();
        code.AppendLine("var trailing = CreateTriviaNode(trailingTrivia);");
        code.AppendLine("currentToken = currentToken.WithTrailingTrivia(trailing);");
        code.AppendLine("trailingTrivia.Clear();");
        code.CloseBlock();
        code.CloseBlock();
        code.CloseBlock();

        // Handle non-trivia tokens
        code.AppendLine("else");
        code.OpenBlock();
        code.AppendLine("// Non-trivia token - finalize previous token and start new one");
        code.AppendLine();
        code.AppendLine("// Yield previous token with its trailing trivia");
        code.AppendLine("if (currentToken is not null)");
        code.OpenBlock();
        code.AppendLine("if (trailingTrivia.Count > 0)");
        code.OpenBlock();
        code.AppendLine("var trailing = CreateTriviaNode(trailingTrivia);");
        code.AppendLine("currentToken = currentToken.WithTrailingTrivia(trailing);");
        code.AppendLine("trailingTrivia.Clear();");
        code.CloseBlock();
        code.AppendLine("yield return currentToken;");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("// Create new token with leading trivia");
        code.AppendLine("currentToken = token;");
        code.AppendLine("if (leadingTrivia.Count > 0)");
        code.OpenBlock();
        code.AppendLine("var leading = CreateTriviaNode(leadingTrivia);");
        code.AppendLine("currentToken = currentToken.WithLeadingTrivia(leading);");
        code.AppendLine("leadingTrivia.Clear();");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("atLineStart = false;");
        code.CloseBlock();

        code.CloseBlock(); // foreach
        code.AppendLine();

        // Yield final token
        code.AppendLine("// Yield final token");
        code.AppendLine("if (currentToken is not null)");
        code.OpenBlock();
        code.AppendLine("// Any remaining trivia becomes trailing");
        code.AppendLine("if (trailingTrivia.Count > 0)");
        code.OpenBlock();
        code.AppendLine("var trailing = CreateTriviaNode(trailingTrivia);");
        code.AppendLine("currentToken = currentToken.WithTrailingTrivia(trailing);");
        code.CloseBlock();
        code.AppendLine("else if (leadingTrivia.Count > 0)");
        code.OpenBlock();
        code.AppendLine("// Edge case: file ends with trivia after newline");
        code.AppendLine("var trailing = CreateTriviaNode(leadingTrivia);");
        code.AppendLine("currentToken = currentToken.WithTrailingTrivia(trailing);");
        code.CloseBlock();
        code.AppendLine("yield return currentToken;");
        code.CloseBlock();

        code.CloseBlock(); // method
    }

    private static void GenerateCreateTriviaNodeMethod(CodeBuilder code)
    {
        code.AppendLine("private GreenNode CreateTriviaNode(List<GreenTrivia> trivia)");
        code.OpenBlock();
        code.AppendLine("if (trivia.Count == 1)");
        code.Indent();
        code.AppendLine("return trivia[0];");
        code.Outdent();
        code.AppendLine("return _factory.CreateTriviaList(0, trivia.ToArray());");
        code.CloseBlock();
    }
}
