using MetaParser;
using MetaParser.Parsing;
using MetaParser.Parsing.Tokens;

namespace UnitTests
{
    public class ParsingTestFixture<Ty> : IDisposable 
        where Ty : unmanaged, IEquatable<Ty>
    {
        protected readonly Parser<Ty> parser;

        #region Constructors

        public ParsingTestFixture(ParsingConfig<Ty> config)
        {
            parser = new(config);
        }

        public ParsingTestFixture(params TokenRuleSet<Ty>[] rules)
        {
            parser = new(new ParsingConfig<Ty>(rules));
        }
        #endregion

        public void AssertTokenType(ReadOnlyMemory<Ty> input, Type TokenType)
        {
            var results = parser.Parse(input).Single();
            Assert.IsType(TokenType, results);
        }

        public void AssertTokens(ReadOnlyMemory<Ty> input, IToken<Ty>[] expected)
        {
            var results = parser.Parse(input);
            Assert.Equal(expected, results);
        }

        public void Dispose()
        {
            parser.Dispose();
        }
    }
}
