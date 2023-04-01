namespace MetaParser.Json.Definitions;

internal enum EParsingStage
{
    Invalid = 0,
    /// <summary>Lexing is the process of mapping predefined data sequences into a reduced set of possible identifiers (tokens)</summary>
    Lexer = 1,
    /// <summary></summary>
    Syntax = 2,
}
