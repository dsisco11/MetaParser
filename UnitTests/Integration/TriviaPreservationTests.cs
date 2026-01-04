using System.Reflection;
using MetaParser.Generation;
using MetaParser.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace UnitTests.Integration;

/// <summary>
/// Fixture for testing trivia (whitespace, comments) preservation.
/// </summary>
public class TriviaPreservationSchemaFixture : IDisposable
{
    public Assembly GeneratedAssembly { get; }
    public SchemaDefinition Schema { get; }
    public Type SyntaxTreeType { get; }
    public Type ParserType { get; }
    public Type TokenKindType { get; }
    public Type GreenTokenType { get; }
    public Type GreenTriviaType { get; }

    public TriviaPreservationSchemaFixture()
    {
        // Create a schema with multiple trivia types
        Schema = new SchemaDefinition
        {
            Namespace = "TriviaTest.Syntax",
            Classname = "Trivia"
        };

        // Define trivia tokens
        Schema.Trivia.Add("whitespace");
        Schema.Trivia.Add("end-of-line");
        Schema.Trivia.Add("single-line-comment");
        Schema.Trivia.Add("multi-line-comment");

        // Single-line comment: // comment
        Schema.Tokens["single-line-comment"] = new TokenDefinition
        {
            Start = new LiteralPattern("//"),
            Consume = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern(' ', '~'),  // Any printable ASCII
            }),
            Stop = new OneOfPattern(new PatternDefinition[]
            {
                new LiteralPattern("\n"),
                new LiteralPattern("\r"),
            })
        };

        // Multi-line comment: /* comment */
        Schema.Tokens["multi-line-comment"] = new TokenDefinition
        {
            Start = new LiteralPattern("/*"),
            Consume = new OneOfPattern(new PatternDefinition[]
            {
                new RangePattern(' ', '~'),     // printable ASCII
                new LiteralPattern("\n"),
                new LiteralPattern("\r"),
                new LiteralPattern("\t"),
            }),
            Stop = new LiteralPattern("*/")
        };

        // Numbers
        Schema.Tokens["number"] = new TokenDefinition
        {
            Start = new RangePattern('0', '9'),
            Consume = new RangePattern('0', '9')
        };

        // Operators
        Schema.Tokens["plus"] = new TokenDefinition { Start = new LiteralPattern("+") };
        Schema.Tokens["minus"] = new TokenDefinition { Start = new LiteralPattern("-") };
        Schema.Tokens["star"] = new TokenDefinition { Start = new LiteralPattern("*") };
        Schema.Tokens["slash"] = new TokenDefinition { Start = new LiteralPattern("/") };

        // Identifiers
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

        // Semicolon
        Schema.Tokens["semicolon"] = new TokenDefinition { Start = new LiteralPattern(";") };

        // Generate all code
        var sources = CalculatorSchemaFixture.GenerateAllSources(Schema);
        GeneratedAssembly = CalculatorSchemaFixture.CompileAndLoad(sources, "TriviaPreservationTestAssembly");

        // Get types
        SyntaxTreeType = GeneratedAssembly.GetType("TriviaTest.Syntax.TriviaSyntaxTree")!;
        ParserType = GeneratedAssembly.GetType("TriviaTest.Syntax.TriviaParser")!;
        TokenKindType = GeneratedAssembly.GetType("TriviaTest.Syntax.TokenKind")!;
        GreenTokenType = GeneratedAssembly.GetType("TriviaTest.Syntax.GreenToken")!;
        GreenTriviaType = GeneratedAssembly.GetType("TriviaTest.Syntax.GreenTrivia")!;
    }

    public void Dispose() { }
}

/// <summary>
/// Tests for trivia (whitespace, comments) preservation during parsing.
/// </summary>
public class TriviaPreservationTests : IClassFixture<TriviaPreservationSchemaFixture>
{
    private readonly TriviaPreservationSchemaFixture _fixture;

    public TriviaPreservationTests(TriviaPreservationSchemaFixture fixture)
    {
        _fixture = fixture;
    }

    #region Whitespace Preservation Tests

    [Theory]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("    ")]
    [InlineData("\t")]
    [InlineData("\t\t")]
    [InlineData(" \t ")]
    public void Parse_WhitespaceOnly_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("x")]
    [InlineData(" x")]
    [InlineData("x ")]
    [InlineData(" x ")]
    [InlineData("  x  ")]
    [InlineData("\tx\t")]
    public void Parse_TokenWithWhitespace_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("1+2")]
    [InlineData("1 + 2")]
    [InlineData("1  +  2")]
    [InlineData("1   +   2")]
    [InlineData("1\t+\t2")]
    public void Parse_ExpressionWithVariableWhitespace_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Newline Preservation Tests

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    [InlineData("\n\n")]
    [InlineData("\r\n\r\n")]
    public void Parse_NewlinesOnly_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Theory]
    [InlineData("x\ny")]
    [InlineData("x\r\ny")]
    [InlineData("x\n\ny")]
    [InlineData("1\n+\n2")]
    public void Parse_TokensWithNewlines_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_MultilineExpression_PreservesNewlines()
    {
        var input = "x\n  + y\n  * z";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Mixed Trivia Tests

    [Theory]
    [InlineData(" \n")]
    [InlineData("\n ")]
    [InlineData(" \n ")]
    [InlineData("\t\n\t")]
    [InlineData("  \n  \n  ")]
    public void Parse_MixedWhitespaceAndNewlines_PreservesExactly(string input)
    {
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_ComplexIndentation_PreservesExactly()
    {
        var input = "x\n    + y\n        * z";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Leading/Trailing Trivia Tests

    [Fact]
    public void Parse_LeadingWhitespace_IsPreserved()
    {
        var input = "   x + y";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_TrailingWhitespace_IsPreserved()
    {
        var input = "x + y   ";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_LeadingAndTrailingWhitespace_BothPreserved()
    {
        var input = "   x + y   ";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_LeadingNewlines_ArePreserved()
    {
        var input = "\n\nx + y";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_TrailingNewlines_ArePreserved()
    {
        var input = "x + y\n\n";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Trivia Attachment Tests

    [Fact]
    public void Parse_WhitespaceBeforeToken_AttachedAsLeading()
    {
        var input = "  x";
        var tree = ParseInput(input);

        // Verify round-trip preservation
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_WhitespaceAfterToken_AttachedAsTrailing()
    {
        var input = "x  ";
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_NewlineSplitsTrivia_CorrectAttachment()
    {
        // Roslyn-style: trivia after newline goes to next token
        var input = "x \n y";
        var tree = ParseInput(input);

        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Complex Trivia Scenarios

    [Fact]
    public void Parse_RealWorldCodeFormat_PreservesExactly()
    {
        var input = @"x
    + y
    - z";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_MultipleLinesWithIndentation_PreservesExactly()
    {
        var input = "a\n  + b\n    + c\n      + d";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_TabsAndSpacesMixed_PreservesExactly()
    {
        var input = "\t x \t + \t y \t";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region Stress Tests

    [Fact]
    public void Parse_ManyTokensWithTrivia_PreservesAll()
    {
        var input = "a + b + c + d + e + f + g";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_LongWhitespaceSequence_PreservesExactly()
    {
        var input = "x" + new string(' ', 100) + "y";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    [Fact]
    public void Parse_ManyNewlines_PreservesExactly()
    {
        var input = "x" + new string('\n', 50) + "y";
        var tree = ParseInput(input);

        Assert.False(HasErrors(tree));
        Assert.Equal(input, GetReconstructedText(tree));
    }

    #endregion

    #region FullWidth Tests

    [Fact]
    public void Parse_TokenWidthIncludesTrivia()
    {
        var input = "  x  ";
        var tree = ParseInput(input);

        var greenRootProp = _fixture.SyntaxTreeType.GetProperty("GreenRoot")!;
        var greenRoot = greenRootProp.GetValue(tree)!;
        var fullWidthProp = greenRoot.GetType().GetProperty("FullWidth")!;
        var fullWidth = (int)fullWidthProp.GetValue(greenRoot)!;

        Assert.Equal(input.Length, fullWidth);
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
