//HintName: MetaParser.MetaParser.parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingLexerToken(global::System.ReadOnlySpan<char> buffer0)
        {
            return buffer0 switch
            {
                [(' ' or '\t' or '\f'), ..] => consume_pattern_35(buffer0),
                [(' ' or '\t' or '\f'), ..] => consume_pattern_34(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_38(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_39(buffer0),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ["var", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                ["byte", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ["short", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ["float", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                ['}', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Bracket, 1),
                ['[', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Sqbracket, 1),
                ['{', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Bracket, 1),
                ["int", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                [']', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Sqbracket, 1),
                ["function", ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                [':', ..] => new ConsumerResult (TokenId.Lexer_Char_Colon, 1),
                [';', ..] => new ConsumerResult (TokenId.Lexer_Char_Semicolon, 1),
                [')', ..] => new ConsumerResult (TokenId.Lexer_Char_Close_Parenthesis, 1),
                ['(', ..] => new ConsumerResult (TokenId.Lexer_Char_Open_Parenthesis, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                ['/', ..] => new ConsumerResult (TokenId.Lexer_Char_Solidus, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Lexer_Char_Reverse_Solidus, 1),
                ['*', ..] => new ConsumerResult (TokenId.Lexer_Char_Asterisk, 1),
                [(>="0" and <="9"), ..] => consume_pattern_36(buffer0),
                [(>="0" and <="9"), ..] => consume_pattern_37(buffer0),
                ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Function, 1),
                ['s', 'h', 'o', 'r', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Short, 1),
                ['f', 'l', 'o', 'a', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Float, 1),
                ['b', 'y', 't', 'e', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Byte, 1),
                ['v', 'a', 'r', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Var, 1),
                ['i', 'n', 't', ..] => new ConsumerResult (TokenId.Lexer_Keyword_Int, 1),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_40(buffer0),
                [((>="a" and <="z") or (>="A" and <="Z")), ((>="a" and <="z") or (>="A" and <="Z") or (>="0" and <="9") or '-' or '_'), ..] => consume_pattern_41(buffer0),
                ["/*", ..] => consume_pattern_44(buffer0),
                ["//", ..] => consume_pattern_45(buffer0),
                ['/', '/', ..] => consume_pattern_43(buffer0),
                ['/', '*', ..] => consume_pattern_42(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
