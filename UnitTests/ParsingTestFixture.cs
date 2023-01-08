using MetaParser;
using MetaParser.Parsing;
using MetaParser.Parsing.Tokens;
using MetaParser.RuleSets.Text.Tokens;

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

        public void AssertTokenTypes(ReadOnlyMemory<Ty> input, params Type[] expected)
        {
            var results = parser.Parse(input);
            var types = results.Select(o => o.GetType());
            Assert.Equal(expected, types);
        }

        public void AssertTokenTypes(ReadOnlyMemory<Ty> input, params ETextToken[] expected)
        {
            var results = parser.Parse(input);
            var types = results.Select(o => ((TextToken)o).Type);
            Assert.Equal(expected, types);
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
