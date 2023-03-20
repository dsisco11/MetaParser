//HintName: MetaParser.MetaParser.parser.tokens.complex.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessComplex(global::System.ReadOnlySpan<byte> buffer0)
        {
            // Recursive tokens
            if (starts_codeblock_token(buffer0))
            {
                return consume_pattern_32(buffer0);
            }
            
            return new (default, default);
            
        }
    }
}
