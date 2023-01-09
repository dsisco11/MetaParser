using MetaParser.Rules;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed class WordBlockRule : ITokenRule<char>
    {
        public bool TryConsume(ITokenReader<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            bool valid = char.IsLetter(Tokenizer.Peek(0)) && char.IsLetterOrDigit(Tokenizer.Peek(1));
            if (!valid)
            {
                outToken = null;
                return false;
            }

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

            outToken = new IdentToken(Tokenizer.Consume(ref rd));
            return true;
        }
    }
}
