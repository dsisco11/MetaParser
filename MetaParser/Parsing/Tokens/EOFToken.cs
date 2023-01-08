using System.Buffers;

namespace MetaParser.Parsing.Tokens
{
    public record EOFToken<Ty> : IToken<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public ReadOnlySequence<Ty> Value
        {
            get => ReadOnlySequence<Ty>.Empty;
        }

        public bool Equals(IToken<Ty>? other)
        {
            return (other is EOFToken<Ty>);
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
