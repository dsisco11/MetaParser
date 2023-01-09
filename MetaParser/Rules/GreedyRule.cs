using MetaParser.Tokens;

using System.Buffers;
using System.Runtime.CompilerServices;

namespace MetaParser.Rules
{
    public sealed class GreedyRule<T> : TokenRule<T> where T : IEquatable<T>
    {
        public T Value { get; init; }

        public GreedyRule(T value, TokenFactory<T>? tokenFactory = null) : base(tokenFactory)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T> Consumed)
        {
            var rd = Tokenizer.GetReader();
            rd.AdvancePast(Value);
            return Tokenizer.TryConsume(rd, out Consumed);
        }
    }
}
