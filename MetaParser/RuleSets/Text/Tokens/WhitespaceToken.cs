using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record WhitespaceToken : ValueToken
    {
        public WhitespaceToken(ReadOnlySequence<char> value) : base(ETextToken.Whitespace, value)
        {
        }

        public WhitespaceToken(ReadOnlyMemory<char> value) : base(ETextToken.Whitespace, value)
        {
        }
    }
}
