using MetaParser.Tokens;

namespace MetaParser.Rules
{
    /// <summary>
    /// Static rules always return the same token instance and detect matches by comparing the current stream to said tokens value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="ITokenRule{T}"/>
    public sealed record AutoRule<T> : ITokenRule<T>
        where T : unmanaged, IEquatable<T>
    {
        private readonly IToken<T> Instance;

        public AutoRule(IToken<T> instance)
        {
            Instance = instance;
            if (!Instance.Value.IsSingleSegment)
            {
                throw new ArgumentException($"{nameof(AutoRule<T>)} can only accept constant tokens whose value is wholly contained within a contiguous block of memory.");
            }
        }

        public bool Check(IReadOnlyTokenizer<T> Tokenizer, IToken<T> Previous)
        {
            return Tokenizer.GetReader().IsNext(Instance.Value.FirstSpan);
        }

        public IToken<T>? Consume(ITokenizer<T> Tokenizer, IToken<T> Previous)
        {
            var rd = Tokenizer.GetReader();
            rd.Advance(1);
            Tokenizer.Consume(ref rd);
            return Instance;
        }
    }
}
