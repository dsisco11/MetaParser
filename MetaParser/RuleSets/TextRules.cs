using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.RuleSets.Text;
using static MetaParser.UnicodeCommon;


namespace MetaParser.RuleSets
{
    using TextTokenRule = ITokenRule<byte, char>;
    using TSingleRule = SingleRule<byte, char>;
    using TBlockRule = BlockRule<byte, char>;
    using TGroupSetRule = GroupSetRule<byte, char>;
    using TGroupSingleRule = GroupSingleRule<byte, char>;

    public static class TextRules
    {
        public static readonly TextTokenRule Newline = new TGroupSingleRule(ETextToken.Newline, CHAR_LINE_FEED);
        public static readonly TextTokenRule Whitespace = new TGroupSetRule(ETextToken.Whitespace, ASCII_WHITESPACE);
        public static readonly TextTokenRule Whitespace_Except_Newline = new TGroupSetRule(ETextToken.Whitespace, ASCII_WHITESPACE_EXCLUDING_NEWLINE);

        /// <summary>
        /// This rule captures sequences of legible 'word' characters, so any sequence of human-readable characters which are not whitespace or control characters
        /// </summary>
        public static readonly TextTokenRule Words = new PredicateRule<byte, char>(ETextToken.Ident, char.IsLetterOrDigit, char.IsLetter);

        /// <summary>
        /// Common symbols of importance for things like programming, markup, and configuration files.
        /// <para>: | ; , [ ] ( ) { } \< \> - = +</para>
        /// </summary>
        public static ITokenRule<byte,char>[] CommonSymbols => new [] {
            new TSingleRule(ETextToken.Colon, CHAR_COLON),
            new TSingleRule(ETextToken.Column, CHAR_PIPE),
            new TSingleRule(ETextToken.Semicolon, CHAR_SEMICOLON),
            new TSingleRule(ETextToken.Comma, CHAR_COMMA),
            new TSingleRule(ETextToken.SqBracketOpen, CHAR_LEFT_SQUARE_BRACKET),
            new TSingleRule(ETextToken.SqBracketClose, CHAR_RIGHT_SQUARE_BRACKET),
            new TSingleRule(ETextToken.ParenthOpen, CHAR_LEFT_PARENTHESES),
            new TSingleRule(ETextToken.ParenthClose, CHAR_RIGHT_PARENTHESES),
            new TSingleRule(ETextToken.BracketOpen, CHAR_LEFT_CURLY_BRACKET),
            new TSingleRule(ETextToken.BracketClose, CHAR_RIGHT_CURLY_BRACKET),
            new TSingleRule(ETextToken.LessThan, CHAR_LEFT_CHEVRON),
            new TSingleRule(ETextToken.GreaterThan, CHAR_RIGHT_CHEVRON),
            new TSingleRule(ETextToken.HypenMinus, CHAR_HYPHEN_MINUS),
            new TSingleRule(ETextToken.Equals, CHAR_EQUALS),
            new TSingleRule(ETextToken.Plus, CHAR_PLUS_SIGN),
            new TSingleRule(ETextToken.Asterisk, CHAR_ASTERISK),
            new TSingleRule(ETextToken.Solidus, CHAR_SOLIDUS),
            new TSingleRule(ETextToken.ReverseSolidus, CHAR_REVERSE_SOLIDUS),
        };

        /// <summary>
        /// Code 'Objects' are more complex structures like comment blocks, function names, variable declarations, etc.
        /// </summary>
        public static TextTokenRule[] CodeStructures => new TextTokenRule[] {
            new TBlockRule(ETextToken.Comment, ETextToken.Bad_Comment, "//", "\n"),
            new TBlockRule(ETextToken.Comment, ETextToken.Bad_Comment, "/*", "*/"),
            new TBlockRule(ETextToken.String, ETextToken.Bad_String, CHAR_APOSTRAPHE, CHAR_APOSTRAPHE, CHAR_REVERSE_SOLIDUS),
            new TBlockRule(ETextToken.String, ETextToken.Bad_String, CHAR_QUOTATION_MARK, CHAR_QUOTATION_MARK, CHAR_REVERSE_SOLIDUS),
            new NumericRule(),
        };
    };
}
