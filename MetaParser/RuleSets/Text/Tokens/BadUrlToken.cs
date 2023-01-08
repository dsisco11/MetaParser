using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record BadUrlToken : ValueToken
    {
        public BadUrlToken(ReadOnlySequence<char> value) : base(ETextToken.Bad_Url, value)
        {
        }

        public BadUrlToken(ReadOnlyMemory<char> value) : base(ETextToken.Bad_Url, value)
        {
        }

        public BadUrlToken(char[] value) : base(ETextToken.Bad_Url, value)
        {
        }
    }
}
