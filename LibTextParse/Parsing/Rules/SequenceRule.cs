using MetaParser.Parsing.Tokens;

using System.Buffers;

namespace MetaParser.Parsing.Rules
{
    public class SequenceRule<Ty> : TokenRule<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public Ty[] Sequence { get; init; }

        public SequenceRule(Ty[] sequence, TokenFactory<Ty>? tokenFactory = null) : base(tokenFactory)
        {
            Sequence = sequence;
        }

        public override bool Check(IReadOnlyTokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            return Tokenizer.Get_Reader().IsNext(Sequence);
        }

        protected override ReadOnlySequence<Ty>? consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            var rd = Tokenizer.Get_Reader();
            if (rd.IsNext(Sequence, true))
            {
                return Tokenizer.Consume(rd);
            }

            return null;
        }
    }
}
