//HintName: MetaParser.MetaParser.parser.tokens.consumption.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_25(buffer0),
                ['/', '*', ..] => consume_pattern_22(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_24(buffer0),
                ['/', '/', ..] => consume_pattern_23(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_31(buffer0),
                _ => new (default, default)
            };
        }
    }
}
