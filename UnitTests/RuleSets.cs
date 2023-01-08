using MetaParser.RuleSets;
using MetaParser.RuleSets.Text.Tokens;

namespace UnitTests
{
    public class RuleSets
    {
        public ParsingTestFixture<char> TestFixture { get; init; }
        public RuleSets()
        {
            TestFixture = new ParsingTestFixture<char>(TextRules.Whitespace, TextRules.CodeLikeSymbols, TextRules.CodeStructures, TextRules.WordBlocks);
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
        [InlineData("//hello world", typeof(CommentToken))]
        [InlineData("/*hello world*/", typeof(CommentToken))]
        public void Token(string text, Type Token)
        {
            TestFixture.AssertTokenTypes(text.AsMemory(), Token);
        }

        [Theory]
        [InlineData("\n", typeof(NewlineToken), typeof(WhitespaceToken))]
        [InlineData(" ", typeof(WhitespaceToken), typeof(WhitespaceToken))]
        [InlineData("\r", typeof(WhitespaceToken), typeof(WhitespaceToken))]
        [InlineData("\f", typeof(WhitespaceToken), typeof(WhitespaceToken))]
        [InlineData("\t", typeof(WhitespaceToken), typeof(WhitespaceToken))]
        // Word blocks
        [InlineData("hello", typeof(IdentToken))]
        // code structures
        [InlineData("//hello world\n   //foo bar", typeof(CommentToken))]
        [InlineData("/*hello world*/", typeof(CommentToken))]
        public void TokenSequence(string text, params Type[] TokenSequence)
        {
            TestFixture.AssertTokenType(text.AsMemory(), Token);
        }
    }
}