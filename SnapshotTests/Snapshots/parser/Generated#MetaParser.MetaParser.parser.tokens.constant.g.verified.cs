//HintName: MetaParser.MetaParser.parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingLexerToken(global::System.ReadOnlySpan<char> buffer0)
        {
            return buffer0 switch
            {
                ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..] => new ConsumerResult (TokenId.Keyword_Function, 8),
                ['s', 'h', 'o', 'r', 't', ..] => new ConsumerResult (TokenId.Keyword_Short, 5),
                ['f', 'l', 'o', 'a', 't', ..] => new ConsumerResult (TokenId.Keyword_Float, 5),
                ['b', 'y', 't', 'e', ..] => new ConsumerResult (TokenId.Keyword_Byte, 4),
                ['v', 'a', 'r', ..] => new ConsumerResult (TokenId.Keyword_Var, 3),
                ['i', 'n', 't', ..] => new ConsumerResult (TokenId.Keyword_Int, 3),
                [(' ' or '\t' or '\f'), ..] => consume_pattern_18(buffer0),
                [('\r' or '\n'), ..] => consume_pattern_20(buffer0),
                [':', ..] => new ConsumerResult (TokenId.Char_Colon, 1),
                ['{', ..] => new ConsumerResult (TokenId.Char_Open_Bracket, 1),
                ['}', ..] => new ConsumerResult (TokenId.Char_Close_Bracket, 1),
                ['[', ..] => new ConsumerResult (TokenId.Char_Open_Sqbracket, 1),
                [']', ..] => new ConsumerResult (TokenId.Char_Close_Sqbracket, 1),
                [';', ..] => new ConsumerResult (TokenId.Char_Semicolon, 1),
                [')', ..] => new ConsumerResult (TokenId.Char_Close_Parenthesis, 1),
                ['*', ..] => new ConsumerResult (TokenId.Char_Asterisk, 1),
                ['/', ..] => new ConsumerResult (TokenId.Char_Solidus, 1),
                ['\\', ..] => new ConsumerResult (TokenId.Char_Reverse_Solidus, 1),
                ['(', ..] => new ConsumerResult (TokenId.Char_Open_Parenthesis, 1),
                [(>='0' and <='9'), ..] => consume_pattern_19(buffer0),
                [((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..] => consume_pattern_21(buffer0),
                ['/', '*', ..] => consume_pattern_22(buffer0),
                ['/', '/', ..] => consume_pattern_23(buffer0),
                _ => new (default, default)
            };
            
        }
    }
}
