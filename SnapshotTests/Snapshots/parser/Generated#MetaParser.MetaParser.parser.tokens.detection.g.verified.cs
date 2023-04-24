//HintName: MetaParser.MetaParser.parser.tokens.detection.g.cs
namespace UnitTestParser;
public sealed partial class Parser
{
    private static bool starts_syntax_comment_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Char_Solidus, TokenId.Lexer_Char_Asterisk, ..] => true,
            [TokenId.Lexer_Char_Solidus, TokenId.Lexer_Char_Solidus, ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Identifier, TokenId.Lexer_Char_Colon, ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_program_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [(TokenId.Lexer_Keyword_Var or TokenId.Lexer_Keyword_Function), (TokenId.Lexer_Whitespace or TokenId.Lexer_Identifier or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Open_Parenthesis or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Close_Parenthesis or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Open_Bracket or TokenId.Lexer_Whitespace or TokenId.Lexer_Char_Close_Bracket), ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_string_multi_line_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Char_At_Symbol, TokenId.Lexer_Char_Double_Quote, ..] => true,
            [TokenId.Lexer_Char_At_Symbol, TokenId.Lexer_Char_Single_Quote, ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_string_single_line_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Char_Single_Quote, ..] => true,
            [TokenId.Lexer_Char_Double_Quote, ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_typename_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Keyword_Var, ..] => true,
            [TokenId.Lexer_Keyword_Vars, ..] => true,
            [TokenId.Lexer_Keyword_Byte, ..] => true,
            [TokenId.Lexer_Keyword_Short, ..] => true,
            [TokenId.Lexer_Keyword_Int, ..] => true,
            [TokenId.Lexer_Keyword_Uint, ..] => true,
            [TokenId.Lexer_Keyword_Float, ..] => true,
            _ => false
        };
    }
    private static bool starts_syntax_codeblock_token(global::System.ReadOnlySpan<byte> buffer0)
    {
        return buffer0 switch
        {
            [TokenId.Lexer_Char_Open_Bracket, ..] => true,
            _ => false
        };
    }
}
