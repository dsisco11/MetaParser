//HintName: MetaParser.MetaParser.parser.tokens.consumption.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult consume_lexer_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                ["//", ..] => consume_pattern_45(buffer0),
                ["/*", ..] => consume_pattern_44(buffer0),
                ['/', '/', ..] => consume_pattern_43(buffer0),
                ['/', '*', ..] => consume_pattern_42(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                ["char_solidus", "char_asterisk", ..] => consume_pattern_49(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_47(buffer0),
                ["char_solidus", "char_solidus", ..] => consume_pattern_48(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_46(buffer0),
                _ => new (default, default)
            };
        }
        private static ConsumerResult consume_syntax_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_60(buffer0),
                ["identifier", "char_colon", ..] => consume_pattern_61(buffer0),
                _ => new (default, default)
            };
        }
    }
}
