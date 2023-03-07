//HintName: MetaParser.MetaParser.recursive_parser.enum.g.cs
namespace UnitTestParser;
public enum ETokenType : byte
{
    Unknown = (byte) 0,
    Solidus = (byte) 1,
    Colon = (byte) 2,
    Semicolon = (byte) 3,
    Open_Bracket = (byte) 4,
    Close_Bracket = (byte) 5,
    Open_Sqbracket = (byte) 6,
    Close_Sqbracket = (byte) 7,
    Open_Parenthesis = (byte) 8,
    Close_Parenthesis = (byte) 9,
    Whitespace = (byte) 10,
    Newline = (byte) 11,
    Identifier = (byte) 12,
    Comment = (byte) 13,
    Declaration = (byte) 14,
    Codeblock = (byte) 15,
}
