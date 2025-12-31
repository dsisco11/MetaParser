using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Fixture that compiles Phase 7 generated code (parser and syntax tree).
/// </summary>
public class ParserGeneratedCodeFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public Type SyntaxTreeType { get; }
    public Type ParserType { get; }
    public Type DiagnosticType { get; }
    public Type DiagnosticSeverityType { get; }
    public Type GreenTokenListType { get; }

    public SchemaDefinition Schema { get; }

    public ParserGeneratedCodeFixture()
    {
        Schema = new SchemaDefinition
        {
            Namespace = "TestParser.Syntax",
            Classname = "Test"
        };

        // Add tokens for testing
        Schema.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };
        Schema.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };
        Schema.Tokens["star"] = new TokenDefinition { Start = new LiteralPattern("*") };
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
            // Phase 7 generators
            SyntaxTreeGenerator.Generate(Schema).Build(),
            ParserGenerator.Generate(Schema).Build(),
        };

        GeneratedAssembly = CompileAndLoad(sources);

        SyntaxTreeType = GeneratedAssembly.GetType("TestParser.Syntax.TestSyntaxTree")!;
        ParserType = GeneratedAssembly.GetType("TestParser.Syntax.TestParser")!;
        DiagnosticType = GeneratedAssembly.GetType("TestParser.Syntax.Diagnostic")!;
        DiagnosticSeverityType = GeneratedAssembly.GetType("TestParser.Syntax.DiagnosticSeverity")!;
        GreenTokenListType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenList")!;
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
            MetadataReference.CreateFromFile(typeof(System.Collections.Immutable.ImmutableArray<>).Assembly.Location),
        };

        var runtimePath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Collections.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Linq.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Memory.dll")));

        var compilation = CSharpCompilation.Create(
            "ParserTestAssembly",
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
/// Integration tests for Phase 7 generated code.
/// </summary>
public class ParserIntegrationTests : IClassFixture<ParserGeneratedCodeFixture>
{
    private readonly ParserGeneratedCodeFixture _fixture;

    public ParserIntegrationTests(ParserGeneratedCodeFixture fixture)
    {
        _fixture = fixture;
    }

    #region SyntaxTree Tests

    [Fact]
    public void SyntaxTree_HasExpectedProperties()
    {
        Assert.NotNull(_fixture.SyntaxTreeType.GetProperty("SourceText"));
        Assert.NotNull(_fixture.SyntaxTreeType.GetProperty("Length"));
        Assert.NotNull(_fixture.SyntaxTreeType.GetProperty("Diagnostics"));
        Assert.NotNull(_fixture.SyntaxTreeType.GetProperty("HasErrors"));
        Assert.NotNull(_fixture.SyntaxTreeType.GetProperty("GreenRoot"));
    }

    [Fact]
    public void SyntaxTree_ParseReturnsTree()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var tree = parseMethod.Invoke(null, new object[] { "x + y" });

        Assert.NotNull(tree);
    }

    [Fact]
    public void SyntaxTree_SourceTextIsPreserved()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "abc + 123";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var sourceTextProp = _fixture.SyntaxTreeType.GetProperty("SourceText")!;
        Assert.Equal(input, sourceTextProp.GetValue(tree));
    }

    [Fact]
    public void SyntaxTree_LengthMatchesSourceText()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "test 123";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var lengthProp = _fixture.SyntaxTreeType.GetProperty("Length")!;
        Assert.Equal(input.Length, lengthProp.GetValue(tree));
    }

    [Fact]
    public void SyntaxTree_GreenRootIsTokenList()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var tree = parseMethod.Invoke(null, new object[] { "x" })!;

        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree);

        Assert.IsType(_fixture.GreenTokenListType, greenRoot);
    }

    #endregion

    #region Parser Tests

    [Fact]
    public void Parser_CanBeInstantiated()
    {
        var parser = Activator.CreateInstance(_fixture.ParserType);
        Assert.NotNull(parser);
    }

    [Fact]
    public void Parser_StaticParseReturnsSyntaxTree()
    {
        var parseMethod = _fixture.ParserType.GetMethod("Parse", new[] { typeof(string) })!;
        var tree = parseMethod.Invoke(null, new object[] { "x + y" });

        Assert.NotNull(tree);
        Assert.IsType(_fixture.SyntaxTreeType, tree);
    }

    [Fact]
    public void Parser_ParseTextReturnsSyntaxTree()
    {
        var parser = Activator.CreateInstance(_fixture.ParserType)!;
        var parseTextMethod = _fixture.ParserType.GetMethod("ParseText", new[] { typeof(string) })!;
        var tree = parseTextMethod.Invoke(parser, new object[] { "x + y" });

        Assert.NotNull(tree);
        Assert.IsType(_fixture.SyntaxTreeType, tree);
    }

    [Fact]
    public void Parser_TokenizeReturnsTokens()
    {
        var parser = Activator.CreateInstance(_fixture.ParserType)!;
        var tokenizeMethod = _fixture.ParserType.GetMethod("Tokenize", new[] { typeof(string) })!;
        var tokens = ((System.Collections.IEnumerable)tokenizeMethod.Invoke(parser, new object[] { "x + y" })!).Cast<object>().ToList();

        // Should have: identifier, whitespace+plus, whitespace+identifier, EOF
        Assert.True(tokens.Count >= 3);
    }

    #endregion

    #region Diagnostic Tests

    [Fact]
    public void Diagnostic_HasExpectedProperties()
    {
        Assert.NotNull(_fixture.DiagnosticType.GetProperty("Id"));
        Assert.NotNull(_fixture.DiagnosticType.GetProperty("Message"));
        Assert.NotNull(_fixture.DiagnosticType.GetProperty("Severity"));
        Assert.NotNull(_fixture.DiagnosticType.GetProperty("Span"));
    }

    [Fact]
    public void SyntaxTree_NoErrorsForValidInput()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var tree = parseMethod.Invoke(null, new object[] { "x + 123" })!;

        var hasErrorsProp = _fixture.SyntaxTreeType.GetProperty("HasErrors")!;
        Assert.False((bool)hasErrorsProp.GetValue(tree)!);
    }

    [Fact]
    public void SyntaxTree_HasErrorsForInvalidInput()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        // @ is not a valid token
        var tree = parseMethod.Invoke(null, new object[] { "x @ y" })!;

        var hasErrorsProp = _fixture.SyntaxTreeType.GetProperty("HasErrors")!;
        Assert.True((bool)hasErrorsProp.GetValue(tree)!);

        var diagnosticsProp = _fixture.SyntaxTreeType.GetProperty("Diagnostics")!;
        var diagnostics = diagnosticsProp.GetValue(tree);
        var countProp = diagnostics!.GetType().GetProperty("Length")!;
        Assert.True((int)countProp.GetValue(diagnostics)! > 0);
    }

    [Fact]
    public void SyntaxTree_DiagnosticHasCorrectId()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var tree = parseMethod.Invoke(null, new object[] { "@" })!;

        var diagnosticsProp = _fixture.SyntaxTreeType.GetProperty("Diagnostics")!;
        var diagnostics = diagnosticsProp.GetValue(tree)!;

        // Get first diagnostic
        var indexer = diagnostics.GetType().GetMethod("get_Item")!;
        var diagnostic = indexer.Invoke(diagnostics, new object[] { 0 })!;

        var idProp = _fixture.DiagnosticType.GetProperty("Id")!;
        Assert.Equal("MP0001", idProp.GetValue(diagnostic));
    }

    #endregion

    #region Round-Trip Tests

    [Fact]
    public void Parser_RoundTripsSimpleExpression()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "x + y";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree)!;

        var toFullStringMethod = greenRoot.GetType().GetMethod("ToFullString")!;
        var reconstructed = (string)toFullStringMethod.Invoke(greenRoot, null)!;

        Assert.Equal(input, reconstructed);
    }

    [Fact]
    public void Parser_RoundTripsComplexExpression()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "  abc123 + 456 * xyz  ";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree)!;

        var toFullStringMethod = greenRoot.GetType().GetMethod("ToFullString")!;
        var reconstructed = (string)toFullStringMethod.Invoke(greenRoot, null)!;

        Assert.Equal(input, reconstructed);
    }

    [Fact]
    public void Parser_RoundTripsWithNewlines()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "x\n  +\n  y";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree)!;

        var toFullStringMethod = greenRoot.GetType().GetMethod("ToFullString")!;
        var reconstructed = (string)toFullStringMethod.Invoke(greenRoot, null)!;

        Assert.Equal(input, reconstructed);
    }

    #endregion

    #region GetText Tests

    [Fact]
    public void SyntaxTree_GetTextReturnsCorrectSpan()
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        var input = "hello world";
        var tree = parseMethod.Invoke(null, new object[] { input })!;

        var textSpanType = _fixture.GeneratedAssembly.GetType("TestParser.Syntax.TextSpan")!;
        var spanCtor = textSpanType.GetConstructor(new[] { typeof(int), typeof(int) })!;
        var span = spanCtor.Invoke(new object[] { 6, 5 }); // "world"

        var getTextMethod = _fixture.SyntaxTreeType.GetMethod("GetText")!;
        var text = (string)getTextMethod.Invoke(tree, new object[] { span! })!;

        Assert.Equal("world", text);
    }

    #endregion
}
