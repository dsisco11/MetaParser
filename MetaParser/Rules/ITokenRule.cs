using MetaParser.Tokens;

namespace MetaParser.Rules
{
    /// <summary>
    /// Provides logic for detecting and consuming tokens
    /// </summary>
    /// <typeparam name="T">The value type being parsed</typeparam>
    public interface ITokenRule<T> where T : IEquatable<T>
    {
        bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out IToken<T>? Token);
    }
}
