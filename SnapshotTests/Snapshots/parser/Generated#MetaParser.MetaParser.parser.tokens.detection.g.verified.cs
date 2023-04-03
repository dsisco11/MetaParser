//HintName: MetaParser.MetaParser.parser.tokens.detection.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool starts_syntax_typename_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Keyword_Var, ..] => true,
                [TokenId.Keyword_Byte, ..] => true,
                [TokenId.Keyword_Short, ..] => true,
                [TokenId.Keyword_Int, ..] => true,
                [TokenId.Keyword_Float, ..] => true,
                _ => false
            };
        }
        private static bool starts_syntax_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => true,
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => true,
                _ => false
            };
        }
        private static bool starts_syntax_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => true,
                _ => false
            };
        }
        private static bool starts_syntax_codeblock_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Open_Bracket, var buffer1] when (starts_declaration_token(buffer1)) => true,
                _ => false
            };
        }
        private static bool starts_syntax_program_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), var buffer1] when (starts_whitespace_token(buffer1) or starts_identifier_token(buffer1) or starts_whitespace_token(buffer1) or starts_char_open_parenthesis_token(buffer1) or starts_whitespace_token(buffer1) or starts_char_close_parenthesis_token(buffer1) or starts_whitespace_token(buffer1) or starts_char_open_bracket_token(buffer1) or starts_whitespace_token(buffer1) or starts_char_close_bracket_token(buffer1)) => true,
                _ => false
            };
        }
    }
}
