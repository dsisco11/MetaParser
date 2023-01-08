using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public record EOFToken<T> : IToken<T> where T : unmanaged, IEquatable<T>
    {
        public ReadOnlySequence<T> Value
        {
            get => ReadOnlySequence<T>.Empty;
        }

        public bool Equals(IToken<T>? other)
        {
            return (other is EOFToken<T>);
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
