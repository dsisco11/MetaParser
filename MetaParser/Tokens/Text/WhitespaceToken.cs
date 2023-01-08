using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
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
