//HintName: MetaParser.MetaParser.recursive_parser.tokens.detection.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool is_comment_token_start(global::System.ReadOnlySpan<byte> input)
        {
            return input switch
            {
                [TokenId.Comment, ..] => true,
                [TokenId.Solidus, TokenId.Solidus, ..] => true,
                _ => false
            };
        }
        private static bool is_declaration_token_start(global::System.ReadOnlySpan<byte> input)
        {
            return input switch
            {
                [TokenId.Identifier, TokenId.Colon, ..] => true,
                _ => false
            };
        }
    }
}
