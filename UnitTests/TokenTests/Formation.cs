using UnitTestParser;
namespace UnitTests.TokenTests;

/// <summary>
/// Validates resulting token data structure
/// </summary>
public class Formation
{
    /// <summary>
    /// Validates that a single token is output which encompasses the entire input
    /// </summary>
    /// <param name="input"></param>
    /// <param name="expected"></param>
    [Theory]
    [InlineData("  \t     \f ", ETokenType.Whitespace)]
    [InlineData("0123456789", ETokenType.Digits)]
    [InlineData("/*hello world*/", ETokenType.Comment)]
    [InlineData("::;;;::;;", ETokenType.Unknown)]
    public void Single(string input, ETokenType expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());

        Assert.Single(results);
        var token = results.First();
        Assert.Equal(expected, token.Id);
        Assert.NotEmpty(token.Values);

        // verify token starts at input start
        var inputStart = input.AsMemory()[0..1];
        var tokenStartData = token.Values.First().Data;
        var tokenStart = tokenStartData[0..1];
        Assert.Equal(inputStart, tokenStart);

        // verify token ends at input end
        var inputEnd = input.AsMemory()[^1..^0];
        var tokenEndData = token.Values.Last().Data;
        var tokenEnd = tokenEndData[^1..^0];
        Assert.Equal(inputEnd, tokenEnd);
    }

    /// <summary>
    /// Validates the output of a token which encompasses a specific range of the input
    /// </summary>
    /// <param name="input"></param>
    /// <param name="expected"></param>
    [Theory]
    [InlineData("   \t    \f ", 0, ETokenType.Whitespace, 0, 10)]
    [InlineData("0123456789", 0, ETokenType.Digits, 0, 10)]
    [InlineData("123   4567", 0, ETokenType.Digits, 0, 3)]
    [InlineData("123   4567", 1, ETokenType.Whitespace, 3, 6)]
    [InlineData("123   4567", 2, ETokenType.Digits, 6, 10)]
    [InlineData("123/*456*/78", 0, ETokenType.Digits, 0, 3)]
    [InlineData("123/*456*/78", 1, ETokenType.Comment, 3, 10)]
    [InlineData("123/*456*/78", 2, ETokenType.Digits, 10, 12)]

    [InlineData("123:::4567", 0, ETokenType.Digits, 0, 3)]
    [InlineData("123:::4567", 1, ETokenType.Unknown, 3, 6)]
    [InlineData("123:::4567", 2, ETokenType.Digits, 6, 10)]
    public void Segment(string input, int indice, ETokenType id, int startIndex, int endIndex)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());

        Assert.True(results.Count > indice);
        Assert.NotNull(results[indice]);
        var token = results[indice];
        Assert.Equal(id, token.Id);
        Assert.NotEmpty(token.Values);

        // verify that the start of the tokens first value matches the range start
        var inputStartRange = new Range(startIndex, startIndex + 1);
        var inputStart = input.AsMemory()[inputStartRange];
        var tokenStartData = token.Values.First().Data;
        var tokenStart = tokenStartData[0..1];
        Assert.Equal(inputStart, tokenStart);

        // verify that the end of the tokens last value matches the range end
        var inputEndRange = new Range(endIndex - 1, endIndex);
        var inputEnd = input.AsMemory()[inputEndRange];
        var tokenEndData = token.Values.Last().Data;
        var tokenEnd = tokenEndData[^1..^0];
        Assert.Equal(inputEnd, tokenEnd);
    }
}