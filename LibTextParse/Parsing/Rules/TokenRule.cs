using MetaParser.Parsing.Tokens;
using MetaParser.Parsing.Definitions;

using System.Buffers;

namespace MetaParser.Parsing.Rules
{
    public abstract class TokenRule<Ty> : ITokenRule<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public TokenFactory<Ty>? TokenFactory { get; init; }

        public TokenRule(TokenFactory<Ty>? tokenFactory = null)
        {
            TokenFactory = tokenFactory;
        }

        public abstract bool Check(IReadOnlyTokenizer<Ty> Tokenizer, IToken<Ty> Previous);

        public IToken<Ty>? Consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous)
        {
            var consumed = consume(Tokenizer, Previous);
            if (consumed is not null)
            {
                return TokenFactory?.Invoke(consumed.Value) ?? new Token<Ty>(consumed.Value);
            }

            return null;
        }

        protected abstract ReadOnlySequence<Ty>? consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous);
    }
}
