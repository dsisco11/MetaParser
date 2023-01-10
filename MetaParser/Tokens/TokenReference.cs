namespace MetaParser.Tokens
{
    public readonly struct TokenReference<TToken, TData> : IEquatable<TokenReference<TToken, TData>>
        where TToken : struct, IEquatable<TToken>
        where TData : unmanaged, IEquatable<TData>
    {
        #region Fields
        public readonly long Index { get; init; }
        public readonly TToken Type { get; init; }
        #endregion

        public TokenReference(long index, TToken info)
        {
            Index = index;
            Type = info;
        }

        #region Equality

        public bool Equals(TokenReference<TToken, TData> other)
        {
            return Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            return obj is TokenReference<TToken, TData> && Equals((TokenReference<TToken, TData>)obj);
        }
        #endregion
    }
}
