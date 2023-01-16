using System.Buffers;

namespace MetaParser.Tokens
{
    public readonly record struct TokenValue<TEnum, TValue>(TEnum Type, ReadOnlySequence<TValue> Data)
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>;
}
