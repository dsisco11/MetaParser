using System.Buffers;

namespace MetaParser.Tokens
{
    /// <summary>
    /// Represents any single item which is not consumed by another token
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed record Token<T> : IToken<T> where T : unmanaged, IEquatable<T>
    {
        private readonly ReadOnlySequence<T> _value;
        public ReadOnlySequence<T> Value => _value;

        public Token(T value)
        {
            _value = new ReadOnlySequence<T>(new[] { value });
        }

        public Token(ReadOnlySequence<T> value)
        {
            _value = value;
        }

        // Token-on-token equality checking theoretically is usually only going to be done in scenarios where the desire is to know if the tokens point to the same memory chunk.
        // Assumedly, the only useful scenarios wherein you would want to do strict equality checks between tokens with the desire to know if they contain equal values, would be in cases such as unit testing.
        // So doing something tragic like... calling toString and comparing the values... is... fine?
        public bool Equals(IToken<T>? other)
        {
            return ReferenceEquals(this, other)
                   || Value.Start.Equals(other?.Value.Start)
                   || ToString().Equals(other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
