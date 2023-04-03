//HintName: MetaParser.MetaParser.parser.parsing_table.stage_0.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_0(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [(' ' or '\t' or '\f'), ..] => consume_pattern_17(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_19(buffer0),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                [(>="0" and <="9"), ..] => consume_pattern_18(buffer0),
                ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                ['f', 'l', 'o', 'a', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                ['s', 'h', 'o', 'r', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ['b', 'y', 't', 'e', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ['v', 'a', 'r', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                ['i', 'n', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                ['/', '/', ..] => consume_pattern_22(buffer0),
                ['/', '*', ..] => consume_pattern_21(buffer0),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_20(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
