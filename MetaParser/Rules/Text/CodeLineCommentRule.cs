using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    /// <summary>
    /// Causes url-like sequences to be emitted as a singe url-type token
    /// </summary>
    public sealed class CodeLineCommentRule : ITokenRule<char>
    {
        private char[] CommentPrefix { get; init; }

        public CodeLineCommentRule(ReadOnlySpan<char> commentPrefix)
        {
            CommentPrefix = commentPrefix.ToArray();
        }

        public bool TryConsume(ITokenReader<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            var rd = Tokenizer.GetReader();
            if (!rd.IsNext(CommentPrefix, advancePast: true))
            {
                outToken = null;
                return false;
            }

            // Consume until line end
            if (!rd.TryAdvanceTo(UnicodeCommon.CHAR_LINE_FEED, advancePastDelimiter: false))
            {
                // If no newline then consume everything thats left
                rd.AdvanceToEnd();
            }

            var consume = Tokenizer.Consume(ref rd);
            outToken = new CommentToken(consume);
            return true;
        }
    }
}
