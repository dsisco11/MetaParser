using MetaParser.Parsing;
using MetaParser.Parsing.Tokens;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public abstract record TextToken : IToken<char>
    {
        public ETextToken Type { get; init; }
        public abstract ReadOnlySequence<char> Value { get; }

        protected TextToken(ETextToken type)
        {
            Type = type;
        }

        // Token-on-token equality checking theoretically is usually only going to be done in scenarios where the desire is to know if the tokens point to the same memory chunk.
        // Assumedly, the only useful scenarios wherein you would want to do strict equality checks between tokens with the desire to know if they contain equal values, would be in cases such as unit testing.
        // So doing something tragic like... calling toString and comparing the values... is... fine?
        public bool Equals(IToken<char>? other)
        {
            return ReferenceEquals(this, other)
|| Value.Start.Equals(other?.Value.Start) || ToString().Equals(other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
