//HintName: MetaParser.MetaParser.parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingSyntaxToken(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                ["char_open_bracket", ..] => consume_pattern_63(buffer0),
                ["keyword_byte", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Float, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Int, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Short, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ["keyword_short", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Var, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ["keyword_int", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ["keyword_float", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ["keyword_var", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Byte, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..] => consume_pattern_64(buffer0),
                [("keyword_var" or "keyword_function"), ("whitespace" or "identifier" or "whitespace" or "char_open_parenthesis" or "whitespace" or "char_close_parenthesis" or "whitespace" or "char_open_bracket" or "whitespace" or "char_close_bracket"), ..] => consume_pattern_65(buffer0),
                ["char_solidus", "char_asterisk", ..] => consume_pattern_49(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_47(buffer0),
                ["char_solidus", "char_solidus", ..] => consume_pattern_48(buffer0),
                ["identifier", "char_colon", ..] => consume_pattern_61(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_46(buffer0),
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_60(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
