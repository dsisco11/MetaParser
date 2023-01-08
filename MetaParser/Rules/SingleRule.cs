using MetaParser.Tokens;

using System.Buffers;

namespace MetaParser.Rules
{
    public class SingleRule<T> : TokenRule<T> where T : unmanaged, IEquatable<T>
    {
        public T Value { get; init; }

        public SingleRule(T value, TokenFactory<T> tokenFactory) : base(tokenFactory)
        {
            Value = value;
        }

        protected override bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T>? outConsumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Value, true);
            outConsumed = success ? Tokenizer.Consume(ref rd) : null;
            return success;
        }
    }
}
