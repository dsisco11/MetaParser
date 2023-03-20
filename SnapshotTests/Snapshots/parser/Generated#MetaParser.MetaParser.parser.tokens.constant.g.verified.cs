//HintName: MetaParser.MetaParser.parser.tokens.constant.g.cs
namespace UnitTestParser
{
    public sealed partial class Parser
    {
        private static ConsumerResult TryProcessingLexerToken(global::System.ReadOnlySpan<char> buffer0)
        {
            switch (buffer0)
            {
                case ['f', 'u', 'n', 'c', 't', 'i', 'o', 'n', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Function, 8);
                }
                case ['s', 'h', 'o', 'r', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Short, 5);
                }
                case ['f', 'l', 'o', 'a', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Float, 5);
                }
                case ['b', 'y', 't', 'e', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Byte, 4);
                }
                case ['v', 'a', 'r', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Var, 3);
                }
                case ['i', 'n', 't', ..]:
                {
                    return new ConsumerResult (TokenId.Keyword_Int, 3);
                }
                case [(' ' or '\t' or '\f'), ..]:
                {
                    return consume_pattern_18(buffer0);
                }
                case [('\r' or '\n'), ..]:
                {
                    return consume_pattern_20(buffer0);
                }
                case [':', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Colon, 1);
                }
                case ['{', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Bracket, 1);
                }
                case ['}', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Bracket, 1);
                }
                case ['[', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Sqbracket, 1);
                }
                case [']', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Sqbracket, 1);
                }
                case [';', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Semicolon, 1);
                }
                case [')', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Close_Parenthesis, 1);
                }
                case ['*', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Asterisk, 1);
                }
                case ['/', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Solidus, 1);
                }
                case ['\\', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Reverse_Solidus, 1);
                }
                case ['(', ..]:
                {
                    return new ConsumerResult (TokenId.Char_Open_Parenthesis, 1);
                }
                case [(>='0' and <='9'), ..]:
                {
                    return consume_pattern_19(buffer0);
                }
                case [((>='a' and <='z') or (>='A' and <='Z')), ((>='a' and <='z') or (>='A' and <='Z') or (>='0' and <='9') or '-' or '_'), ..]:
                {
                    return consume_pattern_21(buffer0);
                }
                case ['/', '*', ..]:
                {
                    return consume_pattern_22(buffer0);
                }
                case ['/', '/', ..]:
                {
                    return consume_pattern_23(buffer0);
                }
            }
            return new (default, default);
            
        }
    }
}
