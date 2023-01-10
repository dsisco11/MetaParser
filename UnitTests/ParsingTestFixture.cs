using MetaParser;
using MetaParser.Parsing;
using MetaParser.Rules;
using MetaParser.Tokens;

namespace UnitTests
{
    public class ParsingTestFixture<TData, TValue> : IDisposable
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        protected readonly Parser<TData, TValue> parser;

        #region Constructors

        public ParsingTestFixture(ParsingConfig<TData, TValue> config)
        {
            parser = new(config);
        }

        public ParsingTestFixture(RuleSet<TData, TValue> rules)
        {
            parser = new(new ParsingConfig<TData, TValue>(rules));
        }
        #endregion

        public void AssertTokenTypes(ReadOnlyMemory<TValue> input, params Type[] expected)
        {
            var results = parser.Parse(input);
            var types = results.Select(o => o.GetType());
            Assert.Equal(expected, types);
        }

        public void AssertTokenTypes(ReadOnlyMemory<TValue> input, params TData[] expected)
        {
            var results = parser.Parse(input);
            var types = results.Select(o => o.Data);
            Assert.Equal(expected, types);
        }

        public void AssertTokens(ReadOnlyMemory<TValue> input, Token<TData, TValue>[] expected)
        {
            var results = parser.Parse(input);
            Assert.Equal(expected, results);
        }

        public void Dispose()
        {
            parser.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
