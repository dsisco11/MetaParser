using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed class WhitespaceRule : ITokenRule<char>
    {
        private bool IncludeNewline { get; init; }
        private readonly ReadOnlyMemory<char> CharacterSet;// => IncludeNewline ? UnicodeCommon.ASCII_WHITESPACE : UnicodeCommon.ASCII_WHITESPACE_EXCLUDING_NEWLINE;
        public WhitespaceRule(bool includeNewline = false)
        {
            IncludeNewline = includeNewline;
            CharacterSet = IncludeNewline ? UnicodeCommon.ASCII_WHITESPACE : UnicodeCommon.ASCII_WHITESPACE_EXCLUDING_NEWLINE;
        }

        public bool TryConsume(ITokenizer<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            var rd = Tokenizer.GetReader();
            var count = rd.AdvancePastAny(CharacterSet.Span);
            if (count <= 0)
            {
                outToken = null;
                return false;
            }

            var consumed = Tokenizer.Consume(ref rd);
            outToken = new WhitespaceToken(consumed);
            return true;
        }
    }

}
