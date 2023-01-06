using MetaParser.RuleSets.Text.Tokens;
using MetaParser.Parsing.Definitions;
using MetaParser.Parsing;
using MetaParser.RuleSets.Text.Rules;

namespace MetaParser.RuleSets
{
    public static class TextRules
    {
        public static TokenRuleSet<char> Whitespace => new(
            new AutoRule<char>(new NewlineToken()),
            new WhitespaceRule(includeNewline: false)
        );

        public static TokenRuleSet<char> CodeSymbols => new(
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

        public static TokenRuleSet<char> WordBlocks => new(
            new WordBlockRule()
        );

        public static TokenRuleSet<char> CodeObjects => new(
            new CodeLineCommentRule("//"),
            new CodeBlockCommentRule("/*", "*/")
        );
    };
}
