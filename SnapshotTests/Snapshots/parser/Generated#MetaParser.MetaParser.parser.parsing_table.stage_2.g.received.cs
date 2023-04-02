//HintName: MetaParser.MetaParser.parser.parsing_table.stage_2.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static byte process_parser_table_2(global::System.ReadOnlySpan<byte> buffer0)
        {
            return buffer0 switch
            {
                [(' ' or '\t' or '\f'), ..] => consume_pattern_34(buffer0),
                [(' ' or '\t' or '\f'), ..] => consume_pattern_35(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_38(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_39(buffer0),
                ["function", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                ["keyword_int", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                ["keyword_byte", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ["short", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ["int", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ["keyword_float", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                ["keyword_short", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                ["char_open_bracket", ..] => consume_pattern_63(buffer0),
                ["byte", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                ["float", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                ["var", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                ["keyword_var", ..] => new ConsumerResult (TokenId.Syntax_Typename, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                [(>="0" and <="9"), ..] => consume_pattern_36(buffer0),
                [(>="0" and <="9"), ..] => consume_pattern_37(buffer0),
                ["char_solidus", "char_asterisk", ..] => consume_pattern_49(buffer0),
                ["identifier", "char_colon", ..] => consume_pattern_61(buffer0),
                ["char_solidus", "char_solidus", ..] => consume_pattern_48(buffer0),
                ["//", ..] => consume_pattern_45(buffer0),
                ["/*", ..] => consume_pattern_44(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
