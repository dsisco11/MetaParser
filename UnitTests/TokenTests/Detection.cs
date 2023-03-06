using UnitTestParser;

namespace UnitTests.TokenTests;

/// <summary>
/// Validates correct sequence of tokens as output
/// </summary>
/// <param name="input"></param>
/// <param name="expected"></param>
public class Detection
{
    [Theory]
    [InlineData("\n", ETokenType.Newline)]
    [InlineData(" ", ETokenType.Whitespace)]
    [InlineData("\t", ETokenType.Whitespace)]
    [InlineData("*", ETokenType.Char_Asterisk)]
    [InlineData("{", ETokenType.Char_Open_Bracket)]
    [InlineData("}", ETokenType.Char_Close_Bracket)]
    [InlineData("[", ETokenType.Char_Open_Sqbracket)]
    [InlineData("]", ETokenType.Char_Close_Sqbracket)]
    [InlineData("(", ETokenType.Char_Open_Parenthesis)]
    [InlineData(")", ETokenType.Char_Close_Parenthesis)]
    [InlineData("/", ETokenType.Char_Solidus)]
    [InlineData("\\", ETokenType.Char_Reverse_Solidus)]
    [InlineData("var", ETokenType.Typename)]
    [InlineData("byte", ETokenType.Typename)]
    [InlineData("short", ETokenType.Typename)]
    [InlineData("int", ETokenType.Typename)]
    [InlineData("float", ETokenType.Typename)]
    [InlineData("function", ETokenType.Keyword_Function)]
    public void Single(string input, ETokenType expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Single(results);
        Assert.Equal(expected, results.Single().Id);
    }

    [Theory]
    [InlineData(" \n*/", ETokenType.Whitespace, ETokenType.Newline, ETokenType.Char_Asterisk, ETokenType.Char_Solidus)]
    [InlineData("123 hello", ETokenType.Digits, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("123 hello world", ETokenType.Digits, ETokenType.Whitespace, ETokenType.Identifier, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("foo123   bar", ETokenType.Identifier, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("{ var int i; }", ETokenType.Codeblock)]
    [InlineData("/*hello world*/\n  ", ETokenType.Comment, ETokenType.Newline, ETokenType.Whitespace)]
    [InlineData(@"/*hello\*world*/  ", ETokenType.Comment, ETokenType.Whitespace)]
    [InlineData(@"hello:::bar", ETokenType.Identifier, ETokenType.Unknown, ETokenType.Identifier)]
    public void Multi(string input, params ETokenType[] expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Equal(expected, results.Select(o => o.Id));
    }
}