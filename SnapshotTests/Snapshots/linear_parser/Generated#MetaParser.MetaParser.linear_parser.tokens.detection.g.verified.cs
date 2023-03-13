//HintName: MetaParser.MetaParser.linear_parser.tokens.detection.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool is_comment_token_start(global::System.ReadOnlySpan<byte> input)
        {
            return input switch
            {
                [TokenId.Comment, ..] => true,
                [TokenId.Solidus, TokenId.Asterisk, ..] => true,
                [TokenId.Solidus, TokenId.Solidus, ..] => true,
                _ => false
            };
        }
        private static bool is_codeblock_token_start(global::System.ReadOnlySpan<byte> input)
        {
            return input switch
            {
                [TokenId.Open_Bracket, ..] => true,
                _ => false
            };
        }
    }
}
