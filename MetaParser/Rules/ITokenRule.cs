namespace MetaParser.Rules
{

    /// <summary>
    /// Provides logic for detecting and consuming tokens
    /// </summary>
    /// <typeparam name="TValue">The value type being parsed</typeparam>
    public interface ITokenRule<TEnum, TValue> : IParsingRule
        where TEnum : unmanaged
        where TValue : unmanaged, IEquatable<TValue>
    {
        bool TryConsume(ITokenizer<TValue> Tokenizer, TEnum? Previous, out TEnum TokenType, out long TokenLength);
    }
}
