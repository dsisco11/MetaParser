//HintName: MetaParser.MetaParser.parser.tokens.detection.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool starts_lexer_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Lexer_Comment, ..] => true,
                _ => false
            };
        }
        private static bool starts_syntax_comment_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                ["char_solidus", "char_asterisk", ..] => true,
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => true,
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => true,
                _ => false
            };
        }
        private static bool starts_syntax_declaration_token(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Identifier, TokenId.Char_Colon, ..] => true,
                _ => false
            };
        }
    }
}
