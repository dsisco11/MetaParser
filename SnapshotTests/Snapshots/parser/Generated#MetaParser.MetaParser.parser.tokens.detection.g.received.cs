//HintName: MetaParser.MetaParser.parser.tokens.detection.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool is_comment_token_start(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Comment, ..] => true,
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => true,
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => true,
                _ => false
            };
        }
        private static bool is_declaration_token_start(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => true,
                _ => false
            };
        }
    }
}
