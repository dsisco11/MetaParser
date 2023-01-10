using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.RuleSets.Text;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace UnitTests
{
    public class NumericTokenTests
    {
        public ParsingTestFixture<TokenType<ETextToken>, char> TestFixture { get; init; }
        public NumericTokenTests()
        {
            TestFixture = new ParsingTestFixture<TokenType<ETextToken>, char>(new RuleSet<TokenType<ETextToken>, char>(new NumericRule()));
        }

        [Theory]
        [InlineData("0", typeof(IntegerToken))]
        [InlineData("0123456789", typeof(IntegerToken))]
        [InlineData("-0123456789", typeof(IntegerToken))]
        [InlineData("+0123456789", typeof(IntegerToken))]
        [InlineData("0.0", typeof(DecimalToken))]
        [InlineData(".0", typeof(DecimalToken))]
        [InlineData("-.0", typeof(DecimalToken))]
        [InlineData("+.0", typeof(DecimalToken))]
        [InlineData("-0.0", typeof(DecimalToken))]
        [InlineData("+0.0", typeof(DecimalToken))]
        [InlineData(".1E10", typeof(DecimalToken))]
        [InlineData("-.1E10", typeof(DecimalToken))]
        [InlineData("+.1E10", typeof(DecimalToken))]
        [InlineData("5.5E10", typeof(DecimalToken))]
        [InlineData("-5.5E10", typeof(DecimalToken))]
        [InlineData("+5.5E10", typeof(DecimalToken))]
        public void Token(string text, Type Token)
        {
            TestFixture.AssertTokenTypes(text.AsMemory(), Token);
        }
    }
}