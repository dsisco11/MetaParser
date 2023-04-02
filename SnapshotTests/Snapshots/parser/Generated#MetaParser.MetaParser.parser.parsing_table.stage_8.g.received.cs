//HintName: MetaParser.MetaParser.parser.parsing_table.stage_8.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_8(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [(TokenId.Keyword_Var or TokenId.Keyword_Function), (TokenId.Whitespace or TokenId.Identifier or TokenId.Whitespace or TokenId.Char_Open_Parenthesis or TokenId.Whitespace or TokenId.Char_Close_Parenthesis or TokenId.Whitespace or TokenId.Char_Open_Bracket or TokenId.Whitespace or TokenId.Char_Close_Bracket), ..] => consume_pattern_64(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
