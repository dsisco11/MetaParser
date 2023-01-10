using System.Buffers;

namespace MetaParser.Tokens
{
    public record EnumToken<TEnum, TValue> : Token<TokenType<TEnum>, TValue>
        where TEnum : struct, Enum
        where TValue : unmanaged, IEquatable<TValue>
    {
        public EnumToken(TokenType<TEnum> info) : base(info)
        {
        }

        public EnumToken(TokenType<TEnum> info, ReadOnlyMemory<TValue> data) : base(info, data)
        {
        }

        public EnumToken(TokenType<TEnum> info, ReadOnlySequence<TValue> data) : base(info, data)
        {
        }

        protected EnumToken(Token<TokenType<TEnum>, TValue> original) : base(original)
        {
        }
    }
}
