//HintName: MetaParser.MetaParser.parser.tokens.consumption.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_syntax_typename_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Keyword_Var, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Vars, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Byte, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Short, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Int, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Uint, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Float, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_string_single_line_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Single_Quote, ..] => consume_pattern_35(buffer0),
                [TokenId.Char_Double_Quote, ..] => consume_pattern_36(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_string_multi_line_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_At_Symbol, TokenId.Char_Single_Quote, ..] => consume_pattern_37(buffer0),
                [TokenId.Char_At_Symbol, TokenId.Char_Double_Quote, ..] => consume_pattern_38(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_39(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_40(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_41(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_codeblock_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Open_Bracket, ..] => consume_pattern_42(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_program_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..] => consume_pattern_43(buffer0),
                _ => new (default, default)
            };
        }
    }
}
