using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record UrlToken : ValueToken
    {
        public UrlToken(ReadOnlySequence<char> value) : base(ETextToken.Url, value)
        {
        }

        public UrlToken(ReadOnlyMemory<char> value) : base(ETextToken.Url, value)
        {
        }

        public UrlToken(char[] value) : base(ETextToken.Url, value)
        {
        }
    }
}
