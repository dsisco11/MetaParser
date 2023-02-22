//HintName: MetaParser.MetaParser.complex_tokens.enum.g.cs
namespace Foo.Bar.Tokens;
public enum ETokenType : byte
{
    Unknown = (byte) 0,
    Keyword_Var = (byte) 1,
    Keyword_Function = (byte) 2,
    Char_Open_Bracket = (byte) 3,
    Char_Close_Bracket = (byte) 4,
    Char_Asterisk = (byte) 5,
    Char_Solidus = (byte) 6,
    Char_Reverse_Solidus = (byte) 7,
    Whitespace = (byte) 8,
    Digits = (byte) 9,
    Letters = (byte) 10,
    Newline = (byte) 11,
    Identifier = (byte) 12,
    Comment = (byte) 13,
}
