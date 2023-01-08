using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
{
    public sealed record IdentToken : ValueToken
    {
        public IdentToken(ReadOnlySequence<char> value) : base(ETextToken.Ident, value)
        {
        }

        public IdentToken(ReadOnlyMemory<char> value) : base(ETextToken.Ident, value)
        {
        }

        public IdentToken(char[] value) : base(ETextToken.Ident, value)
        {
        }
    }
}
