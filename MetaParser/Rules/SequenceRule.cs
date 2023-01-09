using MetaParser.Tokens;

using System.Buffers;
using System.Runtime.CompilerServices;

namespace MetaParser.Rules
{
    public sealed class SequenceRule<T> : TokenRule<T> where T : IEquatable<T>
    {
        public T[] Sequence { get; init; }

        public SequenceRule(T[] sequence, TokenFactory<T>? tokenFactory = null) : base(tokenFactory)
        {
            Sequence = sequence;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T> Consumed)
        {
            var rd = Tokenizer.GetReader();
            var success = rd.IsNext(Sequence, advancePast: true);
            if (Tokenizer.TryConsume(rd, out Consumed))
            {
                return true;
            }

            return success;
        }
    }
}
