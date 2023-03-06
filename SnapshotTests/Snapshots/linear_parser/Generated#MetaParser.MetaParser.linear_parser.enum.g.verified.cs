//HintName: MetaParser.MetaParser.linear_parser.enum.g.cs
namespace UnitTestParser;
public enum ETokenType : byte
{
    Unknown = (byte) 0,
    Keyword_Var = (byte) 1,
    Keyword_Function = (byte) 2,
    Keyword_Byte = (byte) 3,
    Keyword_Short = (byte) 4,
    Keyword_Int = (byte) 5,
    Keyword_Float = (byte) 6,
    Char_Open_Bracket = (byte) 7,
    Char_Close_Bracket = (byte) 8,
    Char_Open_Sqbracket = (byte) 9,
    Char_Close_Sqbracket = (byte) 10,
    Char_Open_Parenthesis = (byte) 11,
    Char_Close_Parenthesis = (byte) 12,
    Char_Asterisk = (byte) 13,
    Char_Solidus = (byte) 14,
    Char_Reverse_Solidus = (byte) 15,
    Whitespace = (byte) 16,
    Digits = (byte) 17,
    Letters = (byte) 18,
    Newline = (byte) 19,
    Identifier = (byte) 20,
    Typename = (byte) 21,
    Comment = (byte) 22,
    Codeblock = (byte) 23,
}
