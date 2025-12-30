using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates consumer interfaces for the lexer.
/// </summary>
internal static class ConsumerInterfaceGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("Consumers.g.cs")
            .AddUsings("System", "System.Buffers");

        var code = file.Code;

        GenerateLexerResult(code);
        code.AppendLine();
        GenerateITokenConsumer(code);
        code.AppendLine();
        GenerateTriviaKind(code);

        return file;
    }

    private static void GenerateLexerResult(CodeBuilder code)
    {
        code.AppendSummary("Result of a token consumption attempt.");
        code.AppendLine("internal readonly struct LexerResult");
        code.OpenBlock();

        code.AppendSummaryLine("Whether the consumer matched.");
        code.AppendLine("public readonly bool Success;");
        code.AppendLine();

        code.AppendSummaryLine("The token kind if successful.");
        code.AppendLine("public readonly TokenKind Kind;");
        code.AppendLine();

        code.AppendSummaryLine("Number of characters consumed.");
        code.AppendLine("public readonly int Length;");
        code.AppendLine();

        code.AppendSummaryLine("Creates a successful result.");
        code.AppendLine("public LexerResult(TokenKind kind, int length)");
        code.OpenBlock();
        code.AppendLine("Success = true;");
        code.AppendLine("Kind = kind;");
        code.AppendLine("Length = length;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummaryLine("A failed result.");
        code.AppendLine("public static readonly LexerResult Fail = default;");
        code.AppendLine();

        code.AppendSummaryLine("Implicit bool conversion for pattern matching.");
        code.AppendLine("public static implicit operator bool(LexerResult result) => result.Success;");

        code.CloseBlock();
    }

    private static void GenerateITokenConsumer(CodeBuilder code)
    {
        code.AppendSummary("Interface for token consumers that match input sequences.");
        code.AppendLine("internal interface ITokenConsumer");
        code.OpenBlock();

        code.AppendSummaryLine("Priority for consumer ordering (lower = higher priority).");
        code.AppendLine("int Priority { get; }");
        code.AppendLine();

        code.AppendSummaryLine("Whether this consumer produces trivia tokens.");
        code.AppendLine("bool IsTrivia { get; }");
        code.AppendLine();

        code.AppendSummaryLine("Attempts to consume a token from the input.");
        code.AppendParam("reader", "The sequence reader positioned at the start of potential token.");
        code.AppendReturns("A result indicating success/failure, token kind, and length consumed.");
        code.AppendLine("LexerResult TryConsume(ref SequenceReader<char> reader);");

        code.CloseBlock();
    }

    private static void GenerateTriviaKind(CodeBuilder code)
    {
        code.AppendSummary("Specifies how trivia should be attached to tokens.");
        code.AppendLine("internal enum TriviaAttachment");
        code.OpenBlock();
        code.AppendSummaryLine("Trivia attaches to the following token as leading trivia.");
        code.AppendLine("Leading,");
        code.AppendSummaryLine("Trivia attaches to the previous token as trailing trivia.");
        code.AppendLine("Trailing,");
        code.CloseBlock();
    }
}
