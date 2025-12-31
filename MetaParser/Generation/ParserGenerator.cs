using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the Parser class that provides the main API for parsing source text.
/// 
/// The Parser combines:
/// - Lexer for tokenization
/// - Trivia attachment
/// - SyntaxTree creation
/// </summary>
internal static class ParserGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("Parser.g.cs")
            .AddUsings("System", "System.Collections.Generic", "System.Collections.Immutable");

        var code = file.Code;

        GenerateParserClass(code, schema);

        return file;
    }

    private static void GenerateParserClass(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary($@"Parser for {schema.Classname} syntax.

This is the main entry point for parsing source text. Use the static Parse methods
or create an instance for more control over parsing options.");
        code.AppendLine($"internal sealed class {schema.Classname}Parser");
        code.OpenBlock();

        // Fields
        code.AppendLine($"private readonly {schema.Classname}Lexer _lexer;");
        code.AppendLine();

        // Constructor
        code.AppendSummary("Creates a new parser instance.");
        code.AppendLine($"public {schema.Classname}Parser()");
        code.OpenBlock();
        code.AppendLine($"_lexer = new {schema.Classname}Lexer();");
        code.CloseBlock();
        code.AppendLine();

        // Static Parse method
        code.AppendSummary("Parses source text into a syntax tree.");
        code.AppendParam("text", "The source text to parse.");
        code.AppendReturns("A syntax tree representing the parsed source.");
        code.AppendLine($"public static {schema.Classname}SyntaxTree Parse(string text)");
        code.OpenBlock();
        code.AppendLine($"return {schema.Classname}SyntaxTree.Parse(text);");
        code.CloseBlock();
        code.AppendLine();

        // Instance Parse method
        code.AppendSummary("Parses source text into a syntax tree using this parser instance.");
        code.AppendLine($"public {schema.Classname}SyntaxTree ParseText(string text)");
        code.OpenBlock();
        code.AppendLine($"return {schema.Classname}SyntaxTree.Parse(text);");
        code.CloseBlock();
        code.AppendLine();

        // Tokenize method (useful for debugging/tooling)
        code.AppendSummary("Tokenizes source text without building a full syntax tree.");
        code.AppendParam("text", "The source text to tokenize.");
        code.AppendReturns("An enumerable of tokens with trivia attached.");
        code.AppendLine("public IEnumerable<GreenToken> Tokenize(string text)");
        code.OpenBlock();
        code.AppendLine("return _lexer.Tokenize(text);");
        code.CloseBlock();
        code.AppendLine();

        // TokenizeRaw method (useful for debugging)
        code.AppendSummary("Tokenizes source text without trivia attachment.");
        code.AppendParam("text", "The source text to tokenize.");
        code.AppendReturns("A list of raw tokens including trivia tokens.");
        code.AppendLine("public List<GreenToken> TokenizeRaw(string text)");
        code.OpenBlock();
        code.AppendLine("return _lexer.TokenizeToList(text);");
        code.CloseBlock();

        code.CloseBlock();
    }
}
