using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
{
    public abstract record ValueToken : TextToken
    {
        #region Properties
        private readonly ReadOnlySequence<char> value;
        public override ReadOnlySequence<char> Value => value;
        #endregion

        public ValueToken(ETextToken type, ReadOnlySequence<char> value) : base(type)
        {
            this.value = value;
        }

        public ValueToken(ETextToken type, ReadOnlyMemory<char> value) : base(type)
        {
            this.value = new(value);
        }
    }
}
