using MetaParser.Rules;
using MetaParser.RuleSets.Text;
using MetaParser.Tokens.Text.Static;

namespace MetaParser.RuleSets
{
    public static class TextRules
    {
        /// <summary>
        /// This ruleset captures newlines, sequences of whitespace, and words.
        /// <note>Newline: \n</note>
        /// <note>Whitespace: \s(space), \r, \t, \f</note>
        /// <note>Words: sequences of 2 or more letters/digits where the first character is a letter and not a digit</note>
        /// </summary>
        public static ITokenRule<char>[] Common => new ITokenRule<char>[] {
            new AutoRule<char>(NewlineToken.Instance),
            new WhitespaceRule(includeNewline: false),
            new WordBlockRule()
        };
        /// <summary>
        /// This ruleset captures sequences of basic ASCII whitespace characters such as: \s(space), \n, \r, \t, \f
        /// <note>The newline(\n) character is output as its own token type</note>
        /// </summary>
        public static ITokenRule<char>[] Whitespace => new ITokenRule<char>[] {
            new AutoRule<char>(NewlineToken.Instance),
            new WhitespaceRule(includeNewline: false)
        };

        /// <summary>
        /// This ruleset contains tokens which capture sequences of legible 'word' characters, so any sequence of human-readable characters which are not whitespace or control characters
        /// </summary>
        public static ITokenRule<char>[] Words => new ITokenRule<char>[] {
            new WordBlockRule()
        };

        /// <summary>
        /// Common symbols of importance for things like programming, markup, and configuration files.
        /// <para>: | ; , [ ] ( ) { } \< \> - = +</para>
        /// </summary>
        public static ITokenRule<char>[] CodeLikeSymbols => new ITokenRule<char>[] {
            new AutoRule<char>(ColonToken.Instance),
            new AutoRule<char>(ColumnToken.Instance),
            new AutoRule<char>(SemicolonToken.Instance),
            new AutoRule<char>(CommaToken.Instance),
            new AutoRule<char>(SqBracketOpenToken.Instance),
            new AutoRule<char>(SqBracketCloseToken.Instance),
            new AutoRule<char>(ParenthOpenToken.Instance),
            new AutoRule<char>(ParenthCloseToken.Instance),
            new AutoRule<char>(BracketOpenToken.Instance),
            new AutoRule<char>(BracketCloseToken.Instance),
            new AutoRule<char>(LessThanToken.Instance),
            new AutoRule<char>(GreaterThanToken.Instance),
            new AutoRule<char>(HypenMinusToken.Instance),
            new AutoRule<char>(EqualsToken.Instance),
            new AutoRule<char>(PlusToken.Instance)
        };

        /// <summary>
        /// Code 'Objects' are more complex structures like comment blocks, function names, variable declarations, etc.
        /// </summary>
        public static ITokenRule<char>[] CodeStructures => new ITokenRule<char>[] {
            new CodeLineCommentRule("//"),
            new CodeBlockCommentRule("/*", "*/"),
            new NumericRule()
        };
    };
}
