using MetaParser.Parsing.Tokens;

using System.Buffers;

namespace MetaParser.Parsing.Rules
{
    public class SingleRule<Ty> : TokenRule<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public Ty Value { get; init; }

        public SingleRule(Ty value, TokenFactory<Ty> tokenFactory) : base(tokenFactory)
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
            if (rd.IsNext(Value, true))
            {
                return Tokenizer.Consume(rd);
            }

            return null;
        }
    }
}
