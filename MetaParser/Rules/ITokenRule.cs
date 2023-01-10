using MetaParser.Tokens;

namespace MetaParser.Rules
{

    /// <summary>
    /// Provides logic for detecting and consuming tokens
    /// </summary>
    /// <typeparam name="TValue">The value type being parsed</typeparam>
    public interface ITokenRule<TData, TValue> : IParsingRule
        where TData : struct, IEquatable<TData>
        where TValue : unmanaged, IEquatable<TValue>
    {
        bool TryConsume(ITokenizer<TValue> Tokenizer, Token<TData, TValue>? Previous, out Token<TData, TValue>? Token);
    }
}
