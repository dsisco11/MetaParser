//HintName: MetaParser.MetaParser.parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingSyntaxToken(global::System.ReadOnlySpan<byte> buffer0)
        {
            switch (buffer0)
            {
                case [TokenId.Keyword_Var, ..]:
                {
                    return new ConsumerResult (TokenId.Typename, 1);
                }
                case [TokenId.Keyword_Byte, ..]:
                {
                    return new ConsumerResult (TokenId.Typename, 1);
                }
                case [TokenId.Keyword_Short, ..]:
                {
                    return new ConsumerResult (TokenId.Typename, 1);
                }
                case [TokenId.Keyword_Int, ..]:
                {
                    return new ConsumerResult (TokenId.Typename, 1);
                }
                case [TokenId.Keyword_Float, ..]:
                {
                    return new ConsumerResult (TokenId.Typename, 1);
                }
                case [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..]:
                {
                    return consume_pattern_33(buffer0);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
                {
                    return consume_pattern_25(buffer0);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Solidus, ..]:
                {
                    return consume_pattern_24(buffer0);
                }
                case [TokenId.Identifier, TokenId.Char_Colon, ..]:
                {
                    return consume_pattern_31(buffer0);
                }
            }
            return new (default, default);
            
        }
    }
}
