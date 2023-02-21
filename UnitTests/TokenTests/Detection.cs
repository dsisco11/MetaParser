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
    [InlineData("\n", EToken.Newline)]
    [InlineData(" ", EToken.Whitespace)]
    [InlineData("\t", EToken.Whitespace)]
    [InlineData("*", EToken.Char_Asterisk)]
    [InlineData("{", EToken.Char_Open_Bracket)]
    [InlineData("}", EToken.Char_Close_Bracket)]
    [InlineData("[", EToken.Char_Open_Sqbracket)]
    [InlineData("]", EToken.Char_Close_Sqbracket)]
    [InlineData("(", EToken.Char_Open_Parenthesis)]
    [InlineData(")", EToken.Char_Close_Parenthesis)]
    [InlineData("/", EToken.Char_Solidus)]
    [InlineData("\\", EToken.Char_Reverse_Solidus)]
    [InlineData("var", EToken.Keyword_Var)]
    [InlineData("byte", EToken.Keyword_Byte)]
    [InlineData("short", EToken.Keyword_Short)]
    [InlineData("int", EToken.Keyword_Int)]
    [InlineData("float", EToken.Keyword_Float)]
    [InlineData("function", EToken.Keyword_Function)]
    public void Single(string input, EToken expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Single(results);
        Assert.Equal<EToken>(expected, results.Single().Id);
    }

    [Theory]
    [InlineData(" \n*/", EToken.Whitespace, EToken.Newline, EToken.Char_Asterisk, EToken.Char_Solidus)]
    [InlineData("123 hello", EToken.Digits, EToken.Whitespace, EToken.Identifier)]
    [InlineData("123 hello world", EToken.Digits, EToken.Whitespace, EToken.Identifier, EToken.Whitespace, EToken.Identifier)]
    [InlineData("foo123   bar", EToken.Identifier, EToken.Whitespace, EToken.Identifier)]
    [InlineData("{ var int i; }", EToken.Codeblock)]
    [InlineData("/*hello world*/\n  ", EToken.Comment, EToken.Newline, EToken.Whitespace)]
    [InlineData(@"/*hello\*world*/  ", EToken.Comment, EToken.Whitespace)]
    public void Multi(string input, params EToken[] expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Equal<EToken>(expected, results.Select(o => o.Id));
    }
}