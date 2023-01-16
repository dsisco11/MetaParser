using MetaParser.Tokens;

using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Rules
{
    //[DebuggerDisplay("TokenRule :: {Definition}")]
    //public abstract class TokenRule<TEnum, TValue> : ITokenRule<TEnum, TValue>
    //    where TEnum: unmanaged
    //    where TValue : unmanaged, IEquatable<TValue>
    //{
    //    public abstract RuleSpecificty Specificity { get; }        

    //    #region Properties
    //    public TEnum Definition { get; init; }
    //    #endregion

    //    #region Constructors
    //    protected TokenRule(TEnum definition)
    //    {
    //        Definition = definition;
    //    }
    //    #endregion

    //    protected abstract bool Consume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out ReadOnlySequence<TValue> Consumed);

    //    public bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out int TokenLength)
    //    {
    //        if (Consume(Tokenizer, Previous, out ReadOnlySequence<TValue> consumed))
    //        {
    //            Token = new TokenValue<TEnum, TValue>(Definition, consumed);
    //            return true;
    //        }

    //        Token = null;
    //        return false;                
    //    }

    //}
}
