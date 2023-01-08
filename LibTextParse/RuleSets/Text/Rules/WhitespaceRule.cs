using MetaParser.RuleSets.Text.Tokens;
using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;

namespace MetaParser.RuleSets.Text.Rules
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

        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return Tokenizer.Get_Reader().AdvancePastAny(CharacterSet.Span) > 0;
            //return !IncludeNewline && Tokenizer.Next == UnicodeCommon.CHAR_LINE_FEED ? false : char.IsWhiteSpace(Tokenizer.Next);
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            var rd = Tokenizer.Get_Reader();
            var count = rd.AdvancePastAny(CharacterSet.Span);
            if (count > 0)
            {
                var consumed = Tokenizer.Consume(ref rd);
                return new WhitespaceToken(consumed);
            }

            return null;
        }
    }

}
