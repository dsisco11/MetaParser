using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.RuleSets;
using MetaParser.Tokens;

namespace UnitTests
{
    public class GeneralTokenTests
    {
        public ParsingTestFixture<TokenType<ETextToken>, char> TestFixture { get; init; }
        public GeneralTokenTests()
        {
            TestFixture = new ParsingTestFixture<TokenType<ETextToken>, char>(new RuleSet<TokenType<ETextToken>, char>(
                new[] { TextRules.Newline, TextRules.Whitespace_Except_Newline, TextRules.Words },
                TextRules.CommonSymbols,
                TextRules.CodeStructures));
        }

        [Theory]
        [InlineData("\n", ETextToken.Newline)]
        [InlineData(" ", ETextToken.Whitespace)]
        [InlineData("\r", ETextToken.Whitespace)]
        [InlineData("\f", ETextToken.Whitespace)]
        [InlineData("\t", ETextToken.Whitespace)]
        // Word Sequences
        [InlineData("hello", ETextToken.Ident)]
        // Blocks
        [InlineData("//hello world", ETextToken.Comment)]
        [InlineData("/*hello world*/", ETextToken.Comment)]
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
        [InlineData("*", ETextToken.Asterisk)]
        [InlineData("/", ETextToken.Solidus)]
        [InlineData("\\", ETextToken.ReverseSolidus)]
        public void Token(string text, ETextToken Type)
        {
            TestFixture.AssertTokenTypes(text.AsMemory(), Type);
        }

        [Theory]
        [InlineData("\n   ", ETextToken.Newline, ETextToken.Whitespace)]
        // Word blocks
        [InlineData("hello", ETextToken.Ident)]
        // code structures
        [InlineData("//hello world\n*   //foo bar", ETextToken.Comment, ETextToken.Asterisk, ETextToken.Whitespace, ETextToken.Comment)]
        [InlineData("/*hello world*/", ETextToken.Comment)]
        public void TokenSequence(string text, params ETextToken[] TokenSequence)
        {
            var seq = TokenSequence.Select(o => (TokenType<ETextToken>)o).ToArray();
            TestFixture.AssertTokenTypes(text.AsMemory(), seq);
        }
    }
}