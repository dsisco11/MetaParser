//HintName: MetaParser.MetaParser.linear_parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessComplex(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            // Recursive consumers
            if (is_consumer_start_27(input))
            {
                id = TokenId.Comment;
                return consume_pattern_27(input, out length);
            }
            if (is_consumer_start_28(input))
            {
                id = TokenId.Comment;
                return consume_pattern_28(input, out length);
            }
            if (is_consumer_start_29(input))
            {
                id = TokenId.Codeblock;
                return consume_pattern_29(input, out length);
            }
            
            id = default;
            length = default;
            return false;
            
            bool is_consumer_start_27(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Solidus, TokenId.Solidus, ..] => true,
                    _ => false
                };
            }
            bool is_consumer_start_28(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Solidus, TokenId.Asterisk, ..] => true,
                    _ => false
                };
            }
            bool is_consumer_start_29(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Open_Bracket, ..] => true,
                    _ => false
                };
            }
        }
    }
}
