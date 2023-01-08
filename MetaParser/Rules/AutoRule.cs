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

        public bool TryConsume(ITokenizer<T> Tokenizer, IToken<T> Previous, out IToken<T>? outToken)
        {
            var rd = Tokenizer.GetReader();
            if(rd.IsNext(Instance.Value, advancePast: true))
            {
                Tokenizer.Consume(ref rd);
                outToken = Instance;
                return true;
            }

            outToken = null;
            return false;
        }
    }
}
