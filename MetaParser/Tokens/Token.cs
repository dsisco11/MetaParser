using System.Buffers;

namespace MetaParser.Tokens
{

    /// <summary>
    /// Represents any single item which is not consumed by another token, the 'default' token
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public record Token<TValue>(ReadOnlySequence<TValue>? Value)
        where TValue : unmanaged, IEquatable<TValue>
    { }
    //{
    //    #region Fields
    //    private readonly ReadOnlySequence<TValue>? _value;
    //    #endregion

    //    #region Properties
    //    public TValue[] Value => _value.ToArray();
    //    #endregion

    //    #region Constructors
    //    public Token(ReadOnlySequence<TValue>? data)
    //    {
    //        _value = data;
    //    }
    //    #endregion

    //    // Token-on-token equality checking theoretically is usually only going to be done in scenarios where the desire is to know if the tokens point to the same memory chunk.
    //    // Assumedly, the only useful scenarios wherein you would want to do strict equality checks between tokens with the desire to know if they contain equal values, would be in cases such as unit testing.
    //    // So doing something tragic like... calling toString and comparing the values... is... fine?
    //    public virtual bool Equals(Token<TValue>? other)
    //    {
    //        return ReferenceEquals(this, other)
    //               || ((other is Token<TValue> tok) && _value.Start.Equals(tok?._value.Start))
    //               || ToString().Equals(other?.ToString(), StringComparison.Ordinal);
    //    }

    //    public override string ToString()
    //    {
    //        return Value.ToString();
    //    }
    //}
}
