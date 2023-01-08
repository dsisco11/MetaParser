using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;
using MetaParser.RuleSets.Text.Tokens;

namespace MetaParser.RuleSets.Text.Rules
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

        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return Tokenizer.GetReader().IsNext(CommentPrefix);
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            var rd = Tokenizer.GetReader();
            // Consume until line end
            if (!rd.TryAdvanceTo(UnicodeCommon.CHAR_LINE_FEED, advancePastDelimiter: false))
            {
                // If no newline then consume everything thats left
                rd.AdvanceToEnd();
            }

            var consume = Tokenizer.Consume(ref rd);
            return new CommentToken(consume);
        }
    }
}
