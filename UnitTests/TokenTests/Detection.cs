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
    [InlineData("var", ETokenType.Keyword_Var)]
    [InlineData("byte", ETokenType.Keyword_Byte)]
    [InlineData("short", ETokenType.Keyword_Short)]
    [InlineData("int", ETokenType.Keyword_Int)]
    [InlineData("float", ETokenType.Keyword_Float)]
    [InlineData("function", ETokenType.Keyword_Function)]
    public void Single(string input, ETokenType expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Single(results);
        Assert.Equal<ETokenType>(expected, results.Single().Id);
    }

    [Theory]
    [InlineData(" \n*/", ETokenType.Whitespace, ETokenType.Newline, ETokenType.Char_Asterisk, ETokenType.Char_Solidus)]
    [InlineData("123 hello", ETokenType.Digits, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("123 hello world", ETokenType.Digits, ETokenType.Whitespace, ETokenType.Identifier, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("foo123   bar", ETokenType.Identifier, ETokenType.Whitespace, ETokenType.Identifier)]
    [InlineData("{ var int i; }", ETokenType.Codeblock)]
    [InlineData("/*hello world*/\n  ", ETokenType.Comment, ETokenType.Newline, ETokenType.Whitespace)]
    [InlineData(@"/*hello\*world*/  ", ETokenType.Comment, ETokenType.Whitespace)]
    public void Multi(string input, params ETokenType[] expected)
    {
        var parser = new Parser();
        var results = parser.Parse(input.AsMemory());
        Assert.Equal<ETokenType>(expected, results.Select(o => o.Id));
    }
}