using System.Buffers;
using System.Diagnostics;

namespace MetaParser.Tokens
{
    /// <summary>
    /// Holds all information required to distinguish a tokens type.
    /// <note>Usually this will just be an enum of some sort.</note>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{Type.ToString(),nq}")]
    public record TypedToken<TEnum, TData>(TEnum Type, ReadOnlySequence<TData> Data) : Token<TData>(Data)
        where TEnum : struct, Enum
        where TData : unmanaged, IEquatable<TData>
    { }
    //public readonly struct TokenType<T> : IEquatable<TokenType<T>>
    //    where T : struct, Enum
    //{
    //    #region Properties
    //    public readonly T Type { get; init; }
    //    #endregion

    //    #region Constructors
    //    public TokenType(T type)
    //    {
    //        Type = type;
    //    }
    //    #endregion

    //    #region Equality
    //    public bool Equals(TokenType<T> other)
    //    {
    //        return base.Equals(other);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return obj is TokenType<T> && Equals((TokenType<T>)obj);
    //    }
    //    #endregion

    //    public static implicit operator TokenType<T>(T value) => new(value);
    //}

}
