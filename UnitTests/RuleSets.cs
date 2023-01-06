using MetaParser.RuleSets;
using MetaParser.RuleSets.Text.Tokens;

namespace UnitTests
{
    public class RuleSets
    {
        public ParsingTestFixture<char> TestFixture { get; init; }
        public RuleSets()
        {
            TestFixture = new ParsingTestFixture<char>(TextRules.Whitespace, TextRules.CodeSymbols, TextRules.WordBlocks);
        }

        [Theory]
        [InlineData("\n", typeof(NewlineToken))]
        [InlineData(" ", typeof(WhitespaceToken))]
        [InlineData("\r", typeof(WhitespaceToken))]
        [InlineData("\f", typeof(WhitespaceToken))]
        [InlineData("\t", typeof(WhitespaceToken))]
        // Word blocks
        [InlineData("hello", typeof(IdentToken))]
        // Code symbols
        [InlineData(":", typeof(ColonToken))]
        [InlineData("|", typeof(ColumnToken))]
        [InlineData(";", typeof(SemicolonToken))]
        [InlineData(",", typeof(CommaToken))]
        [InlineData("[", typeof(SqBracketOpenToken))]
        [InlineData("]", typeof(SqBracketCloseToken))]
        [InlineData("(", typeof(ParenthOpenToken))]
        [InlineData(")", typeof(ParenthCloseToken))]
        [InlineData("{", typeof(BracketOpenToken))]
        [InlineData("}", typeof(BracketCloseToken))]
        [InlineData("<", typeof(LessThanToken))]
        [InlineData(">", typeof(GreaterThanToken))]
        [InlineData("-", typeof(HypenMinusToken))]
        [InlineData("=", typeof(EqualsToken))]
        [InlineData("+", typeof(PlusToken))]
        public void TokenParsing(string text, Type Token)
        {
            TestFixture.AssertTokenType(text.AsMemory(), Token);
        }
    }
}