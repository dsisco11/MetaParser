using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
{
    public abstract record ValueToken : TextToken
    {
        #region Properties
        private readonly ReadOnlySequence<char> _value;
        public override char[] Value => _value.ToArray();
        #endregion

        public ValueToken(ETextToken type, ReadOnlySequence<char> value) : base(type)
        {
            this._value = value;
        }

        public ValueToken(ETextToken type, ReadOnlyMemory<char> value) : base(type)
        {
            this._value = new(value);
        }

        public virtual bool Equals(ValueToken? other)
        {
            return ReferenceEquals(this, other)
                   || _value.Start.Equals(other!._value.Start)
                   || ToString().Equals(other?.ToString(), StringComparison.Ordinal);
        }
    }
}
