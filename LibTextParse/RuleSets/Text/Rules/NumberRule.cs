using MetaParser.Parsing.Definitions;
using MetaParser.Parsing.Tokens;

namespace MetaParser.RuleSets.Text.Rules
{
    // TODO:
    /// <summary>
    /// Causes number-like (integer/decimal) sequences to be emitted as a single number-type token
    /// </summary>
    public sealed class NumberRule : ITokenRule<char>
    {
        public bool Check(IReadOnlyTokenizer<char> Tokenizer, IToken<char> Previous)
        {
            throw new NotImplementedException();
        }

        public IToken<char>? Consume(ITokenizer<char> Tokenizer, IToken<char> Previous)
        {
            throw new NotImplementedException();
        }
    }
}
