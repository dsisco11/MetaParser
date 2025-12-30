using MetaParser.Syntax.Green;
using Xunit;

namespace UnitTests.Syntax.Green;

/// <summary>
/// Tests for GreenNode IFormattable implementation.
/// </summary>
public class GreenNodeFormattableTests
{
    private const ushort TokenKind = 42;
    private const ushort TriviaKind = 1;

    #region IFormattable - Text Format (T)

    [Fact]
    public void Format_T_ReturnsFullString()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "hello");
        
        Assert.Equal("hello", $"{token:T}");
    }

    [Fact]
    public void Format_Default_ReturnsFullString()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "hello");
        
        // Default format (null/empty) should return full text
        Assert.Equal("hello", token.ToString(null, null));
        Assert.Equal("hello", token.ToString("", null));
    }

    [Fact]
    public void Format_T_TokenWithTrivia_IncludesTrivia()
    {
        var leading = new GreenTrivia(TriviaKind, "  ");
        var token = new GreenTokenWithLeadingTrivia(TokenKind, leading, "hello");
        
        Assert.Equal("  hello", $"{token:T}");
    }

    [Fact]
    public void Format_T_ReconstructsSource()
    {
        var factory = new GreenTokenFactory();
        
        var ifLeading = factory.CreateTrivia(TriviaKind, "  ");
        var ifToken = factory.CreateTokenWithLeadingTrivia(10, ifLeading, "if");
        
        var parenLeading = factory.CreateTrivia(TriviaKind, " ");
        var openParen = factory.CreateTokenWithLeadingTrivia(20, parenLeading, "(");
        
        var x = factory.CreateToken(30, "x");
        
        var closeParenTrailing = factory.CreateTrivia(TriviaKind, " ");
        var closeParen = factory.CreateTokenWithTrailingTrivia(20, ")", closeParenTrailing);
        
        var reconstructed = $"{ifToken:T}{openParen:T}{x:T}{closeParen:T}";
        
        Assert.Equal("  if (x) ", reconstructed);
    }

    #endregion

    #region IFormattable - Core Format (C)

    [Fact]
    public void Format_C_ExcludesTrivia()
    {
        var leading = new GreenTrivia(TriviaKind, "  ");
        var trailing = new GreenTrivia(TriviaKind, "\n");
        var token = new GreenTokenWithTrivia(TokenKind, leading, "keyword", trailing);
        
        Assert.Equal("keyword", $"{token:C}");
    }

    [Fact]
    public void Format_C_NoTrivia_SameAsT()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        Assert.Equal($"{token:T}", $"{token:C}");
    }

    #endregion

    #region IFormattable - Mermaid Format (M)

    [Fact]
    public void Format_M_ProducesValidDiagram()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        var mermaid = $"{token:M}";
        
        Assert.Contains("```mermaid", mermaid);
        Assert.Contains("flowchart TD", mermaid);
        Assert.Contains("```", mermaid);
        Assert.Contains("Kind:42", mermaid);
        Assert.Contains("test", mermaid);
    }

    [Fact]
    public void Format_M_TokenWithTrivia_ShowsTriviaConnections()
    {
        var leading = new GreenTrivia(TriviaKind, " ");
        var trailing = new GreenTrivia(TriviaKind, "\n");
        var token = new GreenTokenWithTrivia(TokenKind, leading, "x", trailing);
        
        var mermaid = $"{token:M}";
        
        Assert.Contains("leading", mermaid);
        Assert.Contains("trailing", mermaid);
    }

    #endregion

    #region IFormattable - Debug Tree Format (D)

    [Fact]
    public void Format_D_ProducesTree()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "hello");
        
        var tree = $"{token:D}";
        
        Assert.Contains("Token[Kind=42", tree);
        Assert.Contains("hello", tree);
    }

    [Fact]
    public void Format_D_TokenWithTrivia_ShowsTrivia()
    {
        var leading = new GreenTrivia(TriviaKind, "  ");
        var token = new GreenTokenWithLeadingTrivia(TokenKind, leading, "x");
        
        var tree = $"{token:D}";
        
        Assert.Contains("Token", tree);
        Assert.Contains("Trivia", tree);
    }

    #endregion

    #region IFormattable - JSON Format (J)

    [Fact]
    public void Format_J_ProducesValidJson()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        var json = $"{token:J}";
        
        Assert.Contains("\"type\":", json);
        Assert.Contains("\"kind\": 42", json);
        Assert.Contains("\"text\": \"test\"", json);
        Assert.Contains("\"fullWidth\": 4", json);
    }

    [Fact]
    public void Format_J_TokenWithTrivia_IncludesTrivia()
    {
        var leading = new GreenTrivia(TriviaKind, " ");
        var token = new GreenTokenWithLeadingTrivia(TokenKind, leading, "x");
        
        var json = $"{token:J}";
        
        Assert.Contains("leadingTrivia", json);
    }

    [Fact]
    public void Format_J_EscapesSpecialCharacters()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "line1\nline2");
        
        var json = $"{token:J}";
        
        Assert.Contains("\\n", json);
    }

    #endregion

    #region IFormattable - Debug String Format (G)

    [Fact]
    public void Format_G_ReturnsSingleLineDebug()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        var debug = $"{token:G}";
        
        Assert.Contains("Token", debug);
        Assert.Contains("Kind=42", debug);
        Assert.DoesNotContain("\n", debug);
    }

    #endregion

    #region IFormattable - Case Insensitivity

    [Fact]
    public void Format_CaseInsensitive()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        Assert.Equal($"{token:T}", $"{token:t}");
        Assert.Equal($"{token:M}", $"{token:m}");
        Assert.Equal($"{token:D}", $"{token:d}");
        Assert.Equal($"{token:J}", $"{token:j}");
        Assert.Equal($"{token:G}", $"{token:g}");
        Assert.Equal($"{token:C}", $"{token:c}");
    }

    #endregion

    #region IFormattable - Invalid Format

    [Fact]
    public void Format_InvalidSpecifier_ThrowsFormatException()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "test");
        
        Assert.Throws<FormatException>(() => token.ToString("X", null));
        Assert.Throws<FormatException>(() => token.ToString("invalid", null));
    }

    #endregion

    #region ToOwned Tests

    [Fact]
    public void ToOwned_StringBacked_ReturnsSameInstance()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "keyword");
        
        var owned = token.ToOwned();
        
        Assert.Same(token, owned);
    }

    [Fact]
    public void ToOwned_MemoryBacked_CreatesNewInstance()
    {
        var source = "original source text".AsMemory();
        var slice = source.Slice(9, 6); // "source"
        var token = new GreenTokenWithNoTrivia(TokenKind, slice);
        
        var owned = token.ToOwned();
        
        Assert.NotSame(token, owned);
        Assert.Equal("source", ((GreenToken)owned).GetText());
    }

    [Fact]
    public void ToOwned_WithTrivia_OwnsAllParts()
    {
        var sourceText = "  keyword\n";
        var source = sourceText.AsMemory();
        
        var leadingTrivia = new GreenTrivia(TriviaKind, source.Slice(0, 2));
        var trailingTrivia = new GreenTrivia(TriviaKind, source.Slice(9, 1));
        var token = new GreenTokenWithTrivia(TokenKind, leadingTrivia, source.Slice(2, 7), trailingTrivia);
        
        var owned = token.ToOwned();
        
        Assert.NotSame(token, owned);
        Assert.Equal(sourceText, owned.ToFullString());
    }

    [Fact]
    public void ToOwned_Trivia_CreatesIndependentCopy()
    {
        var source = "   ".AsMemory();
        var trivia = new GreenTrivia(TriviaKind, source);
        
        var owned = trivia.ToOwned();
        
        Assert.NotSame(trivia, owned);
        Assert.Equal("   ", ((GreenTrivia)owned).GetText());
    }

    #endregion

    #region String Interpolation Tests

    [Fact]
    public void StringInterpolation_WorksWithFormattable()
    {
        var token = new GreenTokenWithNoTrivia(TokenKind, "hello");
        
        // Verify IFormattable works in string interpolation
        string result = $"Token text: {token:T}";
        
        Assert.Equal("Token text: hello", result);
    }

    [Fact]
    public void StringInterpolation_MultipleFormats()
    {
        var leading = new GreenTrivia(TriviaKind, "  ");
        var token = new GreenTokenWithLeadingTrivia(TokenKind, leading, "x");
        
        // Full text vs core text
        Assert.Equal("  x", $"{token:T}");
        Assert.Equal("x", $"{token:C}");
    }

    #endregion
}
