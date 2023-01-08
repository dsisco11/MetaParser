using MetaParser.Parsing;
using MetaParser.RuleSets;
using MetaParser.RuleSets.Text.Tokens;

namespace UnitTests
{
    public class GeneralTokenTests
    {
        public ParsingTestFixture<char> TestFixture { get; init; }
        public GeneralTokenTests()
        {
            TestFixture = new ParsingTestFixture<char>(TextRules.Whitespace, TextRules.Words, TextRules.CodeLikeSymbols, TextRules.CodeStructures);
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
        [InlineData("\n   ", ETextToken.Newline, ETextToken.Whitespace)]
        // Word blocks
        [InlineData("hello", ETextToken.Ident)]
        // code structures
        [InlineData("//hello world\n   //foo bar", ETextToken.Comment, ETextToken.Newline, ETextToken.Whitespace, ETextToken.Comment)]
        [InlineData("/*hello world*/", ETextToken.Comment)]
        public void TokenSequence(string text, params ETextToken[] TokenSequence)
        {
            TestFixture.AssertTokenTypes(text.AsMemory(), TokenSequence);
        }
    }
}