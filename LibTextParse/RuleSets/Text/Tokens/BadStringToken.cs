using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record BadStringToken : ValueToken
    {
        public BadStringToken(ReadOnlySequence<char> value) : base(ETextToken.Bad_String, value)
        {
        }

        public BadStringToken(ReadOnlyMemory<char> value) : base(ETextToken.Bad_String, value)
        {
        }

        public BadStringToken(char[] value) : base(ETextToken.Bad_String, value)
        {
        }
    }
}
