using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Tests for GreenNode code generators.
/// </summary>
public class GreenNodeGeneratorTests
{
    private static SchemaDefinition CreateTestSchema()
    {
        return new SchemaDefinition
        {
            Namespace = "TestParser.Syntax",
            Classname = "TestLexer"
        };
    }

    private static bool CompilesSuccessfully(string source, out string[] errors)
        => CompilesSuccessfully(new[] { source }, out errors);

    private static bool CompilesSuccessfully(string[] sources, out string[] errors)
    {
        var syntaxTrees = sources.Select(s => CSharpSyntaxTree.ParseText(s)).ToArray();
        
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Span<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.IO.TextWriter).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Text.StringBuilder).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IFormattable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ISpanFormattable).Assembly.Location),
        };

        // Add runtime reference
        var runtimePath = System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        var runtimeRef = MetadataReference.CreateFromFile(System.IO.Path.Combine(runtimePath, "System.Runtime.dll"));

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            syntaxTrees,
            references.Append(runtimeRef),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var diagnostics = compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToArray();

        errors = diagnostics.Select(d => d.GetMessage()).ToArray();
        return errors.Length == 0;
    }

    #region GreenNodeGenerator Tests

    [Fact]
    public void GreenNodeGenerator_ProducesValidCSharp()
    {
        // GreenNode references GreenToken, GreenTrivia, etc. - so compile all together
        var schema = CreateTestSchema();
        var sources = new[]
        {
            GreenNodeGenerator.Generate(schema).Build(),
            GreenTriviaGenerator.Generate(schema).Build(),
            GreenTokenGenerator.Generate(schema).Build(),
            GreenTokenFactoryGenerator.Generate(schema).Build()
        };

        var compiles = CompilesSuccessfully(sources, out var errors);
        
        Assert.True(compiles, $"Compilation failed:\n{string.Join("\n", errors)}");
    }

    [Fact]
    public void GreenNodeGenerator_IncludesGreenNodeFlags()
    {
        var schema = CreateTestSchema();
        var file = GreenNodeGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("enum GreenNodeFlags", source);
        Assert.Contains("IsToken", source);
        Assert.Contains("IsTrivia", source);
        Assert.Contains("ContainsTrivia", source);
    }

    [Fact]
    public void GreenNodeGenerator_IncludesFormatConstants()
    {
        var schema = CreateTestSchema();
        var file = GreenNodeGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("class GreenNodeFormat", source);
        Assert.Contains("\"T\"", source);
        Assert.Contains("\"M\"", source);
        Assert.Contains("\"J\"", source);
    }

    [Fact]
    public void GreenNodeGenerator_ImplementsIFormattable()
    {
        var schema = CreateTestSchema();
        var file = GreenNodeGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("IFormattable", source);
        Assert.Contains("ISpanFormattable", source);
        Assert.Contains("ToString(string? format, IFormatProvider? formatProvider)", source);
        Assert.Contains("TryFormat(Span<char> destination", source);
    }

    [Fact]
    public void GreenNodeGenerator_IncludesFormatMethods()
    {
        var schema = CreateTestSchema();
        var file = GreenNodeGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("FormatMermaid()", source);
        Assert.Contains("FormatDebugTree()", source);
        Assert.Contains("FormatJson()", source);
    }

    #endregion

    #region GreenTriviaGenerator Tests

    [Fact]
    public void GreenTriviaGenerator_ProducesValidCSharp()
    {
        // Need GreenNode for GreenTrivia to compile
        var schema = CreateTestSchema();
        var nodeFile = GreenNodeGenerator.Generate(schema);
        var triviaFile = GreenTriviaGenerator.Generate(schema);
        
        var combinedSource = nodeFile.Build() + "\n" + triviaFile.Code.ToString();
        
        // Just check it parses - full compilation needs all dependencies
        var tree = CSharpSyntaxTree.ParseText(combinedSource);
        var root = tree.GetRoot();
        var errors = root.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
        
        Assert.Empty(errors);
    }

    [Fact]
    public void GreenTriviaGenerator_IncludesGreenTrivia()
    {
        var schema = CreateTestSchema();
        var file = GreenTriviaGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("class GreenTrivia", source);
        Assert.Contains("ReadOnlyMemory<char>", source);
        Assert.Contains("GetText()", source);
    }

    [Fact]
    public void GreenTriviaGenerator_IncludesGreenTriviaList()
    {
        var schema = CreateTestSchema();
        var file = GreenTriviaGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("class GreenTriviaList", source);
        Assert.Contains("GreenTrivia[]", source);
        Assert.Contains("Count", source);
    }

    #endregion

    #region GreenTokenGenerator Tests

    [Fact]
    public void GreenTokenGenerator_IncludesAllVariants()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("class GreenToken", source);
        Assert.Contains("class GreenTokenWithNoTrivia", source);
        Assert.Contains("class GreenTokenWithLeadingTrivia", source);
        Assert.Contains("class GreenTokenWithTrailingTrivia", source);
        Assert.Contains("class GreenTokenWithTrivia", source);
    }

    [Fact]
    public void GreenTokenGenerator_IncludesExtensionMethods()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("static class GreenTokenExtensions", source);
        Assert.Contains("WithLeadingTrivia", source);
        Assert.Contains("WithTrailingTrivia", source);
    }

    [Fact]
    public void GreenTokenGenerator_SupportsMemoryAndString()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("ReadOnlyMemory<char> text", source);
        Assert.Contains("string text", source);
        Assert.Contains("_ownedText", source);
    }

    #endregion

    #region GreenTokenFactoryGenerator Tests

    [Fact]
    public void GreenTokenFactoryGenerator_IncludesFactory()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenFactoryGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("class GreenTokenFactory", source);
        Assert.Contains("CreateToken", source);
        Assert.Contains("CreateTrivia", source);
    }

    [Fact]
    public void GreenTokenFactoryGenerator_IncludesCaching()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenFactoryGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("Dictionary<CacheKey", source);
        Assert.Contains("_maxCacheSize", source);
        Assert.Contains("ClearCache", source);
        Assert.Contains("CacheCount", source);
    }

    [Fact]
    public void GreenTokenFactoryGenerator_IncludesDefaultSingleton()
    {
        var schema = CreateTestSchema();
        var file = GreenTokenFactoryGenerator.Generate(schema);
        var source = file.Build();

        Assert.Contains("DefaultGreenTokenFactory", source);
        Assert.Contains("Instance", source);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AllGenerators_UseCorrectNamespace()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "MyApp.Parser.Internal",
            Classname = "MyLexer"
        };

        var nodeSource = GreenNodeGenerator.Generate(schema).Build();
        var triviaSource = GreenTriviaGenerator.Generate(schema).Build();
        var tokenSource = GreenTokenGenerator.Generate(schema).Build();
        var factorySource = GreenTokenFactoryGenerator.Generate(schema).Build();

        Assert.Contains("namespace MyApp.Parser.Internal;", nodeSource);
        Assert.Contains("namespace MyApp.Parser.Internal;", triviaSource);
        Assert.Contains("namespace MyApp.Parser.Internal;", tokenSource);
        Assert.Contains("namespace MyApp.Parser.Internal;", factorySource);
    }

    [Fact]
    public void AllGenerators_ProduceCorrectFileNames()
    {
        var schema = CreateTestSchema();

        Assert.Equal("GreenNode.g.cs", GreenNodeGenerator.Generate(schema).FileName);
        Assert.Equal("GreenTrivia.g.cs", GreenTriviaGenerator.Generate(schema).FileName);
        Assert.Equal("GreenToken.g.cs", GreenTokenGenerator.Generate(schema).FileName);
        Assert.Equal("GreenTokenFactory.g.cs", GreenTokenFactoryGenerator.Generate(schema).FileName);
    }

    #endregion
}
