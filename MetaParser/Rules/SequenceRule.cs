using MetaParser.Tokens;

using System.Buffers;

namespace MetaParser.Rules
{
    public class SequenceRule<T> : TokenRule<T> where T : unmanaged, IEquatable<T>
    {
        public T[] Sequence { get; init; }

        public SequenceRule(T[] sequence, TokenFactory<T>? tokenFactory = null) : base(tokenFactory)
        {
            Sequence = sequence;
        }

        public override bool Check(IReadOnlyTokenizer<T> Tokenizer, IToken<T> Previous)
        {
            return Tokenizer.GetReader().IsNext(Sequence);
        }

        protected override bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T>? outConsumed)
        {
            var rd = Tokenizer.GetReader();
            bool success = rd.IsNext(Sequence, true);
            outConsumed = success ? Tokenizer.Consume(ref rd) : null;
            return success;
        }
    }
}
