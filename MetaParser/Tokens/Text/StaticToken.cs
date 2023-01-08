using MetaParser.Parsing;

namespace MetaParser.Tokens.Text
{
    public abstract record StaticToken : TextToken, IStaticToken<char>
    {
        #region Properties
        private readonly char[] _value;
        public override char[] Value => _value;

        public abstract IStaticToken<char> static_instance { get; }
        #endregion

        public StaticToken(ETextToken type, params char[] value) : base(type)
        {
            this._value = value;
        }

        public virtual bool Equals(StaticToken? other)
        {
            return ReferenceEquals(this, other);
        }
    }
}
