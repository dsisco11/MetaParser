using MetaParser.Parsing.Tokens;

using System.Buffers;

namespace MetaParser.Parsing.Rules
{
    public class GreedyRule<Ty> : TokenRule<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public Ty Value { get; init; }

        public GreedyRule(Ty value, TokenFactory<Ty>? tokenFactory = null) : base(tokenFactory)
        {
            Value = value;
        }

        public override bool Check(IReadOnlyTokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            return Tokenizer.Get_Reader().IsNext(Value);
        }

        protected override ReadOnlySequence<Ty>? consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            var rd = Tokenizer.Get_Reader();
            if (rd.AdvancePast(Value) > 0)
            {
                return Tokenizer.Consume(rd);
            }

            return null;
        }
    }
}
