using MetaParser.RuleSets.Text.Tokens;
using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;

namespace MetaParser.RuleSets.Text.Rules
{
    public sealed class WordBlockRule : ITokenRule<char>
    {
        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return char.IsLetter(Tokenizer.Peek(0)) && char.IsLetterOrDigit(Tokenizer.Peek(1));
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            var rd = Tokenizer.Get_Reader();
            do
            {
                if(!rd.TryPeek(out var ch))
                    break;

                if (!char.IsLetterOrDigit(ch))
                    break;

                rd.Advance(1);
            }
            while (!rd.End);

            return rd.Consumed <= 0 ? null : (IToken<char>)new IdentToken(Tokenizer.Consume(ref rd));
        }
    }
}
