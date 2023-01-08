using MetaParser.Tokens;

using System.Buffers;

namespace MetaParser.Rules
{
    public abstract class TokenRule<T> : ITokenRule<T> where T : unmanaged, IEquatable<T>
    {
        public TokenFactory<T>? TokenFactory { get; init; }

        public TokenRule(TokenFactory<T>? tokenFactory = null)
        {
            TokenFactory = tokenFactory;
        }

        public abstract bool Check(IReadOnlyTokenizer<T> Tokenizer, IToken<T> Previous);

        public IToken<T>? Consume(ITokenizer<T> Tokenizer, IToken<T> Previous)
        {
            return TryConsume(Tokenizer, Previous, out var consumed) && consumed is not null
                ? TokenFactory?.Invoke(consumed.Value) ?? new Token<T>(consumed.Value) : null;
        }

        protected abstract bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T>? outConsumed);
    }
}
