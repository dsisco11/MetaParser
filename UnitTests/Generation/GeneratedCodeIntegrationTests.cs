using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Fixture that compiles generated code once and shares it across all tests.
/// </summary>
public class GeneratedCodeFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public Type GreenNodeType { get; }
    public Type GreenTriviaType { get; }
    public Type GreenTriviaListType { get; }
    public Type GreenTokenType { get; }
    public Type GreenTokenWithNoTriviaType { get; }
    public Type GreenTokenWithLeadingTriviaType { get; }
    public Type GreenTokenWithTrailingTriviaType { get; }
    public Type GreenTokenWithTriviaType { get; }
    public Type GreenTokenFactoryType { get; }

    public GeneratedCodeFixture()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "TestParser.Syntax",
            Classname = "TestLexer"
        };

        // Generate all source files
        var sources = new[]
        {
            GreenNodeGenerator.Generate(schema).Build(),
            GreenTriviaGenerator.Generate(schema).Build(),
            GreenTokenGenerator.Generate(schema).Build(),
            GreenTokenFactoryGenerator.Generate(schema).Build()
        };

        // Compile once
        GeneratedAssembly = CompileAndLoad(sources);

        // Get types once
        GreenNodeType = GeneratedAssembly.GetType("TestParser.Syntax.GreenNode")!;
        GreenTriviaType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTrivia")!;
        GreenTriviaListType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTriviaList")!;
        GreenTokenType = GeneratedAssembly.GetType("TestParser.Syntax.GreenToken")!;
        GreenTokenWithNoTriviaType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenWithNoTrivia")!;
        GreenTokenWithLeadingTriviaType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenWithLeadingTrivia")!;
        GreenTokenWithTrailingTriviaType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenWithTrailingTrivia")!;
        GreenTokenWithTriviaType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenWithTrivia")!;
        GreenTokenFactoryType = GeneratedAssembly.GetType("TestParser.Syntax.GreenTokenFactory")!;
    }

    public void Dispose()
    {
        // Assembly unloading not needed for test lifetime
    }

    public object CreateFactory()
    {
        // Factory has constructor with default parameter: GreenTokenFactory(int maxCacheSize = 1024)
        return Activator.CreateInstance(GreenTokenFactoryType, 1024)!;
    }

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
        };

        // Add runtime assemblies
        var runtimePath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(runtimePath, "System.Collections.dll")));

        var compilation = CSharpCompilation.Create(
            "GeneratedTestAssembly",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.GetMessage());
            throw new InvalidOperationException($"Compilation failed:\n{string.Join("\n", errors)}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }
}

/// <summary>
/// Integration tests that compile generated code once and run tests against the output.
/// </summary>
public class GeneratedCodeIntegrationTests : IClassFixture<GeneratedCodeFixture>
{
    private readonly GeneratedCodeFixture _fixture;

    public GeneratedCodeIntegrationTests(GeneratedCodeFixture fixture)
    {
        _fixture = fixture;
    }

    // Convenience accessors
    private Type GreenNodeType => _fixture.GreenNodeType;
    private Type GreenTriviaType => _fixture.GreenTriviaType;
    private Type GreenTriviaListType => _fixture.GreenTriviaListType;
    private Type GreenTokenType => _fixture.GreenTokenType;
    private Type GreenTokenWithNoTriviaType => _fixture.GreenTokenWithNoTriviaType;
    private Type GreenTokenWithLeadingTriviaType => _fixture.GreenTokenWithLeadingTriviaType;
    private Type GreenTokenWithTrailingTriviaType => _fixture.GreenTokenWithTrailingTriviaType;
    private Type GreenTokenWithTriviaType => _fixture.GreenTokenWithTriviaType;
    private Type GreenTokenFactoryType => _fixture.GreenTokenFactoryType;
    private object CreateFactory() => _fixture.CreateFactory();

    #region GreenTrivia Tests

    [Fact]
    public void GreenTrivia_StringCtor_SetsProperties()
    {
        // Create: new GreenTrivia(kind: 1, text: "  ")
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");

        var fullWidth = (int)GreenTriviaType.GetProperty("FullWidth")!.GetValue(trivia)!;
        var rawKind = (ushort)GreenTriviaType.GetProperty("RawKind")!.GetValue(trivia)!;
        var isTrivia = (bool)GreenTriviaType.GetProperty("IsTrivia")!.GetValue(trivia)!;
        var getText = (string)GreenTriviaType.GetMethod("GetText")!.Invoke(trivia, null)!;

        Assert.Equal(2, fullWidth);
        Assert.Equal(1, rawKind);
        Assert.True(isTrivia);
        Assert.Equal("  ", getText);
    }

    [Fact]
    public void GreenTrivia_MemoryCtor_SetsProperties()
    {
        var source = "   whitespace   ".AsMemory();
        var slice = source.Slice(3, 10); // "whitespace"

        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)2, slice);

        var fullWidth = (int)GreenTriviaType.GetProperty("FullWidth")!.GetValue(trivia)!;
        var getText = (string)GreenTriviaType.GetMethod("GetText")!.Invoke(trivia, null)!;

        Assert.Equal(10, fullWidth);
        Assert.Equal("whitespace", getText);
    }

    [Fact]
    public void GreenTrivia_ToOwned_CreatesIndependentCopy()
    {
        var source = "  ".AsMemory();
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, source);

        var owned = GreenTriviaType.GetMethod("ToOwned")!.Invoke(trivia, null);

        Assert.NotSame(trivia, owned);
        var getText = (string)GreenTriviaType.GetMethod("GetText")!.Invoke(owned, null)!;
        Assert.Equal("  ", getText);
    }

    [Fact]
    public void GreenTrivia_StringBacked_ToOwned_ReturnsSame()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");

        var owned = GreenTriviaType.GetMethod("ToOwned")!.Invoke(trivia, null);

        Assert.Same(trivia, owned);
    }

    #endregion

    #region GreenTriviaList Tests

    [Fact]
    public void GreenTriviaList_ComputesWidth()
    {
        var t1 = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var t2 = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");

        var triviaArray = Array.CreateInstance(GreenTriviaType, 2);
        triviaArray.SetValue(t1, 0);
        triviaArray.SetValue(t2, 1);

        var list = Activator.CreateInstance(GreenTriviaListType, (ushort)0, triviaArray);

        var fullWidth = (int)GreenTriviaListType.GetProperty("FullWidth")!.GetValue(list)!;
        var count = (int)GreenTriviaListType.GetProperty("Count")!.GetValue(list)!;

        Assert.Equal(3, fullWidth); // "  " + "\n"
        Assert.Equal(2, count);
    }

    #endregion

    #region GreenTokenWithNoTrivia Tests

    [Fact]
    public void GreenTokenWithNoTrivia_StringCtor_SetsProperties()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "if");

        var fullWidth = (int)GreenTokenType.GetProperty("FullWidth")!.GetValue(token)!;
        var width = (int)GreenTokenType.GetProperty("Width")!.GetValue(token)!;
        var rawKind = (ushort)GreenTokenType.GetProperty("RawKind")!.GetValue(token)!;
        var isToken = (bool)GreenTokenType.GetProperty("IsToken")!.GetValue(token)!;
        var getText = (string)GreenTokenType.GetMethod("GetText")!.Invoke(token, null)!;

        Assert.Equal(2, fullWidth);
        Assert.Equal(2, width);
        Assert.Equal(10, rawKind);
        Assert.True(isToken);
        Assert.Equal("if", getText);
    }

    [Fact]
    public void GreenTokenWithNoTrivia_MemoryCtor_Works()
    {
        var source = "keyword".AsMemory();
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, source);

        var getText = (string)GreenTokenType.GetMethod("GetText")!.Invoke(token, null)!;
        Assert.Equal("keyword", getText);
    }

    [Fact]
    public void GreenTokenWithNoTrivia_ToFullString_ReturnsText()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "hello");

        var toFullString = (string)GreenNodeType.GetMethod("ToFullString")!.Invoke(token, null)!;

        Assert.Equal("hello", toFullString);
    }

    [Fact]
    public void GreenTokenWithNoTrivia_ToOwned_StringBacked_ReturnsSame()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "if");

        var owned = GreenNodeType.GetMethod("ToOwned")!.Invoke(token, null);

        Assert.Same(token, owned);
    }

    [Fact]
    public void GreenTokenWithNoTrivia_ToOwned_MemoryBacked_CreatesCopy()
    {
        var source = "keyword".AsMemory();
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, source);

        var owned = GreenNodeType.GetMethod("ToOwned")!.Invoke(token, null);

        Assert.NotSame(token, owned);
        var getText = (string)GreenTokenType.GetMethod("GetText")!.Invoke(owned, null)!;
        Assert.Equal("keyword", getText);
    }

    #endregion

    #region GreenTokenWithLeadingTrivia Tests

    [Fact]
    public void GreenTokenWithLeadingTrivia_ComputesWidthCorrectly()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var token = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)10, trivia, "if");

        var fullWidth = (int)GreenTokenType.GetProperty("FullWidth")!.GetValue(token)!;
        var width = (int)GreenTokenType.GetProperty("Width")!.GetValue(token)!;
        var leadingTriviaWidth = (int)GreenTokenType.GetProperty("LeadingTriviaWidth")!.GetValue(token)!;

        Assert.Equal(4, fullWidth); // "  " + "if"
        Assert.Equal(2, width); // just "if"
        Assert.Equal(2, leadingTriviaWidth);
    }

    [Fact]
    public void GreenTokenWithLeadingTrivia_ToFullString_IncludesTrivia()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var token = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)10, trivia, "if");

        var toFullString = (string)GreenNodeType.GetMethod("ToFullString")!.Invoke(token, null)!;

        Assert.Equal("  if", toFullString);
    }

    [Fact]
    public void GreenTokenWithLeadingTrivia_ToCoreString_ExcludesTrivia()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var token = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)10, trivia, "if");

        var toCoreString = (string)GreenNodeType.GetMethod("ToCoreString")!.Invoke(token, null)!;

        Assert.Equal("if", toCoreString);
    }

    #endregion

    #region GreenTokenWithTrailingTrivia Tests

    [Fact]
    public void GreenTokenWithTrailingTrivia_ComputesWidthCorrectly()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");
        var token = Activator.CreateInstance(GreenTokenWithTrailingTriviaType, (ushort)10, "if", trivia);

        var fullWidth = (int)GreenTokenType.GetProperty("FullWidth")!.GetValue(token)!;
        var width = (int)GreenTokenType.GetProperty("Width")!.GetValue(token)!;
        var trailingTriviaWidth = (int)GreenTokenType.GetProperty("TrailingTriviaWidth")!.GetValue(token)!;

        Assert.Equal(3, fullWidth); // "if" + "\n"
        Assert.Equal(2, width); // just "if"
        Assert.Equal(1, trailingTriviaWidth);
    }

    [Fact]
    public void GreenTokenWithTrailingTrivia_ToFullString_IncludesTrivia()
    {
        var trivia = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");
        var token = Activator.CreateInstance(GreenTokenWithTrailingTriviaType, (ushort)10, "if", trivia);

        var toFullString = (string)GreenNodeType.GetMethod("ToFullString")!.Invoke(token, null)!;

        Assert.Equal("if\n", toFullString);
    }

    #endregion

    #region GreenTokenWithTrivia Tests

    [Fact]
    public void GreenTokenWithTrivia_ComputesWidthCorrectly()
    {
        var leading = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var trailing = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");
        var token = Activator.CreateInstance(GreenTokenWithTriviaType, (ushort)10, leading, "if", trailing);

        var fullWidth = (int)GreenTokenType.GetProperty("FullWidth")!.GetValue(token)!;
        var width = (int)GreenTokenType.GetProperty("Width")!.GetValue(token)!;
        var leadingTriviaWidth = (int)GreenTokenType.GetProperty("LeadingTriviaWidth")!.GetValue(token)!;
        var trailingTriviaWidth = (int)GreenTokenType.GetProperty("TrailingTriviaWidth")!.GetValue(token)!;

        Assert.Equal(5, fullWidth); // "  " + "if" + "\n"
        Assert.Equal(2, width); // just "if"
        Assert.Equal(2, leadingTriviaWidth);
        Assert.Equal(1, trailingTriviaWidth);
    }

    [Fact]
    public void GreenTokenWithTrivia_ToFullString_IncludesAllTrivia()
    {
        var leading = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var trailing = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");
        var token = Activator.CreateInstance(GreenTokenWithTriviaType, (ushort)10, leading, "if", trailing);

        var toFullString = (string)GreenNodeType.GetMethod("ToFullString")!.Invoke(token, null)!;

        Assert.Equal("  if\n", toFullString);
    }

    [Fact]
    public void GreenTokenWithTrivia_ToCoreString_ExcludesAllTrivia()
    {
        var leading = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var trailing = Activator.CreateInstance(GreenTriviaType, (ushort)1, "\n");
        var token = Activator.CreateInstance(GreenTokenWithTriviaType, (ushort)10, leading, "if", trailing);

        var toCoreString = (string)GreenNodeType.GetMethod("ToCoreString")!.Invoke(token, null)!;

        Assert.Equal("if", toCoreString);
    }

    #endregion

    #region GreenTokenFactory Tests

    [Fact]
    public void GreenTokenFactory_CreateToken_ReturnsToken()
    {
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;

        var token = createToken.Invoke(factory, new object[] { (ushort)10, "if" });

        Assert.NotNull(token);
        var getText = (string)GreenTokenType.GetMethod("GetText")!.Invoke(token, null)!;
        Assert.Equal("if", getText);
    }

    [Fact]
    public void GreenTokenFactory_CachesShortStrings()
    {
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;

        var token1 = createToken.Invoke(factory, new object[] { (ushort)10, "if" });
        var token2 = createToken.Invoke(factory, new object[] { (ushort)10, "if" });

        Assert.Same(token1, token2);
    }

    [Fact]
    public void GreenTokenFactory_DoesNotCacheLongStrings()
    {
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;
        var longText = "this_is_a_very_long_identifier_name";

        var token1 = createToken.Invoke(factory, new object[] { (ushort)10, longText });
        var token2 = createToken.Invoke(factory, new object[] { (ushort)10, longText });

        Assert.NotSame(token1, token2);
    }

    [Fact]
    public void GreenTokenFactory_CacheCount_TracksItems()
    {
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;
        var cacheCount = GreenTokenFactoryType.GetProperty("CacheCount")!;

        Assert.Equal(0, cacheCount.GetValue(factory));

        createToken.Invoke(factory, new object[] { (ushort)10, "if" });
        createToken.Invoke(factory, new object[] { (ushort)11, "else" });
        createToken.Invoke(factory, new object[] { (ushort)12, "while" });

        Assert.Equal(3, cacheCount.GetValue(factory));
    }

    [Fact]
    public void GreenTokenFactory_ClearCache_ResetsCacheCount()
    {
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;
        var clearCache = GreenTokenFactoryType.GetMethod("ClearCache")!;
        var cacheCount = GreenTokenFactoryType.GetProperty("CacheCount")!;

        createToken.Invoke(factory, new object[] { (ushort)10, "if" });
        createToken.Invoke(factory, new object[] { (ushort)11, "else" });
        Assert.Equal(2, cacheCount.GetValue(factory));

        clearCache.Invoke(factory, null);
        Assert.Equal(0, cacheCount.GetValue(factory));
    }

    [Fact]
    public void GreenTokenFactory_CreateTrivia_ReturnsTrivia()
    {
        var factory = CreateFactory();
        var createTrivia = GreenTokenFactoryType.GetMethod("CreateTrivia", new[] { typeof(ushort), typeof(string) })!;

        var trivia = createTrivia.Invoke(factory, new object[] { (ushort)1, "  " });

        Assert.NotNull(trivia);
        var getText = (string)GreenTriviaType.GetMethod("GetText")!.Invoke(trivia, null)!;
        Assert.Equal("  ", getText);
    }

    #endregion

    #region IFormattable Tests

    [Fact]
    public void GreenToken_IFormattable_T_ReturnsFullString()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "hello");
        var formattable = (IFormattable)token!;

        var result = formattable.ToString("T", null);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void GreenToken_IFormattable_C_ReturnsCoreString()
    {
        var leading = Activator.CreateInstance(GreenTriviaType, (ushort)1, "  ");
        var token = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)10, leading, "if");
        var formattable = (IFormattable)token!;

        var resultT = formattable.ToString("T", null);
        var resultC = formattable.ToString("C", null);

        Assert.Equal("  if", resultT);
        Assert.Equal("if", resultC);
    }

    [Fact]
    public void GreenToken_IFormattable_M_ReturnsMermaid()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        var result = formattable.ToString("M", null);

        Assert.Contains("```mermaid", result);
        Assert.Contains("flowchart TD", result);
        Assert.Contains("Kind:10", result);
    }

    [Fact]
    public void GreenToken_IFormattable_D_ReturnsDebugTree()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        var result = formattable.ToString("D", null);

        Assert.Contains("Token", result);
        Assert.Contains("Kind=10", result);
    }

    [Fact]
    public void GreenToken_IFormattable_J_ReturnsJson()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        var result = formattable.ToString("J", null);

        Assert.Contains("\"type\":", result);
        Assert.Contains("\"kind\": 10", result);
        Assert.Contains("\"text\": \"test\"", result);
    }

    [Fact]
    public void GreenToken_IFormattable_G_ReturnsDebugString()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        var result = formattable.ToString("G", null);

        Assert.Contains("Token", result);
        Assert.Contains("Kind=10", result);
        Assert.DoesNotContain("\n", result);
    }

    [Fact]
    public void GreenToken_IFormattable_CaseInsensitive()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        Assert.Equal(formattable.ToString("T", null), formattable.ToString("t", null));
        Assert.Equal(formattable.ToString("C", null), formattable.ToString("c", null));
        Assert.Equal(formattable.ToString("M", null), formattable.ToString("m", null));
    }

    [Fact]
    public void GreenToken_IFormattable_InvalidFormat_ThrowsFormatException()
    {
        var token = Activator.CreateInstance(GreenTokenWithNoTriviaType, (ushort)10, "test");
        var formattable = (IFormattable)token!;

        Assert.Throws<FormatException>(() => formattable.ToString("X", null));
    }

    #endregion

    #region Source Reconstruction Tests

    [Fact]
    public void Tokens_ToFullString_ReconstructsSource()
    {
        // Simulate tokenizing "  if (x) \n"
        var factory = CreateFactory();
        var createToken = GreenTokenFactoryType.GetMethod("CreateToken", new[] { typeof(ushort), typeof(string) })!;
        var createTrivia = GreenTokenFactoryType.GetMethod("CreateTrivia", new[] { typeof(ushort), typeof(string) })!;

        // Create tokens with trivia
        var ifLeading = createTrivia.Invoke(factory, new object[] { (ushort)1, "  " });
        var ifToken = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)10, ifLeading, "if");

        var parenLeading = createTrivia.Invoke(factory, new object[] { (ushort)1, " " });
        var openParen = Activator.CreateInstance(GreenTokenWithLeadingTriviaType, (ushort)20, parenLeading, "(");

        var x = createToken.Invoke(factory, new object[] { (ushort)30, "x" });

        var closeParenTrailing = createTrivia.Invoke(factory, new object[] { (ushort)1, " \n" });
        var closeParen = Activator.CreateInstance(GreenTokenWithTrailingTriviaType, (ushort)20, ")", closeParenTrailing);

        // Reconstruct source
        var toFullString = GreenNodeType.GetMethod("ToFullString")!;
        var reconstructed = 
            (string)toFullString.Invoke(ifToken, null)! +
            (string)toFullString.Invoke(openParen, null)! +
            (string)toFullString.Invoke(x, null)! +
            (string)toFullString.Invoke(closeParen, null)!;

        Assert.Equal("  if (x) \n", reconstructed);
    }

    #endregion
}
