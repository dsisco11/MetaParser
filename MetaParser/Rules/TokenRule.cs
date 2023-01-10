using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    [DebuggerDisplay("TokenRule :: {Definition}")]
    public abstract class TokenRule<TData, TValue> : ITokenRule<TData, TValue>
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        public abstract RuleSpecificty Specificity { get; }        

        #region Properties
        public TData Definition { get; init; }
        public Token<TData, TValue>? Instance { get; init; }
        #endregion

        #region Constructors
        protected TokenRule(TData definition)
        {
            Definition = definition;
        }
        protected TokenRule(Token<TData, TValue> instance)
        {
            Instance = instance;
        }
        #endregion

        protected abstract bool Consume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out ReadOnlySequence<TValue> Consumed);

        public bool TryConsume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out Token<TData, TValue>? Token)
        {
            if (Consume(Tokenizer, Previous, out ReadOnlySequence<TValue> consumed))
            {
                Token = Instance ?? new Token<TData, TValue>(Definition, consumed);
                return true;
            }

            Token = null;
            return false;                
        }

    }
}
