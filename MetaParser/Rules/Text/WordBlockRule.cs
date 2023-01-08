using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed class WordBlockRule : ITokenRule<char>
    {
        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            return char.IsLetter(Tokenizer.Peek(0)) && char.IsLetterOrDigit(Tokenizer.Peek(1));
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            var rd = Tokenizer.GetReader();
            do
            {
                if (!rd.TryPeek(out var ch))
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
