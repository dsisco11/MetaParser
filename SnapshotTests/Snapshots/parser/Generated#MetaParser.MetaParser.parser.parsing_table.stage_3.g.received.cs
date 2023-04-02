//HintName: MetaParser.MetaParser.parser.parsing_table.stage_3.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_3(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                ['s', 'h', 'o', 'r', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ['f', 'l', 'o', 'a', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                ['b', 'y', 't', 'e', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ['i', 'n', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                ['v', 'a', 'r', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                [("keyword_var" or "keyword_function"), ("whitespace" or "identifier" or "whitespace" or "char_open_parenthesis" or "whitespace" or "char_close_parenthesis" or "whitespace" or "char_open_bracket" or "whitespace" or "char_close_bracket"), ..] => consume_pattern_65(buffer0),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_40(buffer0),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_41(buffer0),
                ['/', '/', ..] => consume_pattern_43(buffer0),
                ['/', '*', ..] => consume_pattern_42(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
