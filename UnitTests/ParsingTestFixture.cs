using MetaParser;
using MetaParser.Rules;

namespace UnitTests
{
    public class ParsingTestFixture<TEnum, TValue> : IDisposable
        where TEnum : unmanaged, IEquatable<TEnum>
        where TValue : unmanaged, IEquatable<TValue>
    {
        protected readonly Parser<TEnum, TValue> parser;

        #region Constructors
        public ParsingTestFixture(RuleSet<TEnum, TValue> rules)
        {
            parser = new Parser<TEnum, TValue>(rules);
        }
        #endregion

        public void AssertTokenTypes(ReadOnlyMemory<TValue> input, params Type[] expected)
        {
            var results = parser.Parse(input);
            var types = results.ToArray().Select(o => o.GetType());
            Assert.Equal(expected, types);
        }

        public void AssertTokenTypes(ReadOnlyMemory<TValue> input, params TEnum[] expected)
        {
            var results = parser.Parse(input);
            var types = results.ToArray().Select(o => o.Type);
            Assert.Equal(expected, types);
        }

        public void Dispose()
        {
            parser.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
