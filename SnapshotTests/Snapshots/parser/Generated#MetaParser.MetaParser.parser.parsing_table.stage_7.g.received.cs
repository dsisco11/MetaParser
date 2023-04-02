//HintName: MetaParser.MetaParser.parser.parsing_table.stage_7.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_7(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [TokenId.Keyword_Int, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Short, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Byte, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Var, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Keyword_Float, ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [TokenId.Identifier, TokenId.Char_Colon, ..] => consume_pattern_60(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
