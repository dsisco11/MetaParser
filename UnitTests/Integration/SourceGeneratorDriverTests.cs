using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using MetaParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace UnitTests.Integration;

/// <summary>
/// Tests that use CSharpGeneratorDriver to run the actual source generator.
/// These tests verify the complete integration from .metaparser.json to compiled code.
/// </summary>
public class SourceGeneratorDriverTests
{
    private const string CalculatorSchema = """
        {
            "$schema": "https://raw.githubusercontent.com/user/metaparser/main/schema.json",
            "namespace": "Calculator.Syntax",
            "classname": "CalculatorParser",
            "tokens": {
                "whitespace": {
                    "start": [" ", "\t"],
                    "consume": [" ", "\t"]
                },
                "newline": {
                    "start": ["\r\n", "\n", "\r"]
                },
                "number": {
                    "start": { "range": ["0", "9"] },
                    "consume": { "range": ["0", "9"] }
                },
                "plus": { "start": "+" },
                "minus": { "start": "-" },
                "star": { "start": "*" },
                "slash": { "start": "/" },
                "lparen": { "start": "(" },
                "rparen": { "start": ")" }
            },
            "trivia": ["whitespace", "newline"]
        }
        """;

    [Fact]
    public void Generator_ProducesExpectedFiles()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var additionalText = new InMemoryAdditionalText("calculator.metaparser.json", CalculatorSchema);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        // Assert - no generator errors
        var generatorErrors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.Empty(generatorErrors);

        // Assert - expected files generated
        var generatedTrees = outputCompilation.SyntaxTrees.Skip(1).ToArray(); // Skip the empty input
        Assert.True(generatedTrees.Length >= 15, $"Expected at least 15 generated files, got {generatedTrees.Length}");

        // Verify key files exist
        var fileNames = generatedTrees.Select(t => Path.GetFileName(t.FilePath)).ToArray();
        Assert.Contains("GreenNode.g.cs", fileNames);
        Assert.Contains("GreenToken.g.cs", fileNames);
        Assert.Contains("TokenKind.g.cs", fileNames);
        Assert.Contains("Lexer.g.cs", fileNames);
        Assert.Contains("Parser.g.cs", fileNames);
        Assert.Contains("SyntaxTree.g.cs", fileNames);
    }

    [Fact]
    public void Generator_ProducesCompilableCode()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var additionalText = new InMemoryAdditionalText("calculator.metaparser.json", CalculatorSchema);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        // Assert - compilation succeeds
        var compilationDiagnostics = outputCompilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToArray();

        Assert.Empty(compilationDiagnostics);
    }

    [Fact]
    public void Generator_GeneratedCodeCanParseInput()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var additionalText = new InMemoryAdditionalText("calculator.metaparser.json", CalculatorSchema);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act - generate and compile
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);

        // Compile to assembly
        using var ms = new MemoryStream();
        var emitResult = outputCompilation.Emit(ms);
        Assert.True(emitResult.Success, string.Join("\n", emitResult.Diagnostics.Select(d => d.GetMessage())));

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());

        // Get parser type and parse method
        // Note: The generator appends "Parser" to the classname, so "CalculatorParser" becomes "CalculatorParserParser"
        var parserType = assembly.GetType("Calculator.Syntax.CalculatorParserParser");
        Assert.NotNull(parserType);

        var parseMethod = parserType.GetMethod("Parse", new[] { typeof(string) });
        Assert.NotNull(parseMethod);

        // Parse test input
        var syntaxTree = parseMethod.Invoke(null, new object[] { "1 + 2 * 3" });
        Assert.NotNull(syntaxTree);

        // Get the green root and verify we can reconstruct source
        var greenRootProp = syntaxTree.GetType().GetProperty("GreenRoot");
        Assert.NotNull(greenRootProp);

        var greenRoot = greenRootProp.GetValue(syntaxTree);
        Assert.NotNull(greenRoot);

        var toFullStringMethod = greenRoot!.GetType().GetMethod("ToFullString");
        Assert.NotNull(toFullStringMethod);

        var reconstructed = (string)toFullStringMethod.Invoke(greenRoot, null)!;
        Assert.Equal("1 + 2 * 3", reconstructed);
    }

    [Fact]
    public void Generator_InvalidSchema_ReportsError()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var invalidSchema = """{ "namespace": "Test", "tokens": { "a": { "start": { "$token": "b" } }, "b": { "start": { "$token": "a" } } } }""";
        var additionalText = new InMemoryAdditionalText("invalid.metaparser.json", invalidSchema);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act
        driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics);

        // Assert - should have validation error for circular reference
        var errors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.GetMessage().Contains("circular", StringComparison.OrdinalIgnoreCase));

        // Assert - error should have file location
        var firstError = errors[0];
        Assert.NotEqual(Location.None, firstError.Location);
        Assert.Contains("invalid.metaparser.json", firstError.Location.GetLineSpan().Path);
    }

    [Fact]
    public void Generator_MalformedJson_ReportsError()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var malformedJson = "{ this is not valid json }";
        var additionalText = new InMemoryAdditionalText("bad.metaparser.json", malformedJson);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        // Assert - no generated files (schema parse failed)
        var generatedTrees = outputCompilation.SyntaxTrees.Skip(1).ToArray();
        Assert.Empty(generatedTrees);
    }

    [Fact]
    public void Generator_ErrorRecovery_ContinuesAfterBadToken()
    {
        // Arrange
        var generator = new Generator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CreateCompilation();
        var additionalText = new InMemoryAdditionalText("calculator.metaparser.json", CalculatorSchema);

        driver = driver.AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(additionalText));

        // Act - generate and compile
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);

        using var ms = new MemoryStream();
        var emitResult = outputCompilation.Emit(ms);
        Assert.True(emitResult.Success, string.Join("\n", emitResult.Diagnostics.Select(d => d.GetMessage())));

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());

        // Get parser and parse input with invalid characters
        var parserType = assembly.GetType("Calculator.Syntax.CalculatorParserParser")!;
        var parseMethod = parserType.GetMethod("Parse", new[] { typeof(string) })!;

        // Parse input with @ and # which are not recognized tokens
        var syntaxTree = parseMethod.Invoke(null, new object[] { "1 + @ + 2 # 3" });
        Assert.NotNull(syntaxTree);

        // Verify diagnostics were created for bad tokens
        var diagnosticsProp = syntaxTree!.GetType().GetProperty("Diagnostics");
        Assert.NotNull(diagnosticsProp);

        var diagnostics = diagnosticsProp.GetValue(syntaxTree);
        Assert.NotNull(diagnostics);

        // Get the Length property from ImmutableArray
        var lengthProp = diagnostics!.GetType().GetProperty("Length");
        var count = (int)lengthProp!.GetValue(diagnostics)!;

        // Should have 2 diagnostics (for @ and #)
        Assert.Equal(2, count);

        // Verify the source can still be reconstructed (error recovery worked)
        var greenRootProp = syntaxTree.GetType().GetProperty("GreenRoot");
        var greenRoot = greenRootProp!.GetValue(syntaxTree);
        var toFullStringMethod = greenRoot!.GetType().GetMethod("ToFullString");
        var reconstructed = (string)toFullStringMethod!.Invoke(greenRoot, null)!;

        Assert.Equal("1 + @ + 2 # 3", reconstructed);
    }

    private static CSharpCompilation CreateCompilation()
    {
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Span<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Buffers.SearchValues<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Collections.Immutable.ImmutableArray<>).Assembly.Location),
        };

        // Add runtime assemblies
        var runtimeDir = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "System.Collections.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "System.Memory.dll")));

        return CSharpCompilation.Create(
            "TestAssembly",
            new[] { CSharpSyntaxTree.ParseText("") },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable));
    }

    /// <summary>
    /// In-memory implementation of AdditionalText for testing.
    /// </summary>
    private sealed class InMemoryAdditionalText : AdditionalText
    {
        private readonly string _path;
        private readonly string _content;

        public InMemoryAdditionalText(string path, string content)
        {
            _path = path;
            _content = content;
        }

        public override string Path => _path;

        public override SourceText? GetText(CancellationToken cancellationToken = default)
        {
            return SourceText.From(_content, Encoding.UTF8);
        }
    }
}
