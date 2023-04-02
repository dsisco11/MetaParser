//HintName: MetaParser.MetaParser.parser.parsing_table.stage_6.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_6(global::System.ReadOnlySpan<byte> buffer0)
        {
            // Recursive tokens
            if (starts_syntax_codeblock_token(buffer0))
            {
                return consume_pattern_62(buffer0);
            }
            
            return buffer0 switch
            {
                [TokenId.Char_Solidus, TokenId.Char_Asterisk, ..] => consume_pattern_47(buffer0),
                [TokenId.Char_Solidus, TokenId.Char_Solidus, ..] => consume_pattern_46(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
