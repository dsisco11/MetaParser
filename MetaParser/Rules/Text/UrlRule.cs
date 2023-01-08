using MetaParser.Rules;
using MetaParser.Tokens;

namespace MetaParser.RuleSets.Text
{
    // TODO:
    /// <summary>
    /// Causes url-like sequences to be emitted as a singe url-type token
    /// </summary>
    public sealed class UrlRule : ITokenRule<char>
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
