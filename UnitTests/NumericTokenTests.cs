using MetaParser.Parsing;
using MetaParser.RuleSets.Text;

namespace UnitTests
{
    public class NumericTokenTests
    {
        public ParsingTestFixture<byte, char> TestFixture { get; init; }
        public NumericTokenTests()
        {
            TestFixture = new ParsingTestFixture<byte, char>(new(new NumericRule()));
        }

        [Theory]
        [InlineData("0", ETextToken.Number)]
        [InlineData("0123456789", ETextToken.Number)]
        [InlineData("-0123456789", ETextToken.Number)]
        [InlineData("+0123456789", ETextToken.Number)]
        [InlineData("0.0", ETextToken.Number)]
        [InlineData(".0", ETextToken.Number)]
        [InlineData("-.0", ETextToken.Number)]
        [InlineData("+.0", ETextToken.Number)]
        [InlineData("-0.0", ETextToken.Number)]
        [InlineData("+0.0", ETextToken.Number)]
        [InlineData(".1E10", ETextToken.Number)]
        [InlineData("-.1E10", ETextToken.Number)]
        [InlineData("+.1E10", ETextToken.Number)]
        [InlineData("5.5E10", ETextToken.Number)]
        [InlineData("-5.5E10", ETextToken.Number)]
        [InlineData("+5.5E10", ETextToken.Number)]
        public void Token(string text, byte Token)
        {
            TestFixture.AssertTokenTypes(text.AsMemory(), Token);
        }
    }
}