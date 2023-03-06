//HintName: MetaParser.MetaParser.recursive_parser.enum.g.cs
namespace UnitTestParser;
public enum ETokenType : byte
{
    Unknown = (byte) 0,
    Colon = (byte) 1,
    Semicolon = (byte) 2,
    Open_Bracket = (byte) 3,
    Close_Bracket = (byte) 4,
    Open_Sqbracket = (byte) 5,
    Close_Sqbracket = (byte) 6,
    Open_Parenthesis = (byte) 7,
    Close_Parenthesis = (byte) 8,
    Whitespace = (byte) 9,
    Newline = (byte) 10,
    Identifier = (byte) 11,
    Comment = (byte) 12,
    Declaration = (byte) 13,
    Codeblock = (byte) 14,
}
