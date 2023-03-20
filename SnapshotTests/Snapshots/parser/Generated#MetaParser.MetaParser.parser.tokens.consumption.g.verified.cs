//HintName: MetaParser.MetaParser.parser.tokens.consumption.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool consume_comment_token(global::System.ReadOnlySpan<byte> buffer0, out Token outToken, out int consumeCount)
        {
            switch (buffer0)
            {
                case [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_25(buffer0, out length);
                }
                case ['/', '*', ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_22(buffer0, out length);
                }
                case [TokenId.Char_Solidus, TokenId.Char_Solidus, ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_24(buffer0, out length);
                }
                case ['/', '/', ..]:
                {
                    id = TokenId.Comment;
                    return consume_pattern_23(buffer0, out length);
                }
            }
        }
        private static bool consume_declaration_token(global::System.ReadOnlySpan<byte> buffer0, out Token outToken, out int consumeCount)
        {
            switch (buffer0)
            {
                case [TokenId.Identifier, TokenId.Char_Colon, ..]:
                {
                    id = TokenId.Declaration;
                    return consume_pattern_31(buffer0, out length);
                }
            }
        }
    }
}
