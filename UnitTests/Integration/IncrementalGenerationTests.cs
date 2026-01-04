using System.Collections.Immutable;
using System.Reflection;
using MetaParser;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Integration;

/// <summary>
/// Tests for the incremental source generator behavior.
/// Verifies that MetaParser generates consistent output and handles schema changes correctly.
/// </summary>
public class IncrementalGenerationTests
{
    #region Generator Consistency Tests

    [Fact]
    public void Generate_SameSchema_ProducesIdenticalOutput()
    {
        var schema = CreateSimpleSchema();

        // Generate twice
        var output1 = GenerateAllSources(schema);
        var output2 = GenerateAllSources(schema);

        // Outputs should be identical
        Assert.Equal(output1.Length, output2.Length);
        for (int i = 0; i < output1.Length; i++)
        {
            Assert.Equal(output1[i], output2[i]);
        }
    }

    [Fact]
    public void Generate_MultipleSchemas_ProducesUniqueOutputs()
    {
        var schema1 = new SchemaDefinition
        {
            Namespace = "Parser1.Syntax",
            Classname = "Parser1"
        };
        schema1.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };

        var schema2 = new SchemaDefinition
        {
            Namespace = "Parser2.Syntax",
            Classname = "Parser2"
        };
        schema2.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };

        var output1 = GreenNodeGenerator.Generate(schema1).Build();
        var output2 = GreenNodeGenerator.Generate(schema2).Build();

        // Outputs should differ in namespace
        Assert.Contains("Parser1.Syntax", output1);
        Assert.Contains("Parser2.Syntax", output2);
        Assert.DoesNotContain("Parser2.Syntax", output1);
        Assert.DoesNotContain("Parser1.Syntax", output2);
    }

    #endregion

    #region Schema Modification Tests

    [Fact]
    public void Generate_AddToken_UpdatesTokenKindEnum()
    {
        var schema = CreateSimpleSchema();
        var output1 = TokenKindGenerator.Generate(schema).Build();

        // Add a new token
        schema.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };
        var output2 = TokenKindGenerator.Generate(schema).Build();

        // First output should have Plus but not Minus
        Assert.Contains("Plus", output1);
        Assert.DoesNotContain("Minus = ", output1);

        // Second output should have both
        Assert.Contains("Plus", output2);
        Assert.Contains("Minus", output2);
    }

    [Fact]
    public void Generate_RemoveToken_UpdatesTokenKindEnum()
    {
        var schema = CreateSimpleSchema();
        schema.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };

        var output1 = TokenKindGenerator.Generate(schema).Build();
        Assert.Contains("Minus", output1);

        // Remove the minus token
        schema.Tokens.Remove("minus");
        var output2 = TokenKindGenerator.Generate(schema).Build();

        // Second output should not have Minus
        Assert.DoesNotContain("Minus = ", output2);
    }

    [Fact]
    public void Generate_ChangeNamespace_UpdatesAllFiles()
    {
        var schema = CreateSimpleSchema();

        var output1 = GreenNodeGenerator.Generate(schema).Build();
        Assert.Contains("TestParser.Syntax", output1);

        // Change namespace
        schema.Namespace = "NewParser.Syntax";
        var output2 = GreenNodeGenerator.Generate(schema).Build();

        Assert.Contains("NewParser.Syntax", output2);
        Assert.DoesNotContain("TestParser.Syntax", output2);
    }

    [Fact]
    public void Generate_ChangeClassname_UpdatesParser()
    {
        var schema = CreateSimpleSchema();

        var output1 = ParserGenerator.Generate(schema).Build();
        Assert.Contains("TestParser", output1);

        // Change classname
        schema.Classname = "NewParser";
        var output2 = ParserGenerator.Generate(schema).Build();

        Assert.Contains("NewParserParser", output2);
    }

    [Fact]
    public void Generate_AddTrivia_UpdatesTriviaList()
    {
        var schema = CreateSimpleSchema();

        var output1 = TokenKindGenerator.Generate(schema).Build();
        // By default, whitespace and end-of-line are trivia
        Assert.Contains("Whitespace", output1);

        // Add custom trivia
        schema.Trivia.Add("whitespace");
        schema.Trivia.Add("end-of-line");

        var output2 = TokenKindGenerator.Generate(schema).Build();

        // Should still compile correctly
        Assert.Contains("Whitespace", output2);
        Assert.Contains("EndOfLine", output2);
    }

    #endregion

    #region Generator Output Compilation Tests

    [Fact]
    public void Generate_MinimalSchema_Compiles()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Minimal.Syntax",
            Classname = "Minimal"
        };
        schema.Tokens["a"] = new TokenDefinition { Start = new LiteralPattern("a") };

        var sources = GenerateAllSources(schema);
        var assembly = CompileAndLoad(sources, "MinimalTestAssembly");

        Assert.NotNull(assembly);
        Assert.NotNull(assembly.GetType("Minimal.Syntax.MinimalSyntaxTree"));
        Assert.NotNull(assembly.GetType("Minimal.Syntax.MinimalParser"));
    }

    [Fact]
    public void Generate_LargeSchema_Compiles()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Large.Syntax",
            Classname = "Large"
        };

        // Add many tokens
        for (int i = 0; i < 50; i++)
        {
            schema.Tokens[$"token{i}"] = new TokenDefinition
            {
                Start = new LiteralPattern($"t{i}")
            };
        }

        var sources = GenerateAllSources(schema);
        var assembly = CompileAndLoad(sources, "LargeTestAssembly");

        Assert.NotNull(assembly);

        // Verify all token kinds were generated
        var tokenKindType = assembly.GetType("Large.Syntax.TokenKind")!;
        var enumValues = Enum.GetNames(tokenKindType);

        // Should have built-in tokens + our 50 custom tokens
        Assert.True(enumValues.Length >= 50);
    }

    [Fact]
    public void Generate_ComplexPatterns_Compiles()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Complex.Syntax",
            Classname = "Complex"
        };

        // Token with range pattern
        schema.Tokens["letter"] = new TokenDefinition
        {
            Start = new RangePattern('a', 'z'),
            Consume = new RangePattern('a', 'z')
        };

        // Token with OneOf pattern
        schema.Tokens["alphanumeric"] = new TokenDefinition
        {
            Start = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern('a', 'z'),
                new RangePattern('A', 'Z'),
                new RangePattern('0', '9')
            })
        };

        // Token with escape
        schema.Tokens["string"] = new TokenDefinition
        {
            Start = new LiteralPattern("\""),
            Consume = new RangePattern(' ', '~'),
            Stop = new LiteralPattern("\""),
            Escape = new LiteralPattern("\\")
        };

        var sources = GenerateAllSources(schema);
        var assembly = CompileAndLoad(sources, "ComplexTestAssembly");

        Assert.NotNull(assembly);
    }

    #endregion

    #region Determinism Tests

    [Fact]
    public void Generate_IsFullyDeterministic()
    {
        var schema = CreateSimpleSchema();

        // Generate multiple times and verify identical output
        var outputs = new List<string[]>();
        for (int i = 0; i < 5; i++)
        {
            outputs.Add(GenerateAllSources(schema));
        }

        // All outputs should be identical
        for (int i = 1; i < outputs.Count; i++)
        {
            Assert.Equal(outputs[0].Length, outputs[i].Length);
            for (int j = 0; j < outputs[0].Length; j++)
            {
                Assert.Equal(outputs[0][j], outputs[i][j]);
            }
        }
    }

    [Fact]
    public void Generate_TokenOrderAffectsEnum()
    {
        // Schema with tokens in order A, B
        var schema1 = new SchemaDefinition
        {
            Namespace = "Order.Syntax",
            Classname = "Order"
        };
        schema1.Tokens["a"] = new TokenDefinition { Start = new LiteralPattern("a") };
        schema1.Tokens["b"] = new TokenDefinition { Start = new LiteralPattern("b") };

        // Schema with tokens in order B, A (using ordered dictionary)
        var schema2 = new SchemaDefinition
        {
            Namespace = "Order.Syntax",
            Classname = "Order"
        };
        schema2.Tokens["b"] = new TokenDefinition { Start = new LiteralPattern("b") };
        schema2.Tokens["a"] = new TokenDefinition { Start = new LiteralPattern("a") };

        var output1 = TokenKindGenerator.Generate(schema1).Build();
        var output2 = TokenKindGenerator.Generate(schema2).Build();

        // Order should be reflected in the output
        var aIndex1 = output1.IndexOf("A = ");
        var bIndex1 = output1.IndexOf("B = ");
        var aIndex2 = output2.IndexOf("A = ");
        var bIndex2 = output2.IndexOf("B = ");

        // In schema1: A comes before B
        Assert.True(aIndex1 < bIndex1);
        // In schema2: B comes before A
        Assert.True(bIndex2 < aIndex2);
    }

    #endregion

    #region Error Recovery Tests

    [Fact]
    public void Generate_WithValidationWarnings_StillCompiles()
    {
        var schema = CreateSimpleSchema();

        // Add an unreferenced token (which might generate a warning)
        schema.Tokens["unused"] = new TokenDefinition
        {
            Start = new LiteralPattern("unused")
        };

        var sources = GenerateAllSources(schema);
        var assembly = CompileAndLoad(sources, "WarningTestAssembly");

        Assert.NotNull(assembly);
    }

    #endregion

    #region Helper Methods

    private static SchemaDefinition CreateSimpleSchema()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "TestParser.Syntax",
            Classname = "Test"
        };
        schema.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };
        schema.Tokens["number"] = new TokenDefinition
        {
            Start = new RangePattern('0', '9'),
            Consume = new RangePattern('0', '9')
        };
        return schema;
    }

    private static string[] GenerateAllSources(SchemaDefinition schema)
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

    private static Assembly CompileAndLoad(string[] sources, string assemblyName)
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

    #endregion
}
