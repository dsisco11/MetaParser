using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

using System.Buffers;

namespace MetaParser.RuleSets.Text
{
    /// <summary>
    /// Causes url-like sequences to be emitted as a singe url-type token
    /// </summary>
    public sealed class CodeBlockCommentRule : ITokenRule<char>
    {
        private char[] BlockStart { get; init; }
        private char[] BlockEnd { get; init; }


        public CodeBlockCommentRule(ReadOnlySpan<char> blockStart, ReadOnlySpan<char> blockEnd)
        {
            BlockStart = blockStart.ToArray();
            BlockEnd = blockEnd.ToArray();
        }

        public bool TryConsume(ITokenizer<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            var rd = Tokenizer.GetReader();
            if (!rd.IsNext(BlockStart, advancePast: true))
            {
                outToken = null;
                return false;
            }

            // Skip forward until we find the block end sequence or we reach the stream end
            if (!rd.TryReadTo(out ReadOnlySequence<char> _, BlockEnd, advancePastDelimiter: true))
            {// Consume everything thats left if we couldnt find it
                rd.AdvanceToEnd();
            }

            outToken = new CommentToken(Tokenizer.Consume(ref rd));
            return true;
        }
    }
}
