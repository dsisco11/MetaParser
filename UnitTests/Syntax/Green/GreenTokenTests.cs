using MetaParser.Syntax.Green;
using Xunit;

namespace UnitTests.Syntax.Green;

/// <summary>
/// Tests for GreenToken text retrieval and trivia handling.
/// </summary>
public class GreenTokenTests
{
    private const ushort TestKind = 42;

    #region Text Retrieval (String-Backed)

    [Fact]
    public void StringBacked_GetText_ReturnsOriginalString()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "keyword");
        
        Assert.Equal("keyword", token.GetText());
    }

    [Fact]
    public void StringBacked_ToFullString_ReturnsText()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "hello");
        
        Assert.Equal("hello", token.ToFullString());
    }

    [Fact]
    public void StringBacked_Text_ReturnsSpan()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "test");
        
        Assert.Equal("test", token.Text.ToString());
    }

    #endregion

    #region Text Retrieval (Memory-Backed)

    [Fact]
    public void MemoryBacked_GetText_ReturnsText()
    {
        var source = "hello world".AsMemory();
        var slice = source.Slice(6, 5); // "world"
        var token = new GreenTokenWithNoTrivia(TestKind, slice);
        
        Assert.Equal("world", token.GetText());
    }

    [Fact]
    public void MemoryBacked_Text_ReturnsSpan()
    {
        var source = "some identifier here".AsMemory();
        var slice = source.Slice(5, 10); // "identifier"
        var token = new GreenTokenWithNoTrivia(TestKind, slice);
        
        Assert.Equal("identifier", token.Text.ToString());
    }

    #endregion

    #region Leading Trivia

    [Fact]
    public void WithLeadingTrivia_GetText_ExcludesTrivia()
    {
        var trivia = new GreenTrivia(1, "  ");
        var token = new GreenTokenWithLeadingTrivia(TestKind, trivia, "value");
        
        Assert.Equal("value", token.GetText());
    }

    [Fact]
    public void WithLeadingTrivia_ToFullString_IncludesTrivia()
    {
        var trivia = new GreenTrivia(1, "  ");
        var token = new GreenTokenWithLeadingTrivia(TestKind, trivia, "value");
        
        Assert.Equal("  value", token.ToFullString());
    }

    [Fact]
    public void WithLeadingTrivia_LeadingTrivia_ReturnsTrivia()
    {
        var trivia = new GreenTrivia(1, "\t");
        var token = new GreenTokenWithLeadingTrivia(TestKind, trivia, "x");
        
        Assert.NotNull(token.LeadingTrivia);
        Assert.Same(trivia, token.LeadingTrivia);
        Assert.Null(token.TrailingTrivia);
    }

    #endregion

    #region Trailing Trivia

    [Fact]
    public void WithTrailingTrivia_GetText_ExcludesTrivia()
    {
        var trivia = new GreenTrivia(1, "\n");
        var token = new GreenTokenWithTrailingTrivia(TestKind, "value", trivia);
        
        Assert.Equal("value", token.GetText());
    }

    [Fact]
    public void WithTrailingTrivia_ToFullString_IncludesTrivia()
    {
        var trivia = new GreenTrivia(1, "\r\n");
        var token = new GreenTokenWithTrailingTrivia(TestKind, "value", trivia);
        
        Assert.Equal("value\r\n", token.ToFullString());
    }

    [Fact]
    public void WithTrailingTrivia_TrailingTrivia_ReturnsTrivia()
    {
        var trivia = new GreenTrivia(1, " ");
        var token = new GreenTokenWithTrailingTrivia(TestKind, "x", trivia);
        
        Assert.Null(token.LeadingTrivia);
        Assert.NotNull(token.TrailingTrivia);
        Assert.Same(trivia, token.TrailingTrivia);
    }

    #endregion

    #region Both Trivia

    [Fact]
    public void WithBothTrivia_ToFullString_IncludesBoth()
    {
        var leading = new GreenTrivia(1, "  ");
        var trailing = new GreenTrivia(1, "\n");
        var token = new GreenTokenWithTrivia(TestKind, leading, "keyword", trailing);
        
        Assert.Equal("  keyword\n", token.ToFullString());
    }

    [Fact]
    public void WithBothTrivia_Widths_AreCorrect()
    {
        var leading = new GreenTrivia(1, "   ");  // 3
        var trailing = new GreenTrivia(1, "\r\n"); // 2
        var token = new GreenTokenWithTrivia(TestKind, leading, "test", trailing);
        
        Assert.Equal(4, token.Width);
        Assert.Equal(9, token.FullWidth); // 3 + 4 + 2
        Assert.Equal(3, token.LeadingTriviaWidth);
        Assert.Equal(2, token.TrailingTriviaWidth);
    }

    #endregion

    #region Extension Methods

    [Fact]
    public void WithLeadingTrivia_Extension_CreatesCorrectType()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "x");
        var trivia = new GreenTrivia(1, " ");
        
        var result = token.WithLeadingTrivia(trivia);
        
        Assert.IsType<GreenTokenWithLeadingTrivia>(result);
        Assert.Equal(" x", result.ToFullString());
    }

    [Fact]
    public void WithTrailingTrivia_Extension_CreatesCorrectType()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "x");
        var trivia = new GreenTrivia(1, "\n");
        
        var result = token.WithTrailingTrivia(trivia);
        
        Assert.IsType<GreenTokenWithTrailingTrivia>(result);
        Assert.Equal("x\n", result.ToFullString());
    }

    [Fact]
    public void WithLeadingTrivia_OnTokenWithTrailing_CreatesWithBoth()
    {
        var trailing = new GreenTrivia(1, "\n");
        var token = new GreenTokenWithTrailingTrivia(TestKind, "x", trailing);
        var leading = new GreenTrivia(1, " ");
        
        var result = token.WithLeadingTrivia(leading);
        
        Assert.IsType<GreenTokenWithTrivia>(result);
        Assert.Equal(" x\n", result.ToFullString());
    }

    #endregion
}
