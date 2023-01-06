using MetaParser.Parsing;

using System.Buffers;

namespace MetaParser.RuleSets.Text.Tokens
{
    public sealed record CommentToken : ValueToken
    {
        public CommentToken(ReadOnlySequence<char> value) : base(ETextToken.Comment, value)
        {
        }

        public CommentToken(ReadOnlyMemory<char> value) : base(ETextToken.Comment, value)
        {
        }

        public CommentToken(char[] value) : base(ETextToken.Comment, value)
        {
        }
    }
}
