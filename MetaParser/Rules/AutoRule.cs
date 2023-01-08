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
        private readonly IStaticToken<T> Instance;

        public AutoRule(IStaticToken<T> instance)
        {
            Instance = instance;
        }

        public bool Check(IReadOnlyTokenizer<T> Tokenizer, IToken<T> Previous)
        {
            return Tokenizer.GetReader().IsNext(Instance.Value);
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
