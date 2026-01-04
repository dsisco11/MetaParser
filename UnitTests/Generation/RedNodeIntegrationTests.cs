using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Fixture that compiles Phase 6 generated code (red node infrastructure).
/// </summary>
public class RedNodeGeneratedCodeFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public Type SyntaxNodeType { get; }
    public Type SyntaxTokenType { get; }
    public Type TextSpanType { get; }
    public Type LexerType { get; }
    public Type GreenTokenType { get; }

    public SchemaDefinition Schema { get; }

    public RedNodeGeneratedCodeFixture()
    {
        Schema = new SchemaDefinition
        {
            Namespace = "TestRedNode.Syntax",
            Classname = "Test"
        };

        // Add some tokens for testing
        Schema.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };
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
            }),
            Consume = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern('a', 'z'),
                new RangePattern('A', 'Z'),
                new RangePattern('0', '9'),
            })
        };

        // Generate all source files
        var sources = new[]
        {
            // Phase 3 generators
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
            // Phase 6 generators
            RedNodeGenerator.Generate(Schema).Build(),
            RedTokenGenerator.Generate(Schema).Build(),
            SyntaxTriviaGenerator.Generate(Schema).Build(),
        };

        GeneratedAssembly = CompileAndLoad(sources);

        SyntaxNodeType = GeneratedAssembly.GetType("TestRedNode.Syntax.SyntaxNode")!;
        SyntaxTokenType = GeneratedAssembly.GetType("TestRedNode.Syntax.SyntaxToken")!;
        TextSpanType = GeneratedAssembly.GetType("TestRedNode.Syntax.TextSpan")!;
        LexerType = GeneratedAssembly.GetType("TestRedNode.Syntax.TestLexer")!;
        GreenTokenType = GeneratedAssembly.GetType("TestRedNode.Syntax.GreenToken")!;
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
            MetadataReference.CreateFromFile(typeof(HashCode).Assembly.Location),
        };

        var runtimePath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Collections.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Linq.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Memory.dll")));

        var compilation = CSharpCompilation.Create(
            "RedNodeTestAssembly",
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
/// Integration tests for Phase 6 generated code.
/// </summary>
public class RedNodeIntegrationTests : IClassFixture<RedNodeGeneratedCodeFixture>
{
    private readonly RedNodeGeneratedCodeFixture _fixture;

    public RedNodeIntegrationTests(RedNodeGeneratedCodeFixture fixture)
    {
        _fixture = fixture;
    }

    #region RedNode Tests

    [Fact]
    public void RedNode_HasExpectedProperties()
    {
        // Verify RedNode has expected properties
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Green"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Parent"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Position"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("EndPosition"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("FullWidth"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Width"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("RawKind"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Span"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("FullSpan"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetProperty("Root"));
    }

    [Fact]
    public void RedNode_HasNavigationMethods()
    {
        Assert.NotNull(_fixture.SyntaxNodeType.GetMethod("Ancestors"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetMethod("AncestorsAndSelf"));
        Assert.NotNull(_fixture.SyntaxNodeType.GetMethod("ToFullString"));
    }

    #endregion

    #region RedToken Tests

    [Fact]
    public void RedToken_InheritsFromRedNode()
    {
        Assert.True(_fixture.SyntaxNodeType.IsAssignableFrom(_fixture.SyntaxTokenType));
    }

    [Fact]
    public void RedToken_HasExpectedProperties()
    {
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("Kind"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("Text"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("ValueText"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("LeadingTriviaWidth"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("TrailingTriviaWidth"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("HasLeadingTrivia"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("HasTrailingTrivia"));
        Assert.NotNull(_fixture.SyntaxTokenType.GetProperty("IsMissing"));
    }

    [Fact]
    public void RedToken_CanBeCreatedFromGreenToken()
    {
        // Create a lexer and tokenize
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;
        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "abc" })!).Cast<object>().ToList();

        // Get the first green token (should be 'abc')
        var greenToken = tokens[0];

        // Create a RedToken from it
        var redTokenCtor = _fixture.SyntaxTokenType.GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new[] { _fixture.GreenTokenType, _fixture.SyntaxNodeType, typeof(int), typeof(int) },
            null)!;

        var redToken = redTokenCtor.Invoke(new object?[] { greenToken, null, 0, -1 });

        // Verify properties
        var textProp = _fixture.SyntaxTokenType.GetProperty("Text")!;
        var positionProp = _fixture.SyntaxNodeType.GetProperty("Position")!;

        Assert.Equal("abc", textProp.GetValue(redToken));
        Assert.Equal(0, positionProp.GetValue(redToken));
    }

    [Fact]
    public void RedToken_PositionCalculatesCorrectly()
    {
        // Tokenize "  x" (2 spaces + identifier)
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;
        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "  x" })!).Cast<object>().ToList();

        // First token should be 'x' with leading trivia
        var greenToken = tokens[0];

        // Create red token at position 0 (start of source)
        var redTokenCtor = _fixture.SyntaxTokenType.GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new[] { _fixture.GreenTokenType, _fixture.SyntaxNodeType, typeof(int), typeof(int) },
            null)!;

        var redToken = redTokenCtor.Invoke(new object?[] { greenToken, null, 0, -1 });

        var positionProp = _fixture.SyntaxNodeType.GetProperty("Position")!;
        var fullWidthProp = _fixture.SyntaxNodeType.GetProperty("FullWidth")!;
        var endPositionProp = _fixture.SyntaxNodeType.GetProperty("EndPosition")!;
        var leadingTriviaWidthProp = _fixture.SyntaxTokenType.GetProperty("LeadingTriviaWidth")!;

        Assert.Equal(0, positionProp.GetValue(redToken));
        Assert.Equal(3, fullWidthProp.GetValue(redToken)); // 2 spaces + 1 char
        Assert.Equal(3, endPositionProp.GetValue(redToken));
        Assert.Equal(2, leadingTriviaWidthProp.GetValue(redToken));
    }

    #endregion

    #region TextSpan Tests

    [Fact]
    public void TextSpan_HasExpectedProperties()
    {
        Assert.NotNull(_fixture.TextSpanType.GetProperty("Start"));
        Assert.NotNull(_fixture.TextSpanType.GetProperty("Length"));
        Assert.NotNull(_fixture.TextSpanType.GetProperty("End"));
        Assert.NotNull(_fixture.TextSpanType.GetProperty("IsEmpty"));
    }

    [Fact]
    public void TextSpan_CalculatesEndCorrectly()
    {
        var ctor = _fixture.TextSpanType.GetConstructor(new[] { typeof(int), typeof(int) })!;
        var span = ctor.Invoke(new object[] { 5, 10 });

        var startProp = _fixture.TextSpanType.GetProperty("Start")!;
        var lengthProp = _fixture.TextSpanType.GetProperty("Length")!;
        var endProp = _fixture.TextSpanType.GetProperty("End")!;

        Assert.Equal(5, startProp.GetValue(span));
        Assert.Equal(10, lengthProp.GetValue(span));
        Assert.Equal(15, endProp.GetValue(span));
    }

    [Fact]
    public void TextSpan_ContainsPosition()
    {
        var ctor = _fixture.TextSpanType.GetConstructor(new[] { typeof(int), typeof(int) })!;
        var span = ctor.Invoke(new object[] { 5, 10 }); // [5..15)

        var containsMethod = _fixture.TextSpanType.GetMethod("Contains", new[] { typeof(int) })!;

        Assert.True((bool)containsMethod.Invoke(span, new object[] { 5 })!);
        Assert.True((bool)containsMethod.Invoke(span, new object[] { 10 })!);
        Assert.True((bool)containsMethod.Invoke(span, new object[] { 14 })!);
        Assert.False((bool)containsMethod.Invoke(span, new object[] { 4 })!);
        Assert.False((bool)containsMethod.Invoke(span, new object[] { 15 })!);
    }

    [Fact]
    public void TextSpan_Equality()
    {
        var ctor = _fixture.TextSpanType.GetConstructor(new[] { typeof(int), typeof(int) })!;
        var span1 = ctor.Invoke(new object[] { 5, 10 });
        var span2 = ctor.Invoke(new object[] { 5, 10 });
        var span3 = ctor.Invoke(new object[] { 5, 11 });

        Assert.Equal(span1, span2);
        Assert.NotEqual(span1, span3);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void RedToken_SpanExcludesTrivia()
    {
        // Tokenize "  x  " with leading and trailing trivia
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;
        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "  x  " })!).Cast<object>().ToList();

        var greenToken = tokens[0]; // 'x' with trivia

        var redTokenCtor = _fixture.SyntaxTokenType.GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new[] { _fixture.GreenTokenType, _fixture.SyntaxNodeType, typeof(int), typeof(int) },
            null)!;

        var redToken = redTokenCtor.Invoke(new object?[] { greenToken, null, 0, -1 });

        var spanProp = _fixture.SyntaxTokenType.GetProperty("Span")!;
        var fullSpanProp = _fixture.SyntaxNodeType.GetProperty("FullSpan")!;
        var span = spanProp.GetValue(redToken)!;
        var fullSpan = fullSpanProp.GetValue(redToken)!;

        var startProp = _fixture.TextSpanType.GetProperty("Start")!;
        var lengthProp = _fixture.TextSpanType.GetProperty("Length")!;

        // Span should be just 'x' (position 2, length 1)
        Assert.Equal(2, startProp.GetValue(span));
        Assert.Equal(1, lengthProp.GetValue(span));

        // FullSpan should include trivia (position 0, length 5)
        Assert.Equal(0, startProp.GetValue(fullSpan));
        Assert.Equal(5, lengthProp.GetValue(fullSpan));
    }

    [Fact]
    public void RedToken_ParentIsNullForRootToken()
    {
        var lexer = Activator.CreateInstance(_fixture.LexerType, new object?[] { null });
        var tokenize = _fixture.LexerType.GetMethod("Tokenize", new[] { typeof(string) })!;
        var tokens = ((System.Collections.IEnumerable)tokenize.Invoke(lexer, new object[] { "x" })!).Cast<object>().ToList();

        var greenToken = tokens[0];

        var redTokenCtor = _fixture.SyntaxTokenType.GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new[] { _fixture.GreenTokenType, _fixture.SyntaxNodeType, typeof(int), typeof(int) },
            null)!;

        var redToken = redTokenCtor.Invoke(new object?[] { greenToken, null, 0, -1 });

        var parentProp = _fixture.SyntaxNodeType.GetProperty("Parent")!;
        Assert.Null(parentProp.GetValue(redToken));
    }

    #endregion
}
