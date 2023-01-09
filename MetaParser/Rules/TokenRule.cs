using MetaParser.Tokens;

using System.Buffers;
using System.Runtime.CompilerServices;

namespace MetaParser.Rules
{
    public abstract class TokenRule<T> : ITokenRule<T> where T : IEquatable<T>
    {
        public TokenFactory<T>? TokenFactory { get; init; }

        public TokenRule(TokenFactory<T>? tokenFactory = null)
        {
            TokenFactory = tokenFactory;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract bool Consume(ITokenizer<T> Tokenizer, IToken<T> Previous, out ReadOnlySequence<T> Consumed);

        public bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out IToken<T>? Token)
        {
            if (Consume(Tokenizer, Previous, out ReadOnlySequence<T> consumed))
            {
                Token = TokenFactory?.Invoke(consumed) ?? new Token<T>(consumed);
                return true;
            }

            Token = null;
            return false;                
        }

    }
}
