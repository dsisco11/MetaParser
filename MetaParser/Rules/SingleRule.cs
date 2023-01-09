using MetaParser.Tokens;

using System.Buffers;
using System.Runtime.CompilerServices;

namespace MetaParser.Rules
{
    public sealed class SingleRule<T> : TokenRule<T> where T : IEquatable<T>
    {
        public T Value { get; init; }

        public SingleRule(T value, TokenFactory<T> tokenFactory) : base(tokenFactory)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T> Consumed)
        {
            var rd = Tokenizer.GetReader();
            rd.IsNext(Value, true);
            return Tokenizer.TryConsume(rd, out Consumed);
        }
    }
}
