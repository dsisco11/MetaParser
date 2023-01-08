using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record BadNumberToken : ValueToken
    {
        public BadNumberToken(ReadOnlySequence<char> value) : base(ETextToken.Bad_Number, value)
        {
        }

        public BadNumberToken(ReadOnlyMemory<char> value) : base(ETextToken.Bad_Number, value)
        {
        }

        public BadNumberToken(char[] value) : base(ETextToken.Bad_Number, value)
        {
        }
    }
}
