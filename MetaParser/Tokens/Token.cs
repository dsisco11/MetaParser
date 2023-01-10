using System.Buffers;

namespace MetaParser.Tokens
{

    /// <summary>
    /// Represents any single item which is not consumed by another token, the 'default' token
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public record Token<TData, TValue> 
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        #region Fields
        private readonly TData _data;
        private readonly ReadOnlySequence<TValue> _value;
        #endregion

        #region Properties
        public TData Data => _data;
        public TValue[] Value => _value.ToArray();
        #endregion

        #region Constructors
        public Token(TData info)
        {
            _data = info;
            _value = ReadOnlySequence<TValue>.Empty;
        }

        public Token(TData info, ReadOnlyMemory<TValue> data)
        {
            _data = info;
            _value = new ReadOnlySequence<TValue>(data);
        }

        public Token(TData info, ReadOnlySequence<TValue> data)
        {
            _data = info;
            _value = data;
        }
        #endregion

        // Token-on-token equality checking theoretically is usually only going to be done in scenarios where the desire is to know if the tokens point to the same memory chunk.
        // Assumedly, the only useful scenarios wherein you would want to do strict equality checks between tokens with the desire to know if they contain equal values, would be in cases such as unit testing.
        // So doing something tragic like... calling toString and comparing the values... is... fine?
        public virtual bool Equals(Token<TData, TValue>? other)
        {
            return ReferenceEquals(this, other)
                   || ((other is Token<TData, TValue> tok) && _value.Start.Equals(tok?._value.Start))
                   || ToString().Equals(other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
