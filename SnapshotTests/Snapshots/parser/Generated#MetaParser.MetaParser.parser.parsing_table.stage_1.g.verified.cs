//HintName: MetaParser.MetaParser.parser.parsing_table.stage_1.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_1(global::System.ReadOnlySpan<byte> buffer0)
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
                [TokenId.Char_Single_Quote, ..] => consume_pattern_35(buffer0),
                [TokenId.Char_Double_Quote, ..] => consume_pattern_36(buffer0),
                [TokenId.Char_At_Symbol, TokenId.Char_Single_Quote, ..] => consume_pattern_37(buffer0),
                [TokenId.Char_At_Symbol, TokenId.Char_Double_Quote, ..] => consume_pattern_38(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_39(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_40(buffer0),
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_41(buffer0),
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..] => consume_pattern_43(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
