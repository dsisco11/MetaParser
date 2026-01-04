using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Fixture that compiles Phase 4 generated code (lexer infrastructure).
/// </summary>
public class LexerGeneratedCodeFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public Type TokenKindType { get; }
    public Type LexerResultType { get; }
    public Type ITokenConsumerType { get; }
    public Type SequenceReaderExtensionsType { get; }
    public Type WhitespaceConsumerType { get; }
    public Type EndOfLineConsumerType { get; }
    public Type LexerType { get; }

    public SchemaDefinition Schema { get; }

    public LexerGeneratedCodeFixture()
    {
        // Create a test schema with various token types
        Schema = new SchemaDefinition
        {
            Namespace = "TestLexer.Syntax",
            Classname = "Test"
        };

        // Add some tokens
        Schema.Tokens["plus"] = new TokenDefinition
        {
            Start = new LiteralPattern("+")
        };

        Schema.Tokens["minus"] = new TokenDefinition
        {
            Start = new LiteralPattern("-")
        };

        Schema.Tokens["star"] = new TokenDefinition
        {
            Start = new LiteralPattern("*")
        };

        Schema.Tokens["slash"] = new TokenDefinition
        {
            Start = new LiteralPattern("/")
        };

        Schema.Tokens["number"] = new TokenDefinition
        {
            Start = new RangePattern('0', '9'),
            Consume = new RangePattern('0', '9')
        };

        Schema.Tokens["identifier"] = new TokenDefinition
        {
            Start = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern('a', 'z'),
                new RangePattern('A', 'Z'),
                new LiteralPattern("_")
            }),
            Consume = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern('a', 'z'),
                new RangePattern('A', 'Z'),
                new RangePattern('0', '9'),
                new LiteralPattern("_")
            })
        };

        // Generate all source files
        var sources = new[]
        {
            // Phase 3 generators (required dependencies)
            GreenNodeGenerator.Generate(Schema).Build(),
            GreenTriviaGenerator.Generate(Schema).Build(),
            GreenTokenGenerator.Generate(Schema).Build(),
            GreenTokenFactoryGenerator.Generate(Schema).Build(),
            // Phase 4 generators
            TokenKindGenerator.Generate(Schema).Build(),
            ConsumerInterfaceGenerator.Generate(Schema).Build(),
            SequenceReaderExtensionsGenerator.Generate(Schema).Build(),
            TokenConsumerGenerator.Generate(Schema).Build(),
            // Phase 5 generators
            TriviaAttachmentGenerator.Generate(Schema).Build(),
            LexerGenerator.Generate(Schema).Build(),
            // Phase 6 generators (needed for CreateRed references)
            RedNodeGenerator.Generate(Schema).Build(),
            RedTokenGenerator.Generate(Schema).Build(),
            SyntaxTriviaGenerator.Generate(Schema).Build(),
        };

        // Compile
        GeneratedAssembly = CompileAndLoad(sources);

        // Get types
        TokenKindType = GeneratedAssembly.GetType("TestLexer.Syntax.TokenKind")!;
        LexerResultType = GeneratedAssembly.GetType("TestLexer.Syntax.LexerResult")!;
        ITokenConsumerType = GeneratedAssembly.GetType("TestLexer.Syntax.ITokenConsumer")!;
        SequenceReaderExtensionsType = GeneratedAssembly.GetType("TestLexer.Syntax.SequenceReaderExtensions")!;
        WhitespaceConsumerType = GeneratedAssembly.GetType("TestLexer.Syntax.WhitespaceConsumer")!;
        EndOfLineConsumerType = GeneratedAssembly.GetType("TestLexer.Syntax.EndOfLineConsumer")!;
        LexerType = GeneratedAssembly.GetType("TestLexer.Syntax.TestLexer")!;
    }

    public void Dispose() { }

    private static Assembly CompileAndLoad(string[] sources)
    {
        var syntaxTrees = sources.Select(s => CSharpSyntaxTree.ParseText(s)).ToArray();

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Span<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.IO.TextWriter).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Text.StringBuilder).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IFormattable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ISpanFormattable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Buffers.SearchValues<char>).Assembly.Location),
        };

        var runtimePath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Collections.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Linq.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Memory.dll")));

        var compilation = CSharpCompilation.Create(
            "LexerTestAssembly",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => $"{d.Location}: {d.GetMessage()}");
            throw new InvalidOperationException($"Compilation failed:\n{string.Join("\n", errors)}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }
}

/// <summary>
/// Integration tests for Phase 4 generated code.
/// </summary>
public class LexerIntegrationTests : IClassFixture<LexerGeneratedCodeFixture>
{
    private readonly LexerGeneratedCodeFixture _fixture;

    public LexerIntegrationTests(LexerGeneratedCodeFixture fixture)
    {
        _fixture = fixture;
    }

    #region TokenKind Tests

    [Fact]
    public void TokenKind_HasBuiltInValues()
    {
        var endOfFile = Enum.Parse(_fixture.TokenKindType, "EndOfFile");
        var bad = Enum.Parse(_fixture.TokenKindType, "Bad");
        var whitespace = Enum.Parse(_fixture.TokenKindType, "Whitespace");
        var endOfLine = Enum.Parse(_fixture.TokenKindType, "EndOfLine");

        Assert.Equal(0, Convert.ToInt32(endOfFile));
        Assert.Equal(1, Convert.ToInt32(bad));
        Assert.Equal(2, Convert.ToInt32(whitespace));
        Assert.Equal(3, Convert.ToInt32(endOfLine));
    }

    [Fact]
    public void TokenKind_HasSchemaTokens()
    {
        var plus = Enum.Parse(_fixture.TokenKindType, "Plus");
        var minus = Enum.Parse(_fixture.TokenKindType, "Minus");
        var number = Enum.Parse(_fixture.TokenKindType, "Number");
        var identifier = Enum.Parse(_fixture.TokenKindType, "Identifier");

        Assert.NotNull(plus);
        Assert.NotNull(minus);
        Assert.NotNull(number);
        Assert.NotNull(identifier);
    }

    [Fact]
    public void TokenKindExtensions_IsTrivia_ReturnsTrueForWhitespace()
    {
        var extensionsType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.TokenKindExtensions")!;
        var isTrivia = extensionsType.GetMethod("IsTrivia")!;
        var whitespace = Enum.Parse(_fixture.TokenKindType, "Whitespace");
        var endOfLine = Enum.Parse(_fixture.TokenKindType, "EndOfLine");
        var plus = Enum.Parse(_fixture.TokenKindType, "Plus");

        Assert.True((bool)isTrivia.Invoke(null, new[] { whitespace })!);
        Assert.True((bool)isTrivia.Invoke(null, new[] { endOfLine })!);
        Assert.False((bool)isTrivia.Invoke(null, new[] { plus })!);
    }

    [Fact]
    public void TokenKindExtensions_GetText_ReturnsFixedText()
    {
        var extensionsType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.TokenKindExtensions")!;
        var getText = extensionsType.GetMethod("GetText")!;
        var plus = Enum.Parse(_fixture.TokenKindType, "Plus");
        var minus = Enum.Parse(_fixture.TokenKindType, "Minus");
        var number = Enum.Parse(_fixture.TokenKindType, "Number"); // Variable token

        Assert.Equal("+", getText.Invoke(null, new[] { plus }));
        Assert.Equal("-", getText.Invoke(null, new[] { minus }));
        Assert.Null(getText.Invoke(null, new[] { number }));
    }

    #endregion

    #region LexerHelpers Tests

    // Note: LexerHelpers methods take ReadOnlySpan<char> which can't be boxed,
    // so we test them indirectly through the consumers and lexer.

    #endregion

    #region Consumer Tests

    // Note: Consumer.TryConsume takes ReadOnlySpan<char> which can't be boxed,
    // so we test consumers indirectly through the lexer.

    #endregion

    #region Lexer Integration Tests

    [Fact]
    public void Lexer_TokenizeSimpleExpression()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenizeToList = _fixture.LexerType.GetMethod("TokenizeToList", new[] { typeof(string) })!;

        var tokens = (System.Collections.IList)tokenizeToList.Invoke(lexer, new object[] { "1 + 2" })!;

        // Should be: Number, Whitespace, Plus, Whitespace, Number, EOF
        Assert.Equal(6, tokens.Count);
    }

    [Fact]
    public void Lexer_TokenizeIdentifier()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenizeToList = _fixture.LexerType.GetMethod("TokenizeToList", new[] { typeof(string) })!;

        var tokens = (System.Collections.IList)tokenizeToList.Invoke(lexer, new object[] { "hello" })!;

        // Should be: Identifier, EOF
        Assert.Equal(2, tokens.Count);

        var greenTokenType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenToken")!;
        var getText = greenTokenType.GetMethod("GetText")!;
        Assert.Equal("hello", getText.Invoke(tokens[0], null));
    }

    [Fact]
    public void Lexer_PreservesWhitespaceAsTrivia()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenizeToList = _fixture.LexerType.GetMethod("TokenizeToList", new[] { typeof(string) })!;

        var tokens = (System.Collections.IList)tokenizeToList.Invoke(lexer, new object[] { "  x" })!;

        // Raw tokens: Whitespace, Identifier, EOF
        Assert.Equal(3, tokens.Count);
    }

    [Fact]
    public void Lexer_TokenizeWithTrivia_AttachesLeadingTrivia()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;

        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "  x" })!).Cast<object>().ToList();

        // With trivia attachment: should be 1 token with leading trivia + EOF
        Assert.Equal(2, tokens.Count);

        var greenTokenType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenToken")!;
        var leadingTriviaWidth = greenTokenType.GetProperty("LeadingTriviaWidth")!;
        var width = greenTokenType.GetProperty("Width")!;

        // First token should have 2 chars leading trivia (spaces) and 1 char width (x)
        Assert.Equal(2, leadingTriviaWidth.GetValue(tokens[0]));
        Assert.Equal(1, width.GetValue(tokens[0]));
    }

    [Fact]
    public void Lexer_ReconstructsSource()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenizeToList = _fixture.LexerType.GetMethod("TokenizeToList", new[] { typeof(string) })!;
        var greenNodeType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenNode")!;
        var toFullString = greenNodeType.GetMethod("ToFullString")!;

        var input = "1 + 2 * x";
        var tokens = (System.Collections.IList)tokenizeToList.Invoke(lexer, new object[] { input })!;

        var reconstructed = string.Concat(
            tokens.Cast<object>()
                .Take(tokens.Count - 1) // Skip EOF
                .Select(t => (string)toFullString.Invoke(t, null)!)
        );

        Assert.Equal(input, reconstructed);
    }

    [Fact]
    public void Lexer_HandlesUnknownCharacters()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenizeToList = _fixture.LexerType.GetMethod("TokenizeToList", new[] { typeof(string) })!;

        // @ is not in our schema
        var tokens = (System.Collections.IList)tokenizeToList.Invoke(lexer, new object[] { "@" })!;

        // Should be: Bad, EOF
        Assert.Equal(2, tokens.Count);

        var greenTokenType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenToken")!;
        var rawKind = greenTokenType.GetProperty("RawKind")!;
        var badKind = Convert.ToUInt16(Enum.Parse(_fixture.TokenKindType, "Bad"));

        Assert.Equal(badKind, rawKind.GetValue(tokens[0]));
    }

    [Fact]
    public void TriviaAttacher_SplitsTrailingTriviaAtNewline()
    {
        // "x  \n  y" should result in:
        // - 'x' with trailing trivia "  \n"
        // - 'y' with leading trivia "  "
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;

        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "x  \n  y" })!).Cast<object>().ToList();

        // Should be 3 tokens: x, y, EOF
        Assert.Equal(3, tokens.Count);

        var greenTokenType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenToken")!;
        var leadingTriviaWidth = greenTokenType.GetProperty("LeadingTriviaWidth")!;
        var trailingTriviaWidth = greenTokenType.GetProperty("TrailingTriviaWidth")!;
        var width = greenTokenType.GetProperty("Width")!;

        // First token 'x': no leading, 3 chars trailing ("  \n")
        Assert.Equal(0, leadingTriviaWidth.GetValue(tokens[0]));
        Assert.Equal(1, width.GetValue(tokens[0])); // 'x'
        Assert.Equal(3, trailingTriviaWidth.GetValue(tokens[0])); // "  \n"

        // Second token 'y': 2 chars leading ("  "), no trailing
        Assert.Equal(2, leadingTriviaWidth.GetValue(tokens[1]));
        Assert.Equal(1, width.GetValue(tokens[1])); // 'y'
        Assert.Equal(0, trailingTriviaWidth.GetValue(tokens[1]));
    }

    [Fact]
    public void TriviaAttacher_AttachesTriviaToPreviousTokenOnSameLine()
    {
        // "x + y" - spaces on same line should be leading for following token
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;

        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "x + y" })!).Cast<object>().ToList();

        var greenTokenType = _fixture.GeneratedAssembly.GetType("TestLexer.Syntax.GreenToken")!;
        var trailingTriviaWidth = greenTokenType.GetProperty("TrailingTriviaWidth")!;

        // 'x' should have " " as trailing (same line)
        Assert.Equal(1, trailingTriviaWidth.GetValue(tokens[0]));
    }

    #endregion
}
