using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.Tokens.Text
{
    public record TextToken : EnumToken<ETextToken, char>
    {
        public TextToken(ETextToken type, ReadOnlySequence<char> data) : base(type, data)
        {
        }

        public TextToken(ETextToken type, ReadOnlyMemory<char> data) : base(type, data)
        {
        }

        public TextToken(ETextToken type, params char[] data) : base(type, data.ToArray())
        {
        }
    }
}
