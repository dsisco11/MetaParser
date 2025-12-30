using MetaParser.Syntax.Green;
using Xunit;

namespace UnitTests.Syntax.Green;

/// <summary>
/// Tests for GreenTokenFactory caching and creation.
/// </summary>
public class GreenTokenFactoryTests
{
    private const ushort KeywordKind = 10;
    private const ushort IdentifierKind = 20;
    private const ushort TriviaKind = 1;

    #region Caching Tests

    [Fact]
    public void CreateToken_SameKeyword_ReturnsCachedInstance()
    {
        var factory = new GreenTokenFactory();
        
        var token1 = factory.CreateToken(KeywordKind, "if");
        var token2 = factory.CreateToken(KeywordKind, "if");
        
        Assert.Same(token1, token2);
    }

    [Fact]
    public void CreateToken_DifferentKeywords_ReturnsDifferentInstances()
    {
        var factory = new GreenTokenFactory();
        
        var token1 = factory.CreateToken(KeywordKind, "if");
        var token2 = factory.CreateToken(KeywordKind, "else");
        
        Assert.NotSame(token1, token2);
    }

    [Fact]
    public void CreateToken_SameTextDifferentKind_ReturnsDifferentInstances()
    {
        var factory = new GreenTokenFactory();
        
        var token1 = factory.CreateToken(10, "class");
        var token2 = factory.CreateToken(20, "class"); // Same text, different kind
        
        Assert.NotSame(token1, token2);
    }

    [Fact]
    public void CreateToken_LongString_NotCached()
    {
        var factory = new GreenTokenFactory();
        var longText = "this_is_a_very_long_identifier_name";
        
        var token1 = factory.CreateToken(IdentifierKind, longText);
        var token2 = factory.CreateToken(IdentifierKind, longText);
        
        // Long strings (>16 chars) are not cached
        Assert.NotSame(token1, token2);
        Assert.Equal(longText, token1.GetText());
        Assert.Equal(longText, token2.GetText());
    }

    [Fact]
    public void CacheCount_TracksItems()
    {
        var factory = new GreenTokenFactory();
        
        Assert.Equal(0, factory.CacheCount);
        
        factory.CreateToken(KeywordKind, "if");
        factory.CreateToken(KeywordKind, "else");
        factory.CreateToken(KeywordKind, "while");
        
        Assert.Equal(3, factory.CacheCount);
    }

    [Fact]
    public void ClearCache_ResetsCount()
    {
        var factory = new GreenTokenFactory();
        
        factory.CreateToken(KeywordKind, "if");
        factory.CreateToken(KeywordKind, "else");
        
        Assert.Equal(2, factory.CacheCount);
        
        factory.ClearCache();
        
        Assert.Equal(0, factory.CacheCount);
    }

    [Fact]
    public void CreateToken_AfterClear_CreatesNewInstance()
    {
        var factory = new GreenTokenFactory();
        
        var token1 = factory.CreateToken(KeywordKind, "if");
        factory.ClearCache();
        var token2 = factory.CreateToken(KeywordKind, "if");
        
        Assert.NotSame(token1, token2);
    }

    #endregion

    #region Memory-Backed Tokens

    [Fact]
    public void CreateToken_Memory_NotCached()
    {
        var factory = new GreenTokenFactory();
        var source = "identifier".AsMemory();
        
        var token1 = factory.CreateToken(IdentifierKind, source);
        var token2 = factory.CreateToken(IdentifierKind, source);
        
        // Memory-backed tokens are never cached
        Assert.NotSame(token1, token2);
        Assert.Equal("identifier", token1.GetText());
    }

    #endregion

    #region Trivia Creation

    [Fact]
    public void CreateTrivia_ReturnsGreenTrivia()
    {
        var factory = new GreenTokenFactory();
        
        var trivia = factory.CreateTrivia(TriviaKind, "  ");
        
        Assert.IsType<GreenTrivia>(trivia);
        Assert.Equal("  ", trivia.GetText());
        Assert.Equal(2, trivia.FullWidth);
    }

    [Fact]
    public void CreateTriviaList_ReturnsList()
    {
        var factory = new GreenTokenFactory();
        var t1 = factory.CreateTrivia(1, " ");
        var t2 = factory.CreateTrivia(2, "// comment\n");
        
        var list = factory.CreateTriviaList(0, t1, t2);
        
        Assert.IsType<GreenTriviaList>(list);
        Assert.Equal(2, list.Count);
        Assert.Equal(12, list.FullWidth);
    }

    #endregion

    #region Tokens With Trivia

    [Fact]
    public void CreateTokenWithLeadingTrivia_Succeeds()
    {
        var factory = new GreenTokenFactory();
        var trivia = factory.CreateTrivia(TriviaKind, "  ");
        
        var token = factory.CreateTokenWithLeadingTrivia(KeywordKind, trivia, "if");
        
        Assert.Equal("  if", token.ToFullString());
        Assert.NotNull(token.LeadingTrivia);
    }

    [Fact]
    public void CreateTokenWithTrailingTrivia_Succeeds()
    {
        var factory = new GreenTokenFactory();
        var trivia = factory.CreateTrivia(TriviaKind, "\n");
        
        var token = factory.CreateTokenWithTrailingTrivia(KeywordKind, "if", trivia);
        
        Assert.Equal("if\n", token.ToFullString());
        Assert.NotNull(token.TrailingTrivia);
    }

    [Fact]
    public void CreateTokenWithTrivia_Succeeds()
    {
        var factory = new GreenTokenFactory();
        var leading = factory.CreateTrivia(TriviaKind, " ");
        var trailing = factory.CreateTrivia(TriviaKind, " ");
        
        var token = factory.CreateTokenWithTrivia(KeywordKind, leading, "if", trailing);
        
        Assert.Equal(" if ", token.ToFullString());
        Assert.NotNull(token.LeadingTrivia);
        Assert.NotNull(token.TrailingTrivia);
    }

    #endregion

    #region Default Factory

    [Fact]
    public void DefaultFactory_IsSingleton()
    {
        var instance1 = DefaultGreenTokenFactory.Instance;
        var instance2 = DefaultGreenTokenFactory.Instance;
        
        Assert.Same(instance1, instance2);
    }

    #endregion
}
