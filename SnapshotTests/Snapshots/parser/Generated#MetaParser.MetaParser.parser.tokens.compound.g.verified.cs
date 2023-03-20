//HintName: MetaParser.MetaParser.parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingSyntaxToken(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Keyword_Var, ..] => new ConsumerResult (TokenId.Typename, 1),
                [TokenId.Keyword_Byte, ..] => new ConsumerResult (TokenId.Typename, 1),
                [TokenId.Keyword_Short, ..] => new ConsumerResult (TokenId.Typename, 1),
                [TokenId.Keyword_Int, ..] => new ConsumerResult (TokenId.Typename, 1),
                [TokenId.Keyword_Float, ..] => new ConsumerResult (TokenId.Typename, 1),
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..] => consume_pattern_33(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_25(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_24(buffer0),
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_31(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
