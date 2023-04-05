//HintName: MetaParser.MetaParser.parser.parsing_table.stage_2.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_2(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Char_Open_Bracket, ..] => consume_pattern_42(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
