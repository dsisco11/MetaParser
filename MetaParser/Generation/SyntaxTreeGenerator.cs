using MetaParser.Schema;

namespace MetaParser.Generation;

/// <summary>
/// Generates the SyntaxTree class that represents a parsed source file.
/// 
/// The SyntaxTree provides:
/// - Access to the root node
/// - Access to the original source text
/// - Collection of diagnostics (errors/warnings)
/// - Factory methods for parsing
/// </summary>
internal static class SyntaxTreeGenerator
{
    public static GeneratedFileBuilder Generate(SchemaDefinition schema)
    {
        var file = new GeneratedFileBuilder(schema.Namespace)
            .WithFileName("SyntaxTree.g.cs")
            .AddUsings("System", "System.Collections.Generic", "System.Collections.Immutable", "System.Linq");

        var code = file.Code;

        GenerateDiagnosticClass(code);
        code.AppendLine();
        GenerateDiagnosticSeverityEnum(code);
        code.AppendLine();
        GenerateSyntaxTreeClass(code, schema);

        return file;
    }

    private static void GenerateDiagnosticSeverityEnum(CodeBuilder code)
    {
        code.AppendSummary("Severity levels for diagnostics.");
        code.AppendLine("internal enum DiagnosticSeverity");
        code.OpenBlock();
        code.AppendLine("/// <summary>Hidden diagnostic (not shown to user).</summary>");
        code.AppendLine("Hidden = 0,");
        code.AppendLine("/// <summary>Informational message.</summary>");
        code.AppendLine("Info = 1,");
        code.AppendLine("/// <summary>Warning that doesn't prevent compilation.</summary>");
        code.AppendLine("Warning = 2,");
        code.AppendLine("/// <summary>Error that prevents successful parsing.</summary>");
        code.AppendLine("Error = 3,");
        code.CloseBlock();
    }

    private static void GenerateDiagnosticClass(CodeBuilder code)
    {
        code.AppendSummary("Represents a diagnostic message (error, warning, etc.) from parsing.");
        code.AppendLine("internal sealed class Diagnostic");
        code.OpenBlock();

        code.AppendSummary("Gets the unique identifier for this diagnostic type.");
        code.AppendLine("public string Id { get; }");
        code.AppendLine();

        code.AppendSummary("Gets the human-readable message.");
        code.AppendLine("public string Message { get; }");
        code.AppendLine();

        code.AppendSummary("Gets the severity of the diagnostic.");
        code.AppendLine("public DiagnosticSeverity Severity { get; }");
        code.AppendLine();

        code.AppendSummary("Gets the span in the source where the diagnostic occurred.");
        code.AppendLine("public TextSpan Span { get; }");
        code.AppendLine();

        code.AppendSummary("Creates a new diagnostic.");
        code.AppendLine("public Diagnostic(string id, string message, DiagnosticSeverity severity, TextSpan span)");
        code.OpenBlock();
        code.AppendLine("Id = id;");
        code.AppendLine("Message = message;");
        code.AppendLine("Severity = severity;");
        code.AppendLine("Span = span;");
        code.CloseBlock();
        code.AppendLine();

        code.AppendLine("public override string ToString() => $\"{Severity} {Id}: {Message} at {Span}\";");

        code.CloseBlock();
    }

    private static void GenerateSyntaxTreeClass(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary(@"Represents a parsed syntax tree.

A SyntaxTree contains:
- The root green node (immutable internal representation)
- The original source text
- Any diagnostics produced during parsing

Use Parse() or ParseText() to create a SyntaxTree.");
        code.AppendLine($"internal sealed class {schema.Classname}SyntaxTree");
        code.OpenBlock();

        // Fields
        code.AppendLine("private readonly GreenNode _greenRoot;");
        code.AppendLine("private readonly string _sourceText;");
        code.AppendLine("private readonly ImmutableArray<Diagnostic> _diagnostics;");
        code.AppendLine("private RedNode? _redRoot;");
        code.AppendLine();

        // Constructor
        code.AppendSummary("Creates a new syntax tree. Use Parse() instead of calling this directly.");
        code.AppendLine($"private {schema.Classname}SyntaxTree(GreenNode greenRoot, string sourceText, ImmutableArray<Diagnostic> diagnostics)");
        code.OpenBlock();
        code.AppendLine("_greenRoot = greenRoot;");
        code.AppendLine("_sourceText = sourceText;");
        code.AppendLine("_diagnostics = diagnostics;");
        code.CloseBlock();
        code.AppendLine();

        // Properties
        code.AppendSummary("Gets the original source text.");
        code.AppendLine("public string SourceText => _sourceText;");
        code.AppendLine();

        code.AppendSummary("Gets the length of the source text.");
        code.AppendLine("public int Length => _sourceText.Length;");
        code.AppendLine();

        code.AppendSummary("Gets any diagnostics produced during parsing.");
        code.AppendLine("public ImmutableArray<Diagnostic> Diagnostics => _diagnostics;");
        code.AppendLine();

        code.AppendSummary("Gets whether the tree has any errors.");
        code.AppendLine("public bool HasErrors");
        code.OpenBlock();
        code.AppendLine("get");
        code.OpenBlock();
        code.AppendLine("foreach (var d in _diagnostics)");
        code.Indent();
        code.AppendLine("if (d.Severity == DiagnosticSeverity.Error) return true;");
        code.Outdent();
        code.AppendLine("return false;");
        code.CloseBlock();
        code.CloseBlock();
        code.AppendLine();

        code.AppendSummary("Gets the green (internal) root node.");
        code.AppendLine("public GreenNode GreenRoot => _greenRoot;");
        code.AppendLine();

        // Parse methods
        GenerateParseMethod(code, schema);
        code.AppendLine();

        // GetText method
        code.AppendSummary("Gets the text at the specified span.");
        code.AppendLine("public string GetText(TextSpan span)");
        code.OpenBlock();
        code.AppendLine("if (span.Start < 0 || span.End > _sourceText.Length)");
        code.Indent();
        code.AppendLine("throw new ArgumentOutOfRangeException(nameof(span));");
        code.Outdent();
        code.AppendLine("return _sourceText.Substring(span.Start, span.Length);");
        code.CloseBlock();
        code.AppendLine();

        // ToString
        code.AppendSummary("Returns a string representation of the syntax tree.");
        code.AppendLine("public override string ToString() => $\"{GetType().Name}: {_sourceText.Length} chars, {_diagnostics.Length} diagnostics\";");

        code.CloseBlock();
    }

    private static void GenerateParseMethod(CodeBuilder code, SchemaDefinition schema)
    {
        code.AppendSummary("Parses the source text into a syntax tree.");
        code.AppendParam("text", "The source text to parse.");
        code.AppendReturns("A new syntax tree.");
        code.AppendLine($"public static {schema.Classname}SyntaxTree Parse(string text)");
        code.OpenBlock();
        code.AppendLine($"var lexer = new {schema.Classname}Lexer();");
        code.AppendLine("var tokens = new List<GreenToken>(lexer.Tokenize(text));");
        code.AppendLine("var diagnostics = ImmutableArray<Diagnostic>.Empty;");
        code.AppendLine();
        code.AppendLine("// Check for any Bad tokens and create diagnostics");
        code.AppendLine("var diagnosticBuilder = ImmutableArray.CreateBuilder<Diagnostic>();");
        code.AppendLine("int position = 0;");
        code.AppendLine("foreach (var token in tokens)");
        code.OpenBlock();
        code.AppendLine("if ((TokenKind)token.RawKind == TokenKind.Bad)");
        code.OpenBlock();
        code.AppendLine("var span = new TextSpan(position + token.LeadingTriviaWidth, token.Width);");
        code.AppendLine("diagnosticBuilder.Add(new Diagnostic(");
        code.Indent();
        code.AppendLine("\"MP0001\",");
        code.AppendLine("$\"Unexpected character '{token.GetText()}'\",");
        code.AppendLine("DiagnosticSeverity.Error,");
        code.AppendLine("span));");
        code.Outdent();
        code.CloseBlock();
        code.AppendLine("position += token.FullWidth;");
        code.CloseBlock();
        code.AppendLine();
        code.AppendLine("// Create a token list node as the root");
        code.AppendLine("var factory = DefaultGreenTokenFactory.Instance;");
        code.AppendLine("var greenRoot = factory.CreateTokenList(tokens.ToArray());");
        code.AppendLine();
        code.AppendLine($"return new {schema.Classname}SyntaxTree(greenRoot, text, diagnosticBuilder.ToImmutable());");
        code.CloseBlock();
    }
}
