using MetaParser.Tokens;

namespace MetaParser.Rules
{
    /// <summary>
    /// Provides logic for detecting and consuming tokens
    /// </summary>
    /// <typeparam name="T">The value type being parsed</typeparam>
    public interface ITokenRule<T> where T : unmanaged, IEquatable<T>
    {
        public bool Check(IReadOnlyTokenizer<T> Tokenizer, IToken<T> Previous);
        public IToken<T>? Consume(ITokenizer<T> Tokenizer, IToken<T> Previous);
    }
}
