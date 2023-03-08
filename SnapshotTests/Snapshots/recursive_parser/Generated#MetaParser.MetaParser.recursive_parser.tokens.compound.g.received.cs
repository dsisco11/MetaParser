//HintName: MetaParser.MetaParser.recursive_parser.tokens.compound.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static bool TryProcessCompound(global::System.ReadOnlySpan<byte> input, out byte id, out int length)
        {
            // Recursive consumers
            if (is_consumer_start_14(input))
            {
                id = TokenId.Comment;
                return consume_pattern_14(input, out length);
            }
            if (is_consumer_start_15(input))
            {
                id = TokenId.Declaration;
                return consume_pattern_15(input, out length);
            }
            if (is_consumer_start_16(input))
            {
                id = TokenId.Codeblock;
                return consume_pattern_16(input, out length);
            }
            
            id = default;
            length = default;
            return false;
            
            bool is_consumer_start_14(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Solidus, TokenId.Solidus, ..] => true,
                    _ => false
                };
            }
            bool is_consumer_start_15(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Identifier, TokenId.Colon, ..] => true,
                    _ => false
                };
            }
            bool is_consumer_start_16(global::System.ReadOnlySpan<byte> input)
            {
                return input switch
                {
                    [TokenId.Open_Bracket, var buffer] when (is_declaration_token_start(buffer)) => true,
                    _ => false
                };
            }
        }
    }
}
