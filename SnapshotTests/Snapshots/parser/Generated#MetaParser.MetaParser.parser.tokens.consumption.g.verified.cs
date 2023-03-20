//HintName: MetaParser.MetaParser.parser.tokens.consumption.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            switch (buffer0)
            {
                case [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
                {
                    return consume_pattern_25(buffer0);
                }
                case ['/', '*', ..]:
                {
                    return consume_pattern_22(buffer0);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Solidus, ..]:
                {
                    return consume_pattern_24(buffer0);
                }
                case ['/', '/', ..]:
                {
                    return consume_pattern_23(buffer0);
                }
            }
        }
        private static ConsumerResult consume_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            switch (buffer0)
            {
                case [TokenId.Identifier, TokenId.Char_Colon, ..]:
                {
                    return consume_pattern_31(buffer0);
                }
            }
        }
    }
}
