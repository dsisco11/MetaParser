//HintName: MetaParser.MetaParser.parser.parsing_table.stage_0.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_0(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [('\r' or '\n'), ..] => consume_pattern_1(buffer0),
                [(' ' or '\t' or '\f'), ..] => consume_pattern_0(buffer0),
                ['"', ..] => new ConsumerResult (TokenId.Lexer_Char_Double_Quote, 1),
                ['\'', ..] => new ConsumerResult (TokenId.Lexer_Char_Single_Quote, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                ['@', ..] => new ConsumerResult (TokenId.Lexer_Char_At_Symbol, 1),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                [(>="0" and <="9"), ..] => consume_pattern_24(buffer0),
                ['/', '*', ..] => consume_pattern_26(buffer0),
                ['/', '/', ..] => consume_pattern_27(buffer0),
                ['b', 'y', 't', 'e', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ['f', 'l', 'o', 'a', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                ['i', 'n', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                ['s', 'h', 'o', 'r', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ['u', 'i', 'n', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Uint, 1),
                ['v', 'a', 'r', 's', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Vars, 1),
                ['v', 'a', 'r', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_25(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
