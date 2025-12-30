using MetaParser.Schema;
using System.Linq;

namespace MetaParser.Generation;

/// <summary>
/// Generates the main Lexer class that orchestrates token consumption.
/// </summary>
internal static class LexerGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("Lexer.g.cs")
            .AddUsings("System", "System.Buffers", "System.Collections.Generic", "System.Runtime.CompilerServices");

        var code = file.Code;

        GenerateLexer(code, schema);

        return file;
    }

    private static void GenerateLexer(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Lexer that tokenizes input text into a sequence of tokens.");
        code.AppendLine($"internal sealed class {schema.Classname}Lexer");
        code.OpenBlock();

        // Consumers array
        code.AppendLine("private static readonly ITokenConsumer[] s_consumers;");
        code.AppendLine("private readonly GreenTokenFactory _factory;");
        code.AppendLine("private readonly TriviaAttacher _triviaAttacher;");
        code.AppendLine();

        // Static constructor to initialize consumers
        code.AppendLine($"static {schema.Classname}Lexer()");
        code.OpenBlock();
        code.AppendLine("s_consumers = new ITokenConsumer[]");
        code.OpenBlock();
        code.AppendLine("WhitespaceConsumer.Instance,");
        code.AppendLine("EndOfLineConsumer.Instance,");

        foreach (var kvp in schema.Tokens)
        {
            var className = SanitizeIdentifier(kvp.Key) + "Consumer";
            code.AppendLine($"{className}.Instance,");
        }

        code.CloseBlock(";");
        code.AppendLine();
        code.AppendLine("// Sort by priority (lower = higher priority)");
        code.AppendLine("Array.Sort(s_consumers, (a, b) => a.Priority.CompareTo(b.Priority));");
        code.CloseBlock();
        code.AppendLine();

        // Constructor
        code.AppendSummaryLine("Creates a new lexer with a custom token factory.");
        code.AppendLine($"public {schema.Classname}Lexer(GreenTokenFactory? factory = null)");
        code.OpenBlock();
        code.AppendLine("_factory = factory ?? DefaultGreenTokenFactory.Instance;");
        code.AppendLine("_triviaAttacher = new TriviaAttacher(_factory);");
        code.CloseBlock();
        code.AppendLine();

        // Tokenize method
        GenerateTokenizeMethod(code, schema);
        code.AppendLine();

        // TokenizeToList method
        GenerateTokenizeToListMethod(code, schema);
        code.AppendLine();

        // TokenizeWithTrivia method
        GenerateTokenizeWithTriviaMethod(code, schema);

        code.CloseBlock();
    }

    private static void GenerateTokenizeMethod(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Tokenizes input and returns tokens with trivia attached (Roslyn-style).");
        code.AppendParam("input", "The input text to tokenize.");
        code.AppendReturns("An enumerable of green tokens with trivia attached.");
        code.AppendLine("public IEnumerable<GreenToken> Tokenize(string input)");
        code.OpenBlock();
        code.AppendLine("return Tokenize(input.AsMemory());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Tokenizes input memory and returns tokens with trivia attached.");
        code.AppendLine("public IEnumerable<GreenToken> Tokenize(ReadOnlyMemory<char> input)");
        code.OpenBlock();
        code.AppendLine("var tokens = TokenizeRaw(input);");
        code.AppendLine("return _triviaAttacher.AttachTrivia(tokens);");
        code.CloseBlock();
    }

    private static void GenerateTokenizeToListMethod(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Tokenizes input and returns all tokens (including trivia) in a list.");
        code.AppendLine("public List<GreenToken> TokenizeToList(string input)");
        code.OpenBlock();
        code.AppendLine("return TokenizeRaw(input.AsMemory());");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public List<GreenToken> TokenizeToList(ReadOnlyMemory<char> input)");
        code.OpenBlock();
        code.AppendLine("return TokenizeRaw(input);");
        code.CloseBlock();
    }

    private static void GenerateTokenizeWithTriviaMethod(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Raw tokenization - returns all tokens without trivia attachment.");
        code.AppendLine("private List<GreenToken> TokenizeRaw(ReadOnlyMemory<char> input)");
        code.OpenBlock();
        code.AppendLine("var tokens = new List<GreenToken>();");
        code.AppendLine("var sequence = new ReadOnlySequence<char>(input);");
        code.AppendLine("var reader = new SequenceReader<char>(sequence);");
        code.AppendLine();
        code.AppendLine("while (!reader.End)");
        code.OpenBlock();
        code.AppendLine("long startPosition = reader.Consumed;");
        code.AppendLine("bool matched = false;");
        code.AppendLine();
        code.AppendLine("// Try each consumer in priority order");
        code.AppendLine("foreach (var consumer in s_consumers)");
        code.OpenBlock();
        code.AppendLine("var result = consumer.TryConsume(ref reader);");
        code.AppendLine("if (result.Success)");
        code.OpenBlock();
        code.AppendLine("var tokenMemory = input.Slice((int)startPosition, result.Length);");
        code.AppendLine("tokens.Add(_factory.CreateToken((ushort)result.Kind, tokenMemory));");
        code.AppendLine("matched = true;");
        code.AppendLine("break;");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("// If no consumer matched, emit a Bad token for the character");
        code.AppendLine("if (!matched)");
        code.OpenBlock();
        code.AppendLine("var badMemory = input.Slice((int)reader.Consumed, 1);");
        code.AppendLine("tokens.Add(_factory.CreateToken((ushort)TokenKind.Bad, badMemory));");
        code.AppendLine("reader.Advance(1);");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("// Emit EOF token");
        code.AppendLine("tokens.Add(_factory.CreateToken((ushort)TokenKind.EndOfFile, \"\"));");
        code.AppendLine("return tokens;");
        code.CloseBlock();
    }

    private static string SanitizeIdentifier(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "Unknown";

        var result = new System.Text.StringBuilder();
        bool capitalizeNext = true;

        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                result.Append(capitalizeNext ? char.ToUpperInvariant(c) : c);
                capitalizeNext = false;
            }
            else if (c == '_' || c == '-' || c == ' ')
            {
                capitalizeNext = true;
            }
        }

        if (result.Length > 0 && char.IsDigit(result[0]))
        {
            result.Insert(0, '_');
        }

        return result.Length > 0 ? result.ToString() : "Unknown";
    }
}
