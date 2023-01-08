using MetaParser.Parsing.Tokens;

namespace MetaParser.Parsing.Definitions
{
    /// <summary>
    /// Provides logic for detecting and consuming tokens
    /// </summary>
    /// <typeparam name="Ty">The value type being parsed</typeparam>
    public interface ITokenRule<Ty> where Ty : unmanaged, IEquatable<Ty>
    {
        public bool Check(IReadOnlyTokenizer<Ty> Tokenizer, IToken<Ty> Previous);
        public IToken<Ty>? Consume(ITokenizer<Ty> Tokenizer, IToken<Ty> Previous);
    }
}
