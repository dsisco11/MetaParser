using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;

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
            return Try_Consume(Tokenizer, Previous, out var consumed) && consumed is not null
                ? (TokenFactory?.Invoke(consumed.Value) ?? new Token<Ty>(consumed.Value)) : null;
        }

        protected abstract bool Try_Consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous, out ReadOnlySequence<Ty>? outConsumed);
    }
}
