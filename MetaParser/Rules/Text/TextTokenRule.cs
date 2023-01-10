using MetaParser.Parsing;
using MetaParser.Tokens;

namespace MetaParser.Rules.Text
{
    public abstract class TextTokenRule : ITokenRule<TokenType<ETextToken>, char>
    {
        public RuleSpecificty Specificity => new RuleSpecificty(true, false);

        public abstract bool TryConsume(ITokenizer<char> Tokenizer, Token<TokenType<ETextToken>, char>? Previous, out Token<TokenType<ETextToken>, char>? Token);
    }
}
