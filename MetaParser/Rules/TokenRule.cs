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

        protected abstract bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T>? outConsumed);

        public bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out IToken<T>? outToken)
        {
            if (Consume(Tokenizer, Previous, out ReadOnlySequence<T>? consumed) && consumed is not null)
            {
                outToken = TokenFactory?.Invoke(consumed.Value) ?? new Token<T>(consumed.Value);
                return true;
            }

            outToken = null;
            return false;                
        }

    }
}
