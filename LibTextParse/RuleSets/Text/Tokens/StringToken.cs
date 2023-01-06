using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record StringToken : ValueToken
    {
        public StringToken(ReadOnlySequence<char> value) : base(ETextToken.String, value)
        {
        }

        public StringToken(ReadOnlyMemory<char> value) : base(ETextToken.String, value)
        {
        }

        public StringToken(char[] value) : base(ETextToken.String, value)
        {
        }
    }
}
