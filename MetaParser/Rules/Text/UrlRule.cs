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
        public bool TryConsume(ITokenReader<char> Tokenizer, IToken<char> Previous, out IToken<char>? outToken)
        {
            throw new NotImplementedException();
        }
    }
}
