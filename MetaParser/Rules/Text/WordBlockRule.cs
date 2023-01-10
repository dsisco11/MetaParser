using MetaParser.Parsing;
using MetaParser.Rules.Text;
using MetaParser.Tokens;
using MetaParser.Tokens.Text;

namespace MetaParser.RuleSets.Text
{
    public sealed class WordBlockRule : TextTokenRule
    {
        public override bool TryConsume(ITokenizer<char> Tokenizer, Token<TokenType<ETextToken>, char>? Previous, out Token<TokenType<ETextToken>, char>? Token)
        {
            bool valid = char.IsLetter(Tokenizer.Peek(0)) && char.IsLetterOrDigit(Tokenizer.Peek(1));
            if (!valid)
            {
                Token = null;
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

            Token = new TextToken(ETextToken.Ident, Tokenizer.Consume(ref rd));
            return true;
        }
    }
}
