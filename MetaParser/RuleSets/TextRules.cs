using MetaParser.Rules;
using MetaParser.RuleSets.Text;
using MetaParser.Tokens.Text;

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
        public static RuleSet<char> Common => new(
            new AutoRule<char>(new NewlineToken()),
            new WhitespaceRule(includeNewline: false),
            new WordBlockRule()
        );
        /// <summary>
        /// This ruleset captures sequences of basic ASCII whitespace characters such as: \s(space), \n, \r, \t, \f
        /// <note>The newline(\n) character is output as its own token type</note>
        /// </summary>
        public static RuleSet<char> Whitespace => new(
            new AutoRule<char>(new NewlineToken()),
            new WhitespaceRule(includeNewline: false)
        );

        /// <summary>
        /// This ruleset contains tokens which capture sequences of legible 'word' characters, so any sequence of human-readable characters which are not whitespace or control characters
        /// </summary>
        public static RuleSet<char> Words => new(
            new WordBlockRule()
        );

        /// <summary>
        /// Common symbols of importance for things like programming, markup, and configuration files.
        /// <para>: | ; , [ ] ( ) { } \< \> - = +</para>
        /// </summary>
        public static RuleSet<char> CodeLikeSymbols => new(
            new AutoRule<char>(new ColonToken()),
            new AutoRule<char>(new ColumnToken()),
            new AutoRule<char>(new SemicolonToken()),
            new AutoRule<char>(new CommaToken()),
            new AutoRule<char>(new SqBracketOpenToken()),
            new AutoRule<char>(new SqBracketCloseToken()),
            new AutoRule<char>(new ParenthOpenToken()),
            new AutoRule<char>(new ParenthCloseToken()),
            new AutoRule<char>(new BracketOpenToken()),
            new AutoRule<char>(new BracketCloseToken()),
            new AutoRule<char>(new LessThanToken()),
            new AutoRule<char>(new GreaterThanToken()),
            new AutoRule<char>(new HypenMinusToken()),
            new AutoRule<char>(new EqualsToken()),
            new AutoRule<char>(new PlusToken())
        );

        /// <summary>
        /// Code 'Objects' are more complex structures like comment blocks, function names, variable declarations, etc.
        /// </summary>
        public static RuleSet<char> CodeStructures => new(
            new CodeLineCommentRule("//"),
            new CodeBlockCommentRule("/*", "*/"),
            new NumericRule()
        );
    };
}
