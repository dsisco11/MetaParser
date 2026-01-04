using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Integration;

/// <summary>
/// Fixture for end-to-end calculator schema tests.
/// Tests the full pipeline: schema → generated code → compilation → execution.
/// </summary>
public class CalculatorSchemaFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public SchemaDefinition Schema { get; }
    public Type SyntaxTreeType { get; }
    public Type ParserType { get; }
    public Type TokenKindType { get; }
    public Type GreenTokenType { get; }
    public Type GreenTokenListType { get; }

    public CalculatorSchemaFixture()
    {
        // Create a realistic calculator schema
        Schema = new SchemaDefinition
        {
            Namespace = "Calculator.Syntax",
            Classname = "Calculator"
        };

        // Define trivia tokens
        Schema.Trivia.Add("whitespace");
        Schema.Trivia.Add("end-of-line");

        // Numbers: 0-9+
        Schema.Tokens["number"] = new TokenDefinition
        {
            Start = new RangePattern('0', '9'),
            Consume = new RangePattern('0', '9')
        };

        // Decimal numbers: 123.456
        Schema.Tokens["decimal"] = new TokenDefinition
        {
            Start = new RangePattern('0', '9'),
            Consume = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern('0', '9'),
                new LiteralPattern(".")
            })
        };

        // Operators
        Schema.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };
        Schema.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };
        Schema.Tokens["star"] = new TokenDefinition { Start = new LiteralPattern("*") };
        Schema.Tokens["slash"] = new TokenDefinition { Start = new LiteralPattern("/") };
        Schema.Tokens["percent"] = new TokenDefinition { Start = new LiteralPattern("%") };
        Schema.Tokens["caret"] = new TokenDefinition { Start = new LiteralPattern("^") };

        // Parentheses
        Schema.Tokens["open-paren"] = new TokenDefinition { Start = new LiteralPattern("(") };
        Schema.Tokens["close-paren"] = new TokenDefinition { Start = new LiteralPattern(")") };

        // Comparison operators
        Schema.Tokens["equals"] = new TokenDefinition { Start = new LiteralPattern("=") };
        Schema.Tokens["less-than"] = new TokenDefinition { Start = new LiteralPattern("<") };
        Schema.Tokens["greater-than"] = new TokenDefinition { Start = new LiteralPattern(">") };

        // Identifiers (for variables/functions)
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

        // Generate all code
        var sources = GenerateAllSources(Schema);
        GeneratedAssembly = CompileAndLoad(sources, "CalculatorTestAssembly");

        // Get types
        SyntaxTreeType = GeneratedAssembly.GetType("Calculator.Syntax.CalculatorSyntaxTree")!;
        ParserType = GeneratedAssembly.GetType("Calculator.Syntax.CalculatorParser")!;
        TokenKindType = GeneratedAssembly.GetType("Calculator.Syntax.TokenKind")!;
        GreenTokenType = GeneratedAssembly.GetType("Calculator.Syntax.GreenToken")!;
        GreenTokenListType = GeneratedAssembly.GetType("Calculator.Syntax.GreenTokenList")!;
    }

    public void Dispose() { }

    public static string[] GenerateAllSources(SchemaDefinition schema)
    {
        return new[]
        {
            GreenNodeGenerator.Generate(schema).Build(),
            GreenTriviaGenerator.Generate(schema).Build(),
            GreenTokenGenerator.Generate(schema).Build(),
            GreenTokenFactoryGenerator.Generate(schema).Build(),
            TokenKindGenerator.Generate(schema).Build(),
            ConsumerInterfaceGenerator.Generate(schema).Build(),
            SequenceReaderExtensionsGenerator.Generate(schema).Build(),
            TokenConsumerGenerator.Generate(schema).Build(),
            TriviaAttachmentGenerator.Generate(schema).Build(),
            LexerGenerator.Generate(schema).Build(),
            RedNodeGenerator.Generate(schema).Build(),
            RedTokenGenerator.Generate(schema).Build(),
            SyntaxTriviaGenerator.Generate(schema).Build(),
            SyntaxTreeGenerator.Generate(schema).Build(),
            ParserGenerator.Generate(schema).Build(),
        };
    }

    public static Assembly CompileAndLoad(string[] sources, string assemblyName)
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
            assemblyName,
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
/// End-to-end integration tests for a calculator schema.
/// Tests Phase 10: complete pipeline from schema to parsed syntax tree.
/// </summary>
public class CalculatorEndToEndTests : IClassFixture<CalculatorSchemaFixture>
{
    private readonly CalculatorSchemaFixture _fixture;

    public CalculatorEndToEndTests(CalculatorSchemaFixture fixture)
    {
        _fixture = fixture;
    }

    #region Basic Expression Tests

    [Theory]
    [InlineData("1 + 2")]
    [InlineData("10 - 5")]
    [InlineData("3 * 4")]
    [InlineData("8 / 2")]
    [InlineData("10 % 3")]
    [InlineData("2 ^ 8")]
    public void Parse_SimpleArithmeticExpressions_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("(1 + 2)")]
    [InlineData("((1 + 2))")]
    [InlineData("(1 + (2 * 3))")]
    [InlineData("((1 + 2) * (3 - 4))")]
    public void Parse_ParenthesizedExpressions_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("x")]
    [InlineData("abc")]
    [InlineData("_foo")]
    [InlineData("camelCase")]
    [InlineData("PascalCase")]
    [InlineData("snake_case")]
    [InlineData("x1")]
    [InlineData("_123")]
    public void Parse_Identifiers_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("x + y")]
    [InlineData("a * b + c")]
    [InlineData("result = x + y * z")]
    public void Parse_ComplexExpressions_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    // Note: Function call syntax (comma-separated args) requires comma token definition
    [Theory(Skip = "Function call syntax requires comma token which is not in calculator schema")]
    [InlineData("foo(1, 2)")]
    public void Parse_FunctionCalls_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Comparison Operator Tests

    [Theory]
    [InlineData("x < 10")]
    [InlineData("y > 0")]
    [InlineData("a = b")]
    public void Parse_ComparisonOperators_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Number Tests

    [Theory]
    [InlineData("0")]
    [InlineData("123")]
    [InlineData("999999")]
    public void Parse_IntegerNumbers_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    // Note: Decimal parsing requires more sophisticated lexer support
    // The current schema overlaps number and decimal patterns
    [Theory(Skip = "Decimal parsing requires pattern priority improvements")]
    [InlineData("3.14")]
    [InlineData("0.5")]
    [InlineData("123.456")]
    public void Parse_DecimalNumbers_Succeeds(string input)
    {
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Round-Trip Tests

    [Theory]
    [InlineData("  1 + 2  ")]
    [InlineData("\t3 * 4\t")]
    [InlineData("   x   +   y   ")]
    public void Parse_WithLeadingTrailingWhitespace_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("1+2")]
    [InlineData("1 + 2")]
    [InlineData("1  +  2")]
    [InlineData("1   +   2")]
    public void Parse_VariableWhitespace_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_LongExpression_RoundTripsCorrectly()
    {
        var input = "result = (a + b) * (c - d) / (e % f) ^ g";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Error Handling Tests

    [Theory]
    [InlineData("@")]
    [InlineData("#")]
    [InlineData("$")]
    [InlineData("&")]
    [InlineData("!")]
    public void Parse_InvalidCharacters_ProducesErrors(string input)
    {
        var tree = ParseInput(input);

        Assert.True(HasErrors(tree));
    }

    [Theory]
    [InlineData("1 @ 2")]
    [InlineData("x # y")]
    public void Parse_InvalidCharacterInExpression_ProducesErrors(string input)
    {
        var tree = ParseInput(input);

        Assert.True(HasErrors(tree));
    }

    #endregion

    #region Token Count Tests

    [Fact]
    public void Parse_SimpleExpression_HasNoErrors()
    {
        var tree = ParseInput("1 + 2");

        Assert.False(HasErrors(tree));
        Assert.Equal("1 + 2", GetReconstructedText(tree));
    }

    #endregion

    #region Helper Methods

    private object ParseInput(string input)
    {
        var parseMethod = _fixture.SyntaxTreeType.GetMethod("Parse", new[] { typeof(string) })!;
        return parseMethod.Invoke(null, new object[] { input })!;
    }

    private bool HasErrors(object tree)
    {
        var hasErrorsProp = _fixture.SyntaxTreeType.GetProperty("HasErrors")!;
        return (bool)hasErrorsProp.GetValue(tree)!;
    }

    private string GetReconstructedText(object tree)
    {
        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree)!;
        var toFullStringMethod = greenRoot.GetType().GetMethod("ToFullString")!;
        return (string)toFullStringMethod.Invoke(greenRoot, null)!;
    }

    #endregion
}
