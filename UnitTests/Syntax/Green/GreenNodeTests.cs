using MetaParser.Syntax.Green;
using Xunit;

namespace UnitTests.Syntax.Green;

/// <summary>
/// Tests for GreenNode base class functionality.
/// </summary>
public class GreenNodeTests
{
    private const ushort TestKind = 42;

    [Fact]
    public void GreenToken_Width_ReturnsTextLength()
    {
        var token = new GreenTokenWithNoTrivia(TestKind, "hello");
        
        Assert.Equal(5, token.Width);
        Assert.Equal(5, token.FullWidth);
    }

    [Fact]
    public void GreenToken_WidthWithTrivia_IncludesTrivia()
    {
        var leading = new GreenTrivia(1, "  ");
        var trailing = new GreenTrivia(1, "\n");
        var token = new GreenTokenWithTrivia(TestKind, leading, "hello", trailing);
        
        Assert.Equal(5, token.Width);  // Just token text
        Assert.Equal(8, token.FullWidth);  // 2 + 5 + 1
        Assert.Equal(2, token.LeadingTriviaWidth);
        Assert.Equal(1, token.TrailingTriviaWidth);
    }

    [Fact]
    public void GreenTrivia_Width_ReturnsTextLength()
    {
        var trivia = new GreenTrivia(1, "   ");
        
        Assert.Equal(3, trivia.Width);
        Assert.Equal(3, trivia.FullWidth);
    }

    [Fact]
    public void GreenTriviaList_Width_SumsChildren()
    {
        var t1 = new GreenTrivia(1, "  ");
        var t2 = new GreenTrivia(2, "// comment\n");
        var list = new GreenTriviaList(0, new[] { t1, t2 });
        
        Assert.Equal(13, list.FullWidth);  // 2 + 11
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void GreenNode_Flags_AreSetCorrectly()
    {
        var trivia = new GreenTrivia(1, " ");
        var tokenNoTrivia = new GreenTokenWithNoTrivia(TestKind, "x");
        var tokenWithTrivia = new GreenTokenWithLeadingTrivia(TestKind, trivia, "x");
        
        Assert.True(trivia.IsTrivia);
        Assert.False(trivia.IsToken);
        
        Assert.True(tokenNoTrivia.IsToken);
        Assert.False(tokenNoTrivia.IsTrivia);
        Assert.False(tokenNoTrivia.ContainsTrivia);
        
        Assert.True(tokenWithTrivia.IsToken);
        Assert.True(tokenWithTrivia.ContainsTrivia);
    }
}
