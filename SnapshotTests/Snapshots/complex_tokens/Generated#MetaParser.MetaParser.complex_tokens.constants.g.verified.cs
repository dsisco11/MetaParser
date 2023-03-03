//HintName: MetaParser.MetaParser.complex_tokens.constants.g.cs
namespace UnitTestParser;
{
    internal static class TokenId
    {
        public const byte Unknown = 0;
        public const byte Keyword_Var = 1;
        public const byte Keyword_Function = 2;
        public const byte Keyword_Byte = 3;
        public const byte Keyword_Short = 4;
        public const byte Keyword_Int = 5;
        public const byte Keyword_Float = 6;
        public const byte Char_Open_Bracket = 7;
        public const byte Char_Close_Bracket = 8;
        public const byte Char_Open_Sqbracket = 9;
        public const byte Char_Close_Sqbracket = 10;
        public const byte Char_Open_Parenthesis = 11;
        public const byte Char_Close_Parenthesis = 12;
        public const byte Char_Asterisk = 13;
        public const byte Char_Solidus = 14;
        public const byte Char_Reverse_Solidus = 15;
        public const byte Whitespace = 16;
        public const byte Digits = 17;
        public const byte Letters = 18;
        public const byte Newline = 19;
        public const byte Identifier = 20;
        public const byte Typename = 21;
        public const byte Comment = 22;
        public const byte Codeblock = 23;
    }
}
