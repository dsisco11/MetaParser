using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.RuleSets.Text;
using static MetaParser.UnicodeCommon;

using TextTokenType = MetaParser.Tokens.TokenType<MetaParser.Parsing.ETextToken>;
using TextTokenRule = MetaParser.Rules.ITokenRule<MetaParser.Tokens.TokenType<MetaParser.Parsing.ETextToken>, char>;

namespace MetaParser.RuleSets
{
    public static class TextRules
    {
        public static readonly TextTokenRule Newline = new GroupSingleRule<TextTokenType, char>(ETextToken.Newline, CHAR_LINE_FEED);
        public static readonly TextTokenRule Whitespace = new GroupSetRule<TextTokenType, char>(ETextToken.Whitespace, ASCII_WHITESPACE);
        public static readonly TextTokenRule Whitespace_Except_Newline = new GroupSetRule<TextTokenType, char>(ETextToken.Whitespace, ASCII_WHITESPACE_EXCLUDING_NEWLINE);

        /// <summary>
        /// This rule captures sequences of legible 'word' characters, so any sequence of human-readable characters which are not whitespace or control characters
        /// </summary>
        public static readonly TextTokenRule Words = new WordBlockRule();

        /// <summary>
        /// Common symbols of importance for things like programming, markup, and configuration files.
        /// <para>: | ; , [ ] ( ) { } \< \> - = +</para>
        /// </summary>
        public static TextTokenRule[] CommonSymbols => new [] {
            new SingleRule<TextTokenType, char>(ETextToken.Colon, CHAR_COLON),
            new SingleRule<TextTokenType, char>(ETextToken.Column, CHAR_PIPE),
            new SingleRule<TextTokenType, char>(ETextToken.Semicolon, CHAR_SEMICOLON),
            new SingleRule<TextTokenType, char>(ETextToken.Comma, CHAR_COMMA),
            new SingleRule<TextTokenType, char>(ETextToken.SqBracketOpen, CHAR_LEFT_SQUARE_BRACKET),
            new SingleRule<TextTokenType, char>(ETextToken.SqBracketClose, CHAR_RIGHT_SQUARE_BRACKET),
            new SingleRule<TextTokenType, char>(ETextToken.ParenthOpen, CHAR_LEFT_PARENTHESES),
            new SingleRule<TextTokenType, char>(ETextToken.ParenthClose, CHAR_RIGHT_PARENTHESES),
            new SingleRule<TextTokenType, char>(ETextToken.BracketOpen, CHAR_LEFT_CURLY_BRACKET),
            new SingleRule<TextTokenType, char>(ETextToken.BracketClose, CHAR_RIGHT_CURLY_BRACKET),
            new SingleRule<TextTokenType, char>(ETextToken.LessThan, CHAR_LEFT_CHEVRON),
            new SingleRule<TextTokenType, char>(ETextToken.GreaterThan, CHAR_RIGHT_CHEVRON),
            new SingleRule<TextTokenType, char>(ETextToken.HypenMinus, CHAR_HYPHEN_MINUS),
            new SingleRule<TextTokenType, char>(ETextToken.Equals, CHAR_EQUALS),
            new SingleRule<TextTokenType, char>(ETextToken.Plus, CHAR_PLUS_SIGN),
            new SingleRule<TextTokenType, char>(ETextToken.Asterisk, CHAR_ASTERISK),
            new SingleRule<TextTokenType, char>(ETextToken.Solidus, CHAR_SOLIDUS),
            new SingleRule<TextTokenType, char>(ETextToken.ReverseSolidus, CHAR_REVERSE_SOLIDUS),
        };

        /// <summary>
        /// Code 'Objects' are more complex structures like comment blocks, function names, variable declarations, etc.
        /// </summary>
        public static TextTokenRule[] CodeStructures => new TextTokenRule[] {
            new BlockRule<TextTokenType, char>(ETextToken.Comment, "//", "\n"),
            new BlockRule<TextTokenType, char>(ETextToken.Comment, "/*", "*/"),
            new NumericRule()
        };
    };
}
