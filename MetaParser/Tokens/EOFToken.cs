using System.Buffers;

namespace MetaParser.Tokens
{
    public record EOFToken<T> : IToken<T> where T : unmanaged, IEquatable<T>
    {
        public T[] Value
        {
            get => Array.Empty<T>();
        }

        public bool Equals(IToken<T>? other)
        {
            return other is EOFToken<T>;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
