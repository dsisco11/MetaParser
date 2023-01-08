using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.RuleSets;

namespace UnitTests
{
    public class GeneralTokenTests
    {
        public ParsingTestFixture<char> TestFixture { get; init; }
        public GeneralTokenTests()
        {
            TestFixture = new ParsingTestFixture<char>(new RuleSet<char>(TextRules.Whitespace, TextRules.Words, TextRules.CodeLikeSymbols, TextRules.CodeStructures));
        }

        [Theory]
        [InlineData("\n", ETextToken.Newline)]
        [InlineData(" ", ETextToken.Whitespace)]
        [InlineData("\r", ETextToken.Whitespace)]
        [InlineData("\f", ETextToken.Whitespace)]
        [InlineData("\t", ETextToken.Whitespace)]
        // Word blocks
        [InlineData("hello", ETextToken.Ident)]
        // Code symbols
        [InlineData(":", ETextToken.Colon)]
        [InlineData("|", ETextToken.Column)]
        [InlineData(";", ETextToken.Semicolon)]
        [InlineData(",", ETextToken.Comma)]
        [InlineData("[", ETextToken.SqBracketOpen)]
        [InlineData("]", ETextToken.SqBracketClose)]
        [InlineData("(", ETextToken.ParenthOpen)]
        [InlineData(")", ETextToken.ParenthClose)]
        [InlineData("{", ETextToken.BracketOpen)]
        [InlineData("}", ETextToken.BracketClose)]
        [InlineData("<", ETextToken.LessThan)]
        [InlineData(">", ETextToken.GreaterThan)]
        [InlineData("-", ETextToken.HypenMinus)]
        [InlineData("=", ETextToken.Equals)]
        [InlineData("+", ETextToken.Plus)]
        [InlineData("//hello world", ETextToken.Comment)]
        [InlineData("/*hello world*/", ETextToken.Comment)]
        public void Token(string text, ETextToken Token)
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